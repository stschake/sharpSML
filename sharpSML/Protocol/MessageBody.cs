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

        [ChoiceCase(0x200)]
        public PublicClose? PublicCloseReq;
        [ChoiceCase(0x201)]
        public PublicClose? PublicCloseRes;

        [ChoiceCase(0x300)]
        public GetProfilePackReq? GetProfilePackReq;
        [ChoiceCase(0x301)]
        public GetProfilePackRes? GetProfilePackRes;
    }

}