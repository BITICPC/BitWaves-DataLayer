using System;
using System.Runtime.Serialization;

namespace BitWaves.Data.Utils
{
    /// <summary>
    /// 当尝试获取一个空的 <see cref="Maybe{T}"/> 值的内部值时抛出此异常。
    /// </summary>
    [Serializable]
    public sealed class EmptyMaybeException : Exception
    {
        /// <summary>
        /// 初始化 <see cref="EmptyMaybeException"/> 类的新实例。
        /// </summary>
        public EmptyMaybeException()
        {
        }

        /// <summary>
        /// 初始化 <see cref="EmptyMaybeException"/> 类的新实例。
        /// </summary>
        /// <param name="message">异常消息。</param>
        public EmptyMaybeException(string message) : base(message)
        {
        }

        /// <summary>
        /// 初始化 <see cref="EmptyMaybeException"/> 类的新实例。
        /// </summary>
        /// <param name="message">异常消息。</param>
        /// <param name="inner">引发当前异常的内部异常。</param>
        public EmptyMaybeException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// 从给定的序列化环境中初始化 <see cref="EmptyMaybeException"/> 类的新实例。
        /// </summary>
        /// <param name="info">序列化信息。</param>
        /// <param name="context">序列化环境的流上下文。</param>
        private EmptyMaybeException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
