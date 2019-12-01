using System;
using System.Collections.Generic;
using BitWaves.Data.Extensions;
using NUnit.Framework;

namespace BitWaves.Data.UnitTest
{
    /// <summary>
    /// 为 <see cref="TypeExtensions"/> 提供单元测试。
    /// </summary>
    public sealed class TypeExtensionTests
    {
        [Test]
        public void TestCanBeAssignedTypeNull()
        {
            Assert.Throws<ArgumentNullException>(() => TypeExtensions.CanBeAssigned(null, new object()));
        }

        [Test]
        public void TestCanBeAssignedValueTypeNullValue()
        {
            Assert.IsFalse(typeof(int).CanBeAssigned(null));
        }

        [Test]
        public void TestCanBeAssignedRefTypeNullValue()
        {
            Assert.IsTrue(typeof(object).CanBeAssigned(null));
            Assert.IsTrue(typeof(ICollection<int>).CanBeAssigned(null));
            Assert.IsTrue(typeof(Action).CanBeAssigned(null));
        }

        [Test]
        public void TestCanBeAssignedTypeMismatch()
        {
            Assert.IsFalse(typeof(DateTime).CanBeAssigned(10));
        }

        [Test]
        public void TestCanBeAssigned()
        {
            Assert.IsTrue(typeof(DateTime).CanBeAssigned(new DateTime()));
        }

        [Test]
        public void TestCanBeAssignedDerived()
        {
            Assert.IsTrue(typeof(IList<int>).CanBeAssigned(new List<int>()));
        }
    }
}
