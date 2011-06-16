using System.Collections.Generic;
using System.Runtime.InteropServices;
using sharpSML.Attributes;

namespace sharpSML.Protocol
{

    [Sequence]
    [StructLayout(LayoutKind.Sequential)]
    public struct Tree
    {
        public byte[] ParameterName;

        [Attributes.Optional]
        public ProcParValue? ParameterValue;

        [Attributes.Optional]
        public List<Tree> ChildList;
    }

}