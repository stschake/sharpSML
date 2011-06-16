using System.Runtime.InteropServices;
using sharpSML.Attributes;

namespace sharpSML.Protocol
{

    [Sequence]
    [StructLayout(LayoutKind.Sequential)]
    public struct Message
    {
        public byte[] TransactionId;
        public byte GroupNo;
        public AbortOnError AbortOnError;
        public MessageBody MessageBody;
        public ushort CRC16;

        [RawData(0x00)]
        public byte EndOfSmlMessage;
    }

}