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

using System.IO;
using NUnit.Framework;
using sharpSML;

namespace sharpSMLTest
{

    [TestFixture]
    public class TypeLengthTest
    {
        
        private TypeLength Read(params byte[] bytes)
        {
            return new TypeLength(new MemoryStream(bytes));
        }

        [Test]
        public void TestDecoding()
        {
            var tl = Read(0x52);
            Assert.AreEqual(SMLType.Integer, tl.Type);
            Assert.AreEqual(1, tl.Length);
        }

        [Test]
        public void TestIsOptionalMarker()
        {
            var tl = Read(0x01);
            Assert.IsTrue(tl.IsOptionalMarker);
        }
    
    }

}