using System.Runtime.InteropServices;
using sharpSML.Attributes;

namespace sharpSML.Protocol
{

    [Choice]
    [StructLayout(LayoutKind.Sequential)]
    public struct MessageBody
    {
        [ChoiceCase(0x100)]
        public PublicOpenReq? PublicOpenReq;

        [ChoiceCase(0x101)]
        public PublicOpenRes? PublicOpenRes;
    }

}