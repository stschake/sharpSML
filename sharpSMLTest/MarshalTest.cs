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

using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using sharpSML;
using sharpSML.Attributes;

namespace sharpSMLTest
{

    [TestFixture]
    public class MarshalTest
    {

        protected Marshal CreateMarshal(params byte[] data)
        {
            return new Marshal(new MemoryStream(data));
        }
        
        [Sequence]
        public struct SimpleSequence
        {
            public bool FieldA;
            public bool FieldB;
        }

        [Test]
        public void TestSimpleSequence()
        {
            var m = CreateMarshal(0x72, 0x42, 0x00, 0x42, 0x01);
            var ret = m.Read<SimpleSequence>();
            Assert.IsFalse(ret.FieldA);
            Assert.IsTrue(ret.FieldB);
        }

        [Sequence]
        public struct OptionalSequence
        {
            public int Important;

            [Optional]
            public int Unnecessary;
        }

        [Test]
        public void TestHandlesOptional()
        {
            var m = CreateMarshal(0x72, 0x53, 0x00, 0x01, 0x01);
            var ret = m.Read<OptionalSequence>();
            Assert.AreEqual(1, ret.Important);
            // default value of type
            Assert.AreEqual(0, ret.Unnecessary);
        }

        [Sequence]
        public struct NullableOptional
        {
            public int Important;

            [Optional]
            public int? Unnecessary;
        }

        [Test]
        public void TestHandlesOptionalWithNullable()
        {
            {
                var m = CreateMarshal(0x72, 0x53, 0x00, 0x01, 0x01);
                var ret = m.Read<NullableOptional>();
                Assert.AreEqual(1, ret.Important);
                Assert.IsNull(ret.Unnecessary);
            }

            {
                // test if marshal can correctly _set_ nullables
                var m = CreateMarshal(0x72, 0x53, 0x00, 0x01, 0x53, 0x00, 0x01);
                var ret = m.Read<NullableOptional>();
                Assert.AreEqual(1, ret.Important);
                Assert.IsTrue(ret.Unnecessary.HasValue);
                Assert.AreEqual(1, ret.Unnecessary.Value);
            }
        }

        [Sequence]
        public struct ImplicitOptional
        {
            public int Important;
            public int? Unnecessary;
        }

        [Test]
        public void TestTreatsNullablesAsOptional()
        {
            var m = CreateMarshal(0x72, 0x53, 0x00, 0x01, 0x01);
            var ret = m.Read<ImplicitOptional>();
            Assert.AreEqual(1, ret.Important);
            Assert.IsNull(ret.Unnecessary);
        }

        [Sequence]
        public struct Nested
        {
            [Sequence]
            public struct NestedA
            {
                public bool Value;
            }
            
            [Sequence]
            public struct NestedB
            {
                public bool Value;
            }

            public NestedA A;
            public NestedB B;
        }

        [Test]
        public void TestHandlesNestedStructures()
        {
            var m = CreateMarshal(0x72, 0x71, 0x42, 0x01, 0x71, 0x42, 0x01);
            var ret = m.Read<Nested>();

            Assert.IsTrue(ret.A.Value);
            Assert.IsTrue(ret.B.Value);
        }

        [Choice]
        public struct SimpleChoice
        {
            [ChoiceCase(0x1)]
            public bool? Boolean;

            [ChoiceCase(0x2)]
            public int? Integer;
        }

        [Test]
        public void TestSimpleChoice()
        {
            var m = CreateMarshal(0x72, 0x62, 0x02, 0x52, 0x05);
            var ret = m.Read<SimpleChoice>();
            Assert.IsFalse(ret.Boolean.HasValue);
            Assert.IsTrue(ret.Integer.HasValue);
            Assert.AreEqual(5, ret.Integer.Value);
        }

        [Sequence]
        public struct OctetTest
        {
            public byte[] Octets;
        }

        [Test]
        public void TestHandlesOctets()
        {
            var m = CreateMarshal(0x71, 0x05, 0x00, 0x01, 0x02, 0x03);
            var ret = m.Read<OctetTest>();
            Assert.IsNotNull(ret.Octets);
            Assert.AreEqual(4, ret.Octets.Length);
            Assert.AreEqual(new byte[]{0x00, 0x01, 0x02, 0x03}, ret.Octets);
        }

        [Sequence]
        public struct SequenceOfTest
        {
            public List<int> IntegerList;
        }

        [Test]
        public void TestHandlesSequenceOf()
        {
            var m = CreateMarshal(0x71, 0x73, 0x52, 0x00, 0x52, 0x01, 0x52, 0x02);
            var ret = m.Read<SequenceOfTest>();
            Assert.IsNotNull(ret.IntegerList);
            Assert.AreEqual(3, ret.IntegerList.Count);
            Assert.AreEqual(0x00, ret.IntegerList[0]);
            Assert.AreEqual(0x01, ret.IntegerList[1]);
            Assert.AreEqual(0x02, ret.IntegerList[2]);
        }

        [Sequence]
        public struct SequenceOfHead
        {
            [Sequence]
            public struct SequenceOfNode
            {
                public int One;
                public int Two;
            }

            public List<SequenceOfNode> Nodes;
        }

        [Test]
        public void TestHandlesNestedSequenceOf()
        {
            var m = CreateMarshal(0x71, 0x72, 0x72, 0x52, 0x01, 0x52, 0x02, 0x72, 0x52, 0x01, 0x52, 0x02);
            var ret = m.Read<SequenceOfHead>();
            Assert.IsNotNull(ret.Nodes);
            Assert.AreEqual(2, ret.Nodes.Count);
            for (int i = 0; i < 2; i++)
            {
                Assert.AreEqual(1, ret.Nodes[i].One);
                Assert.AreEqual(2, ret.Nodes[i].Two);
            }
        }

    }

}