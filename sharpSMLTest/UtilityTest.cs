using NUnit.Framework;
using sharpSML.Utility;

namespace sharpSMLTest
{

    [TestFixture]
    public class UtilityTest
    {
   
        [Test]
        public void TestEndiannessReverse()
        {
            Assert.AreEqual(0xBBAA, Endianness.Reverse((ushort)0xAABB));
            Assert.AreEqual(0xDDCCBBAA, Endianness.Reverse(0xAABBCCDD));
            Assert.AreEqual(0x7766554433221100, Endianness.Reverse(0x0011223344556677));
        }

    }

}