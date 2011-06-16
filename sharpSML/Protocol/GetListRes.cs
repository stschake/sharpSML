using System.Collections.Generic;
using System.Runtime.InteropServices;
using sharpSML.Attributes;

namespace sharpSML.Protocol
{

    [Sequence]
    [StructLayout(LayoutKind.Sequential)]
    public struct GetListRes
    {
        [Attributes.Optional]
        public byte[] ClientId;

        public byte[] ServerId;

        [Attributes.Optional]
        public byte[] ListName;

        [Attributes.Optional]
        public Time? ActSensorTime;

        public List<ListEntry> ValList;

        [Attributes.Optional]
        public byte[] ListSignature;

        [Attributes.Optional]
        public Time? ActGatewayTime;
    }

}