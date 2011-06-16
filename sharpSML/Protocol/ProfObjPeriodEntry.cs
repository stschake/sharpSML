using System.Collections.Generic;
using System.Runtime.InteropServices;
using sharpSML.Attributes;

namespace sharpSML.Protocol
{

    [Sequence]
    [StructLayout(LayoutKind.Sequential)]
    public struct ProfObjPeriodEntry
    {
        public Time ValTime;
        public ulong Status;
        public List<ValueEntry> ValueList;

        [Attributes.Optional]
        public byte[] PeriodSignature;
    }

}