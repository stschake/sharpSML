/*
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
using System.IO;

namespace sharpSML
{

    public enum SMLType : byte
    {
        OctetString = 0,
        Boolean = 4,
        Integer = 5,
        Unsigned = 6,
        List = 7
    }

    public class TypeLength
    {
        private const byte ExtraByteMask = 0x80;
        private const byte TypeMask = 0x70;
        private const byte LengthMask = 0x0F;

        public TypeLength(Stream source)
        {
            // TODO: missing support for TypeLengths that span multiple bytes

            var reader = new BinaryReader(source);
            var value = reader.ReadByte();

            if ((value & ExtraByteMask) != 0)
                throw new NotImplementedException("Found multi-byte TypeLength which is currently not supported");

            Type = (SMLType)((value & TypeMask) >> 4);
            // no idea why this oddity is necessary..
            Length = (value & LengthMask) - (Type != SMLType.List ? 1 : 0);
        }

        public int Length { get; private set; }
        public SMLType Type { get; private set; }

        public bool IsOptionalMarker
        {
            get
            {
                return Type == SMLType.OctetString && Length == 0;
            }
        }

    }

}