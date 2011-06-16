using System.Runtime.InteropServices;
using sharpSML.Attributes;

namespace sharpSML.Protocol
{

    /// <summary>
    /// originally called TupelEntry in the specification
    /// </summary>
    [Sequence]
    [StructLayout(LayoutKind.Sequential)]
    public struct TupleEntry
    {
        public byte[] ServerId;
        public Time SecIndex;
        public ulong Status;

        public byte UnitPA;
        public sbyte ScalerPA;
        public ulong ValuePA;
        public byte UnitR1;
        public sbyte ScalerR1;
        public ulong ValueR1;
        public byte UnitR4;
        public sbyte ScalerR4;
        public ulong ValueR4;
        public byte[] SignaturePAR1R4;

        public byte UnitMA;
        public sbyte ScalerMA;
        public ulong ValueMA;
        public byte UnitR2;
        public sbyte ScalerR2;
        public ulong ValueR2;
        public byte UnitR3;
        public sbyte ScalerR3;
        public ulong ValueR3;
        public byte[] SignatureMAR2R3;
    }

}