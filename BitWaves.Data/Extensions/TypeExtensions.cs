using System;

namespace BitWaves.Data.Extensions
{
    /// <summary>
    /// 为 <see cref="Type"/> 提供扩展方法。
    /// </summary>
    internal static class TypeExtensions
    {
        /// <summary>
        /// 检查是否可以将给定的值赋值给给定的类型。
        /// </summary>
        /// <param name="type">需要赋值到的类型。</param>
        /// <param name="value">赋值的值。</param>
        /// <returns>是否可以将给定的值赋值给给定的类型。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> 为 null。</exception>
        public static bool CanBeAssigned(this Type type, object value)
        {
            Contract.NotNull(type, nameof(type));

            if (value == null)
            {
                return type.IsInterface || type.IsClass;
            }

            return type.IsInstanceOfType(value);
        }
    }
}
