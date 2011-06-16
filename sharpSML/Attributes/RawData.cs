using System;

namespace sharpSML.Attributes
{

    /// <summary>
    /// RawData attribute will match provided data 1:1 with stream data at marshaling
    /// Not defined in the specification; used in sharpSML to fully read Message
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class RawData : Attribute
    {
        public byte[] Data { get; private set; }

        public RawData(byte value, params byte[] data)
        {
            // this is necessary to ensure at least one byte is provided
            Data = new byte[1 + data.Length];
            Data[0] = value;
            data.CopyTo(Data, 1);
        }
    }

}