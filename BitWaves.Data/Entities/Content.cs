using System;
using MongoDB.Bson;

namespace BitWaves.Data.Entities
{
    /// <summary>
    /// 表示一个静态内容。
    /// </summary>
    public sealed class Content
    {
        /// <summary>
        /// 初始化 <see cref="Content"/> 类的新实例。
        /// </summary>
        private Content()
        {
        }

        /// <summary>
        /// 初始化 <see cref="Content"/> 类的新实例。
        /// </summary>
        /// <param name="name">静态内容的名称。</param>
        /// <param name="mimeType">静态内容的 MIME 类型。</param>
        /// <param name="data">静态内容的原始数据。</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="name"/> 为 null
        ///     或
        ///     <paramref name="mimeType"/> 为 null
        ///     或
        ///     <paramref name="data"/> 为 null。
        /// </exception>
        public Content(string name, string mimeType, byte[] data)
        {
            Contract.NotNull(name, nameof(name));
            Contract.NotNull(mimeType, nameof(mimeType));
            Contract.NotNull(data, nameof(data));

            Id = ObjectId.GenerateNewId();
            Name = name;
            MimeType = mimeType;
            CreationTime = DateTime.UtcNow;
            Size = data.Length;
            Data = data;
        }

        /// <summary>
        /// 获取静态内容的全局唯一 ID。
        /// </summary>
        public ObjectId Id { get; private set; }

        /// <summary>
        /// 获取静态内容的名称。
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 获取静态内容的 MIME 类型。
        /// </summary>
        public string MimeType { get; private set; }

        /// <summary>
        /// 获取静态内容的创建时间。
        /// </summary>
        public DateTime CreationTime { get; private set; }

        /// <summary>
        /// 获取静态内容的字节长度。
        /// </summary>
        public long Size { get; private set; }

        /// <summary>
        /// 获取静态内容的原始数据。
        /// </summary>
        public byte[] Data { get; private set; }
    }
}
