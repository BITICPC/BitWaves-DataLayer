using System;
using BitWaves.Data.Utils;
using NUnit.Framework;

namespace BitWaves.Data.UnitTest
{
    /// <summary>
    /// 为 <see cref="ObjectPath"/> 提供单元测试。
    /// </summary>
    public sealed class ObjectPathTests
    {
        [Test]
        public void TestDefaultConstructor()
        {
            var path = new ObjectPath();
            Assert.IsEmpty(path.Path);
        }

        [Test]
        public void TestCustomConstructorNull()
        {
            Assert.Throws<ArgumentNullException>(() => new ObjectPath(null));
        }

        [Test]
        public void TestCustomConstructor()
        {
            var path = new ObjectPath("a.b.c.d");
            Assert.AreEqual("a.b.c.d", path.Path);
        }

        [Test]
        public void TestPushToEmptyPath()
        {
            var path = new ObjectPath();
            var pushed = path.Push("msr");
            Assert.IsEmpty(path.Path);
            Assert.AreEqual("msr", pushed.Path);
        }

        [Test]
        public void TestPushToNonEmptyPath()
        {
            var path = new ObjectPath("a.b.c.d");
            var pushed = path.Push("e");
            Assert.AreEqual("a.b.c.d", path.Path);
            Assert.AreEqual("a.b.c.d.e", pushed.Path);
        }

        [Test]
        public void TestToString()
        {
            var path = new ObjectPath("a.b.c.d");
            Assert.AreEqual(path.Path, path.ToString());
        }

        [Test]
        public void TestGetRoot()
        {
            var root = ObjectPath.Root;
            Assert.IsEmpty(root.Path);
        }
    }
}
