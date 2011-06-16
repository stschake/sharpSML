namespace sharpSML.Utility
{

    internal static class Endianness
    {

        public static short Reverse(short value)
        {
            return (short)Reverse((ushort)value);
        }

        public static int Reverse(int value)
        {
            return (int)Reverse((uint)value);
        }

        public static long Reverse(long value)
        {
            return (long)Reverse((ulong)value);
        }

        public static ushort Reverse(ushort value)
        {
            return (ushort)(((value & 0x00FF) << 8) | ((value & 0xFF00) >> 8));
        }

        public static uint Reverse(uint value)
        {
            return ((value & 0x000000FF) << 24) |
                ((value & 0x0000FF00) << 8) |
                ((value & 0x00FF0000) >> 8) |
                ((value & 0xFF000000) >> 24);
        }

        public static ulong Reverse(ulong value)
        {
            return ((value & 0x00000000000000FFUL) << 56) |
                ((value & 0x000000000000FF00UL) << 40) |
                ((value & 0x0000000000FF0000UL) << 24) |
                ((value & 0x00000000FF000000UL) << 8) |
                ((value & 0x000000FF00000000UL) >> 8) |
                ((value & 0x0000FF0000000000UL) >> 24) |
                ((value & 0x00FF000000000000UL) >> 40) |
                ((value & 0xFF00000000000000UL) >> 56);
        }



    }

}