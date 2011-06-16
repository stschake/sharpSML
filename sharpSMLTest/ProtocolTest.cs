using System.IO;
using NUnit.Framework;
using sharpSML;
using sharpSML.Protocol;

namespace sharpSMLTest
{

    [TestFixture]
    public class ProtocolTest
    {

        protected MemoryStream OpenResource(string name)
        {
            return OpenResource(name, 0);
        }

        protected MemoryStream OpenResource(string name, int offset)
        {
            var path = "Resources\\" + name;
            Assert.IsTrue(File.Exists(path), "Resource not found: " + name);
            var ms = new MemoryStream(File.ReadAllBytes(path));
            ms.Seek(offset, SeekOrigin.Begin);
            return ms;
        }

        [Test]
        public void TestMessage()
        {
            // skip escape sequence
            var res = OpenResource("capture.bin", 8);
            var marshal = new Marshal(res);
            var message = marshal.Read<Message>();
            Assert.AreEqual(0xdad6, message.CRC16);
        }

    }

}