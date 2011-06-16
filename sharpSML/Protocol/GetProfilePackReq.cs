using System.Collections.Generic;
using System.Runtime.InteropServices;
using sharpSML.Attributes;

namespace sharpSML.Protocol
{

    [Sequence]
    [StructLayout(LayoutKind.Sequential)]
    public struct GetProfilePackReq
    {
        [Attributes.Optional]
        public byte[] ServerId;

        [Attributes.Optional]
        public byte[] Username;

        [Attributes.Optional]
        public byte[] Password;

        [Attributes.Optional]
        public bool WithRawdata;

        [Attributes.Optional]
        public Time? BeginTime;

        [Attributes.Optional]
        public Time? EndTime;

        public List<byte[]> ParameterTreePath;

        [Attributes.Optional]
        public List<byte[]> ObjectList;

        [Attributes.Optional]
        public Tree DasDetails;
    }

}