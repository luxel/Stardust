namespace Stardust.Test
{
    using UnityEngine;
    using System.Collections;

    public static class TestUtilities
    {
        public static bool ByteDataEquals(byte[] data1, byte[] data2)
        {
            if (data1.Length != data2.Length)
            {
                return false;
            }
            for (int i = 0; i < data1.Length;i ++)
            {
                if (data1[i] != data2[i])
                {
                    return false;
                }
            }
            return true;
        }

        public static byte[] GetRandomData(int length)
        {
            byte[] data = new byte[length];
            (new System.Random()).NextBytes(data);
            return data;
        }
    }

}