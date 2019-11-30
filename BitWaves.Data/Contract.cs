using System;

namespace BitWaves.Data
{
    /// <summary>
    /// 提供常用的前置约束检查。
    /// </summary>
    internal static class Contract
    {
        /// <summary>
        /// 检查给定的引用类型的值不为 null，否则抛出 <see cref="ArgumentNullException"/>。
        /// </summary>
        /// <param name="value">要检查的值。</param>
        /// <param name="variableName">变量名称。</param>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> 为 null。</exception>
        public static void NotNull(object value, string variableName)
        {
            if (value == null)
                throw new ArgumentNullException(variableName);
        }

        /// <summary>
        /// 检查给定的字符串不为空串。
        /// </summary>
        /// <param name="value">要检查的字符串。</param>
        /// <param name="message"><see cref="ArgumentException"/> 的异常信息。</param>
        /// <param name="variableName">变量名称。</param>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> 为 null。</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> 为空串。</exception>
        public static void NotNullOrEmpty(string value, string message, string variableName)
        {
            NotNull(value, variableName);

            if (value.Length == 0)
                throw new ArgumentException(message, variableName);
        }

        /// <summary>
        /// 检查给定的值是否不为负值。如果给定的值为负值，抛出 <see cref="ArgumentOutOfRangeException"/> 异常。
        /// </summary>
        /// <param name="value">要检查的值。</param>
        /// <param name="message">异常消息。</param>
        /// <param name="variableName">要检查的值在调用方处的变量名。</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="value"/> 的值小于零。
        /// </exception>
        public static void NonNegative(long value, string message, string variableName)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(variableName, message);
        }

        /// <summary>
        /// 检查给定的值是否为正。如果给定的值不为正，抛出 <see cref="ArgumentOutOfRangeException"/> 异常。
        /// </summary>
        /// <param name="value">要检查的值。</param>
        /// <param name="message">异常消息。</param>
        /// <param name="variableName">要检查的值在调用方处的变量名。</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="value"/> 的值小于或等于零。
        /// </exception>
        public static void Positive(long value, string message, string variableName)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(variableName, message);
        }
    }
}
