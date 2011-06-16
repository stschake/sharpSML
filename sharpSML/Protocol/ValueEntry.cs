using System.Runtime.InteropServices;
using sharpSML.Attributes;

namespace sharpSML.Protocol
{

    [Sequence]
    [StructLayout(LayoutKind.Sequential)]
    public struct ValueEntry
    {
        public object Value;

        [Attributes.Optional]
        public byte[] ValueSignature;
    }

}