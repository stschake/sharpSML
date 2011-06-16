using System.Collections.Generic;
using System.Runtime.InteropServices;
using sharpSML.Attributes;

namespace sharpSML.Protocol
{

    [Sequence]
    [StructLayout(LayoutKind.Sequential)]
    public struct GetProfilePackRes
    {
        public byte[] ServerId;
        public Time ActTime;
        public uint RegPeriod;
        public List<byte[]> ParameterTreePath;
        public List<ProfObjHeaderEntry> HeaderList;
        public List<ProfObjPeriodEntry> PeriodList;

        [Attributes.Optional]
        public byte[] Rawdata;

        [Attributes.Optional]
        public byte[] ProfileSignature;
    }

}