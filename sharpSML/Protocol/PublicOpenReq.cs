using System.Runtime.InteropServices;
using sharpSML.Attributes;

namespace sharpSML.Protocol
{

    [Sequence]
    [StructLayout(LayoutKind.Sequential)]
    public struct PublicOpenReq
    {
        [Attributes.Optional]
        public byte[] Codepage;

        public byte[] ClientId;
        public byte[] ReqFileId;

        [Attributes.Optional]
        public byte[] ServerId;

        [Attributes.Optional]
        public byte[] Username;

        [Attributes.Optional]
        public byte[] Password;

        [Attributes.Optional]
        public byte? SMLVersion;
    }

}