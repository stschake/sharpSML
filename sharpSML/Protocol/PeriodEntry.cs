using System.Runtime.InteropServices;
using sharpSML.Attributes;

namespace sharpSML.Protocol
{

    [Sequence]
    [StructLayout(LayoutKind.Sequential)]
    public struct PeriodEntry
    {
        public byte[] ObjName;
        public byte Unit;
        public sbyte Scaler;
        public object Value;

        [Attributes.Optional]
        public byte[] ValueSignature;
    }

}