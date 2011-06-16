using System.Runtime.InteropServices;
using sharpSML.Attributes;

namespace sharpSML.Protocol
{
    
    [Sequence]
    [StructLayout(LayoutKind.Sequential)]
    public struct PublicOpenRes
    {
        [Attributes.Optional]
        public byte[] Codepage;

        [Attributes.Optional]
        public byte[] ClientId;

        public byte[] ReqFileId;
        public byte[] ServerId;

        [Attributes.Optional]
        public Time? RefTime;

        [Attributes.Optional]
        public byte? SMLVersion;
    }

}