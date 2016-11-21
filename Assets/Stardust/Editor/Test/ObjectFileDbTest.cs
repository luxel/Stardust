#if UNITY_EDITOR
namespace Stardust.Test
{
    using UnityEngine;
    using System.Collections;
    using NUnit.Framework;
    using System;
    using Stardust;
    using ProtoBuf;

    public class ObjectFileDbTest
    {
        private int RandomItemCount = 10;

        [Test]
        public void TestBinary()
        {
            RunTestCasesOnDb(new TestBinaryDb(), new TestBinaryDb());
        }

        [Test]
        public void TestJson()
        {
            RunTestCasesOnDb(new TestTextDb(), new TestTextDb());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer">The db instance used to write and modify items.</param>
        /// <param name="reader">The db isntance used to read files and validate.</param>
        private void RunTestCasesOnDb(IObjectFileDb<int, TestItem> writer, IObjectFileDb<int, TestItem> reader)
        {
            int testItemId = 1;
            string testItemValue = "test";
            var item = new TestItem(testItemId, testItemValue);

            writer.Load();
            writer.Clear();
            writer.Save();

            reader.Load();
            Assert.AreEqual(0, reader.Count);

            writer.Add(item);
            
            for (int i = 0; i < RandomItemCount; i ++)
            {
                writer.Add(new TestItem(i + 2, "random"));
            }
            writer.Save();

            reader.Load();
            Assert.AreEqual(RandomItemCount + 1, reader.Count);
            Assert.NotNull(reader.Find(testItemId));
            Assert.AreEqual(testItemValue, reader.Find(testItemId).StringValue);

            writer.Clear();
            writer.Save();

            reader.Load();
            Assert.AreEqual(0, reader.Count);

            writer.Add(item);
            writer.Save();

            reader.Load();
            Assert.AreEqual(1, reader.Count);
            Assert.NotNull(reader.Find(testItemId));
            Assert.AreEqual(testItemValue, reader.Find(testItemId).StringValue);
        }
    }

    public class TestBinaryDb : ObjectBinaryFileDb<int, TestItem>
    {
        public TestBinaryDb()
            : base("test_bin")
        {
        }
    }

    public class TestTextDb : ObjectTextFileDb<int, TestItem>
    {
        public TestTextDb()
            : base("test_json")
        {
        }
    }

    [Serializable]
    [ProtoContract]
    public class TestItem : IObjectWithId<int>
    {
        public int Id
        {
            get { return IntId; }
        }

        [ProtoMember(1)]
        public int IntId;

        [ProtoMember(2)]
        public string StringValue;

        public TestItem()
        {

        }

        public TestItem(int id, string value)
        {
            IntId = id;
            StringValue = value;
        }
    }
} 
#endif