using System.Runtime.InteropServices;
using sharpSML.Attributes;

namespace sharpSML.Protocol
{

    [Sequence]
    [StructLayout(LayoutKind.Sequential)]
    public struct ListEntry
    {
        public byte[] ObjName;

        [Attributes.Optional]
        public ulong Status;

        [Attributes.Optional]
        public Time? ValTime;

        [Attributes.Optional]
        public byte? Unit;

        [Attributes.Optional]
        public sbyte Scaler;

        public object Value;

        [Attributes.Optional]
        public byte[] ValueSignature;
    }

}