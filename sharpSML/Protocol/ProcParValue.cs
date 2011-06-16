using sharpSML.Attributes;

namespace sharpSML.Protocol
{

    [Choice]
    public struct ProcParValue
    {
        [ChoiceCase(0x01)]
        public object SMLValue;

        [ChoiceCase(0x02)]
        public PeriodEntry? SMLPeriodEntry;

        [ChoiceCase(0x03)]
        public TupleEntry? SMLTupleEntry;

        [ChoiceCase(0x04)]
        public Time SMLTime;
    }

}