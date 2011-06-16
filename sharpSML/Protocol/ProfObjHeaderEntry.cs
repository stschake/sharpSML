using System.Runtime.InteropServices;
using sharpSML.Attributes;

namespace sharpSML.Protocol
{

    [Sequence]
    [StructLayout(LayoutKind.Sequential)]
    public struct ProfObjHeaderEntry
    {
        public byte[] ObjName;
        public byte Unit;
        public sbyte Scaler;
    }

}