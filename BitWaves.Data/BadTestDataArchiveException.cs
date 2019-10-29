using System;
using System.Runtime.Serialization;

namespace BitWaves.Data
{
    /// <summary>
    /// 当测试数据集文件格式不正确时抛出。
    /// </summary>
    [Serializable]
    public class BadTestDataArchiveException : Exception
    {
        /// <summary>
        /// 初始化 <see cref="BadTestDataArchiveException"/> 类的新实例。
        /// </summary>
        public BadTestDataArchiveException()
        {
        }

        /// <summary>
        /// 初始化 <see cref="BadTestDataArchiveException"/> 类的新实例。
        /// </summary>
        /// <param name="message">异常消息。</param>
        public BadTestDataArchiveException(string message) : base(message)
        {
        }

        /// <summary>
        /// 初始化 <see cref="BadTestDataArchiveException"/> 类的新实例。
        /// </summary>
        /// <param name="message">异常消息。</param>
        /// <param name="inner">引发当前异常的内部异常。</param>
        public BadTestDataArchiveException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// 从给定的序列化环境中反序列化 <see cref="BadTestDataArchiveException"/> 类的实例对象。
        /// </summary>
        /// <param name="info">序列化信息。</param>
        /// <param name="context">序列化上下文。</param>
        protected BadTestDataArchiveException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
