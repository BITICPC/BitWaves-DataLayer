using System;
using BitWaves.Data.Utils;
using NUnit.Framework;

namespace BitWaves.Data.UnitTest
{
    /// <summary>
    /// 为 <see cref="Maybe{T}"/> 以及相关服务提供单元测试。
    /// </summary>
    public sealed class MaybeTests
    {
        [Test]
        public void TestDefaultConstructor()
        {
            var maybe = new Maybe<int>();
            Assert.IsFalse(maybe.HasValue);
            Assert.Throws<EmptyMaybeException>(() =>
            {
                var _ = maybe.Value;
            });
        }

        [Test]
        public void TestCustomConstructor()
        {
            var maybe = new Maybe<int>(10);
            Assert.IsTrue(maybe.HasValue);
            Assert.AreEqual(10, maybe.Value);
        }

        [Test]
        public void TestSetValue()
        {
            var maybe = new Maybe<int>();
            maybe.Value = 10;

            Assert.IsTrue(maybe.HasValue);
            Assert.AreEqual(10, maybe.Value);
        }

        [Test]
        public void TestReplaceValue()
        {
            var maybe = new Maybe<int>(10);
            maybe.Value = 12;

            Assert.IsTrue(maybe.HasValue);
            Assert.AreEqual(12, maybe.Value);
        }

        [Test]
        public void TestGetEmpty()
        {
            var empty = Maybe<int>.Empty;
            Assert.IsFalse(empty.HasValue);
        }

        [Test]
        public void TestImplicitTypeConverter()
        {
            var maybe = (Maybe<int>) 10;
            Assert.IsTrue(maybe.HasValue);
            Assert.AreEqual(10, maybe.Value);
        }

        [Test]
        public void TestIsMaybeTypeNull()
        {
            Assert.Throws<ArgumentNullException>(() => MaybeUtils.IsMaybeType(null));
        }

        [Test]
        public void TestIsMaybeTypeTrue()
        {
            Assert.IsTrue(MaybeUtils.IsMaybeType(typeof(Maybe<object>)));
            Assert.IsTrue(MaybeUtils.IsMaybeType(typeof(Maybe<int>)));
        }

        [Test]
        public void TestIsMaybeTypeFalse()
        {
            Assert.IsFalse(MaybeUtils.IsMaybeType(typeof(object)));
            Assert.IsFalse(MaybeUtils.IsMaybeType(typeof(Maybe<>)));
        }

        [Test]
        public void TestIsMaybeTypeGenericNull()
        {
            Assert.Throws<ArgumentNullException>(() => MaybeUtils.IsMaybeType<object>(null));
        }

        [Test]
        public void TestIsMaybeTypeGenericTrue()
        {
            Assert.IsTrue(MaybeUtils.IsMaybeType<int>(typeof(Maybe<int>)));
            Assert.IsTrue(MaybeUtils.IsMaybeType<DateTime>(typeof(Maybe<DateTime>)));
        }

        [Test]
        public void TestIsMaybeTypeGenericFalse()
        {
            Assert.IsFalse(MaybeUtils.IsMaybeType<object>(typeof(object)));
            Assert.IsFalse(MaybeUtils.IsMaybeType<object>(typeof(Maybe<>)));
            Assert.IsFalse(MaybeUtils.IsMaybeType<int>(typeof(Maybe<double>)));
            Assert.IsFalse(MaybeUtils.IsMaybeType<object>(typeof(Maybe<Type>)));
        }

        [Test]
        public void TestGetInnerTypeNull()
        {
            Assert.Throws<ArgumentNullException>(() => MaybeUtils.GetInnerType(null));
        }

        [Test]
        public void TestGetInnerTypeNonMaybe()
        {
            Assert.Throws<ArgumentException>(() => MaybeUtils.GetInnerType(typeof(int)));
            Assert.Throws<ArgumentException>(() => MaybeUtils.GetInnerType(typeof(Maybe<>)));
        }

        [Test]
        public void TestGetInnerType()
        {
            Assert.AreEqual(typeof(int), MaybeUtils.GetInnerType(typeof(Maybe<int>)));
        }

        [Test]
        public void TestIsMaybeNull()
        {
            Assert.Throws<ArgumentNullException>(() => MaybeUtils.IsMaybe(null));
        }

        [Test]
        public void TestIsMaybeTrue()
        {
            Assert.IsTrue(MaybeUtils.IsMaybe(new Maybe<int>()));
            Assert.IsTrue(MaybeUtils.IsMaybe(new Maybe<object>(new object())));
        }

        [Test]
        public void TestIsMaybeFalse()
        {
            Assert.IsFalse(MaybeUtils.IsMaybe(10));
        }

        [Test]
        public void TestIsMaybeGenericNull()
        {
            Assert.Throws<ArgumentNullException>(() => MaybeUtils.IsMaybe<object>(null));
        }

        [Test]
        public void TestIsMaybeGenericTrue()
        {
            Assert.IsTrue(MaybeUtils.IsMaybe<int>(new Maybe<int>()));
            Assert.IsTrue(MaybeUtils.IsMaybe<int>(new Maybe<int>(10)));
        }

        [Test]
        public void TestIsMaybeGenericFalse()
        {
            Assert.IsFalse(MaybeUtils.IsMaybe<int>(10));
            Assert.IsFalse(MaybeUtils.IsMaybe<int>(new Maybe<short>(10)));
        }

        [Test]
        public void TestUnboxNull()
        {
            Assert.Throws<ArgumentNullException>(() => MaybeUtils.Unbox(null));
        }

        [Test]
        public void TestUnboxNonMaybe()
        {
            Assert.Throws<ArgumentException>(() => MaybeUtils.Unbox(10));
        }

        [Test]
        public void TestUnboxEmpty()
        {
            var maybe = MaybeUtils.Unbox(new Maybe<int>());
            Assert.IsFalse(maybe.HasValue);
        }

        [Test]
        public void TestUnbox()
        {
            var maybe = MaybeUtils.Unbox(new Maybe<int>(10));
            Assert.IsTrue(maybe.HasValue);
            Assert.IsInstanceOf<int>(maybe.Value);
            Assert.AreEqual(10, maybe.Value);
        }
    }
}
