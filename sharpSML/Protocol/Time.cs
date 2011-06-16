using System.Runtime.InteropServices;
using sharpSML.Attributes;

namespace sharpSML.Protocol
{

    [Choice]
    [StructLayout(LayoutKind.Sequential)]
    public struct Time
    {
        [ChoiceCase(0x01)]
        public uint? SecIndex;

        [ChoiceCase(0x02)]
        public uint? Timestamp;
    }

}