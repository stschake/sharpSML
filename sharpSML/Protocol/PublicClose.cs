using System.Runtime.InteropServices;
using sharpSML.Attributes;

namespace sharpSML.Protocol
{

    [Sequence]
    [StructLayout(LayoutKind.Sequential)]
    public struct PublicClose
    {
        [Attributes.Optional]
        public byte[] GlobalSignature;
    }

}