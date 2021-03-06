﻿/*
    This file is part of sharpSML, Copyright 2011 Stefan Schake.

    sharpSML is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    sharpSML is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with sharpSML.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using sharpSML.Attributes;
using sharpSML.Utility;

namespace sharpSML
{

    public class Marshal
    {
        private readonly BinaryReader _reader;

        public Stream Source { get; private set; }

        public Marshal(Stream source)
        {
            Source = source;
            _reader = new BinaryReader(source);
        }

        private static int NextPow2(int v)
        {
            v--;
            v |= v >> 1;
            v |= v >> 2;
            v |= v >> 4;
            v |= v >> 8;
            v |= v >> 16;
            v++;
            return v;
        }

        protected byte[] FillPow2(TypeLength tl)
        {
            int resLength = NextPow2(tl.Length);
            if (tl.Length == 1 || tl.Length == resLength)
                return _reader.ReadBytes(tl.Length);

            var res = new byte[resLength];
            _reader.ReadBytes(tl.Length).CopyTo(res, resLength - tl.Length);

            /*  the specification is erroneous on this
                it allows _leading_ all-ones or all-zeros bytes to be omitted
                since it mandates big-endian, and in signed integers the sign will be in the MSB
                it can't possibly omit any bytes */

            for (int i = 0; i < (resLength - tl.Length); i++)
                res[i] = 0x00;

            return res;
        }

        protected object ReadPOD()
        {
            return ReadPOD(false);
        }

        protected object ReadPOD(bool optional)
        {
            var tl = new TypeLength(Source);

            if (optional && tl.IsOptionalMarker)
                return null;

            switch (tl.Type)
            {
                case SMLType.Boolean:
                    return _reader.ReadByte() != 0;

                case SMLType.OctetString:
                    return _reader.ReadBytes(tl.Length);

                case SMLType.Integer:
                    {
                        var raw = FillPow2(tl);
                        if (raw.Length == 1)
                            return (sbyte)raw[0];
                        if (raw.Length == 2)
                            return Endianness.Reverse(BitConverter.ToInt16(raw, 0));
                        if (raw.Length == 4)
                            return Endianness.Reverse(BitConverter.ToInt32(raw, 0));
                        if (raw.Length == 8)
                            return Endianness.Reverse(BitConverter.ToInt64(raw, 0));
                        throw new InvalidDataException("Invalid integer, expected 1, 2, 4 or 8 bytes, got " + raw.Length);
                    }

                case SMLType.Unsigned:
                    {
                        var raw = FillPow2(tl);
                        if (raw.Length == 1)
                            return raw[0];
                        if (raw.Length == 2)
                            return Endianness.Reverse(BitConverter.ToUInt16(raw, 0));
                        if (raw.Length == 4)
                            return Endianness.Reverse(BitConverter.ToUInt32(raw, 0));
                        if (raw.Length == 8)
                            return Endianness.Reverse(BitConverter.ToUInt64(raw, 0));
                        throw new InvalidDataException("Invalid unsigned, expected 1, 2, 4 or 8 bytes, got " + raw.Length);
                    }

                case SMLType.List:
                    // we don't handle this directly, as in recursive
                    // we could, but it doesn't fit into the reflection/type model
                    throw new InvalidDataException("ReadPOD is only for POD types");
            }

            throw new InvalidDataException("Invalid TypeLength");
        }

        protected object ReadElement(Type type)
        {
            return ReadElement(type, false);
        }

        protected object ReadElement(Type type, bool optional)
        {
            if (type.GetCustomAttributes(false).Any(attribute => attribute is Sequence || attribute is Choice))
                return Read(type);

            var podValue = ReadPOD(optional);
            if (podValue == null)
                return null;
            
            // ChangeType doesn't support enums
            if (type.IsEnum)
                return Enum.ToObject(type, podValue);

            var podValueType = podValue.GetType();
            if (podValueType != type && !podValueType.IsSubclassOf(type))
                return Convert.ChangeType(podValue, type);

            return podValue;
        }

        protected void FillField(object obj, FieldInfo field)
        {
            var rawDataAttributes = field.GetCustomAttributes(typeof(RawData), false);
            if (rawDataAttributes.Length == 1)
            {
                var data = (rawDataAttributes[0] as RawData).Data;
                var dataRead = _reader.ReadBytes(data.Length);
                if (!data.SequenceEqual(dataRead))
                    throw new InvalidDataException("RawData attribute data and stream data mismatch");
                // we don't bother filling the field with anything
                // RawData fields are only there to facilitate parsing
                return;
            }

            var isList = field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof(List<>);
            if (isList)
            {
                var tl = new TypeLength(Source);
                if (tl.Type != SMLType.List)
                    throw new InvalidDataException("Expected List");
                if (tl.IsOptionalMarker)
                    return;

                var underlyingType = field.FieldType.GetGenericArguments()[0];
                var list = Activator.CreateInstance(field.FieldType, tl.Length) as IList;
                for (int i = 0; i < tl.Length; i++)
                    list.Add(ReadElement(underlyingType));

                field.SetValue(obj, list);
            }
            else
            {
                var underlyingType = Nullable.GetUnderlyingType(field.FieldType);
                var isNullable = underlyingType != null;
                var isOptional = isNullable || (field.GetCustomAttributes(false).Any(attribute => attribute is Optional));
                var value = ReadElement(isNullable ? underlyingType : field.FieldType, isOptional);

                // optional and not present in stream
                if (value == null)
                    return;

                field.SetValue(obj, value);
            }
        }

        protected object HandleSequence(Type type, TypeLength tl)
        {
            if (tl.Type != SMLType.List)
                throw new InvalidDataException("Expected list");

            var fieldCount = tl.Length;
            var fields = type.GetFields();
            if (fields.Count() != fieldCount)
                throw new InvalidDataException("Read list with " + fieldCount + " items, expected " + fields.Count());

            var ret = Activator.CreateInstance(type);
            foreach (var field in fields)
                FillField(ret, field);

            return ret;
        }
        
        protected object HandleChoice(Type type, TypeLength tl)
        {
            if (tl.Type != SMLType.List || tl.Length != 2)
                throw new InvalidDataException("Invalid Choice; expected list with two items (tag and element)");

            var tag = (uint)Convert.ChangeType(ReadPOD(), typeof(uint));
            var fields = type.GetFields();

            foreach (var field in fields)
            {
                var cases = field.GetCustomAttributes(typeof(ChoiceCase), false);

                // ignore fields without a ChoiceCase
                if (cases.Length <= 0)
                    continue;

                // throw on fields with more than one case; possible, but not in the specification
                if (cases.Length > 1)
                    throw new ArgumentException("Type contains field (" + field.Name + ") with more than one ChoiceCase");

                if ((cases[0] as ChoiceCase).Tag != tag)
                    continue;

                // we have the correct field
                var ret = Activator.CreateInstance(type);
                FillField(ret, field);
                return ret;
            }

            throw new InvalidDataException("No case found for tag " + tag);
        }

        public object Read(Type type)
        {
            var attributes = type.GetCustomAttributes(false);

            var tl = new TypeLength(Source);
            if (tl.IsOptionalMarker)
                return null;

            if (attributes.Any(attribute => attribute is Sequence))
                return HandleSequence(type, tl);

            if (attributes.Any(attribute => attribute is Choice))
                return HandleChoice(type, tl);

            throw new ArgumentException("Type doesn't use any of the SML attributes");
        }

        public T Read<T>() where T : struct
        {
            return (T) Read(typeof(T));
        }
    }

}