namespace Stardust
{
    using UnityEngine;
    using System.Collections;
    using System.IO;

    public class XorCipher
    {
        public static void Encode(Stream input, Stream output, byte[] key)
        {
            int i = 0;
            while(input.CanRead)
            {
                output.WriteByte(EncodeByte((byte)input.ReadByte(), key, i++));
            }
        }

        public static byte[] Encode(byte[] data, byte[] key)
        {
            int length = data.Length;
            byte[] encodedData = new byte[length];

            for (int i = 0; i < length; i ++)
            {
                encodedData[i] = EncodeByte(data[i], key, i);
            }

            return encodedData;
        }

        private static byte EncodeByte(byte data, byte[] key, int index)
        {
            return (byte)((uint)data ^ (uint)key[index % key.Length]);
        }
    }
}