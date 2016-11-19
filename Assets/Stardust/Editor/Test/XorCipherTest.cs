namespace Stardust.Test
{
    using UnityEngine;
    using UnityEditor;
    using NUnit.Framework;

    public class XorCipherTest
    {
        [Test]
        public void TestByteEncoding()
        {
            byte[] data = TestUtilities.GetRandomData(10000);
            byte[] key = TestUtilities.GetRandomData(20);

            byte[] encoded = XorCipher.Encode(data, key);

            Assert.AreEqual(data.Length, encoded.Length);
            Assert.False(TestUtilities.ByteDataEquals(encoded, data));

            Assert.True(TestUtilities.ByteDataEquals(XorCipher.Encode(encoded, key), data));
        }
    }

}