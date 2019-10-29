using System;
using NUnit.Framework;

namespace BitWaves.Data.UnitTest
{
    /// <summary>
    /// 为 <see cref="PathUtils"/> 类提供单元测试逻辑。
    /// </summary>
    public sealed class PathUtilsTests
    {
        [Test]
        public void TestRemoveExtensionNull()
        {
            Assert.Throws<ArgumentNullException>(() => PathUtils.RemoveExtension(null));
        }

        [Test]
        public void TestRemoveExtensionNoExtension()
        {
            Assert.AreEqual("abc/def", PathUtils.RemoveExtension("abc/def"));
        }

        [Test]
        public void TestRemoveExtension()
        {
            Assert.AreEqual("abc/def", PathUtils.RemoveExtension("abc/def.cpp"));
            Assert.AreEqual("abc/def.cpp", PathUtils.RemoveExtension("abc/def.cpp.axx"));
        }
    }
}
