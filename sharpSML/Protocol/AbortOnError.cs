namespace sharpSML.Protocol
{

    public enum AbortOnError : byte
    {
        Continue = 0x00,
        ContinueNextGroup = 0x01,
        ContinueCurrentGroup = 0x02,
        AbortImmediately = 0xFF
    }

}