using System;
using System.Linq;
using BitWaves.Data.Utils;
using NUnit.Framework;

namespace BitWaves.Data.UnitTest
{
    /// <summary>
    /// 为 <see cref="BufferUtils"/> 类提供单元测试逻辑。
    /// </summary>
    public sealed class BufferUtilsTests
    {
        [Test]
        public void TestEqualsNull()
        {
            Assert.IsTrue(BufferUtils.Equals(null, null));
            Assert.IsFalse(BufferUtils.Equals(null, new byte[0]));
            Assert.IsFalse(BufferUtils.Equals(new byte[0], null));
        }

        [Test]
        public void TestEqualsNonNull()
        {
            var bufferLengths = new[] { 0, 1, 2, 3, 4, 5, 7, 8, 9, 15, 16, 17, 100, 200 };

            var rnd = new Random();
            foreach (var len in bufferLengths)
            {
                var lhs = new byte[len];
                rnd.NextBytes(lhs);

                // 测试 Equals(...) 返回 true 的情况
                var rhs = (byte[]) lhs.Clone();
                Assert.IsTrue(BufferUtils.Equals(lhs, rhs));

                if (len != 0)
                {
                    // 测试两数组长度相同但内容不同的情况
                    rhs = new byte[len];
                    do
                    {
                        rnd.NextBytes(rhs);
                    } while (lhs.SequenceEqual(rhs));
                    Assert.IsFalse(BufferUtils.Equals(lhs, rhs));
                }

                // 测试两数组长度不同的情况
                rhs = new byte[len + rnd.Next(1, 20)];
                rnd.NextBytes(rhs);
                Assert.IsFalse(BufferUtils.Equals(lhs, rhs));
                Assert.IsFalse(BufferUtils.Equals(rhs, lhs));
            }
        }
    }
}
