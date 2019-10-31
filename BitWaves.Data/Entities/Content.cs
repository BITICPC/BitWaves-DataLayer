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
        /// 获取静态内容的全局唯一 ID。
        /// </summary>
        public ObjectId Id { get; private set; }

        /// <summary>
        /// 获取或设置静态内容的名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取静态内容的 MIME 类型。
        /// </summary>
        public string MimeType { get; set; }

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
        public byte[] Data { get; set; }

        /// <summary>
        /// 创建一个新的 <see cref="Content"/> 实例对象。
        /// </summary>
        /// <param name="name">静态内容的名称。</param>
        /// <param name="mimeType">静态内容的 MIME 类型。</param>
        /// <param name="data">静态内容的数据。s</param>
        /// <returns>新创建的 <see cref="Content"/> 实例对象。</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="name"/> 为 null
        ///     或
        ///     <paramref name="mimeType"/> 为 null
        ///     或
        ///     <paramref name="data"/> 为 null。
        /// </exception>
        public static Content Create(string name, string mimeType, byte[] data)
        {
            Contract.NotNull(name, nameof(name));
            Contract.NotNull(mimeType, nameof(mimeType));
            Contract.NotNull(data, nameof(data));

            return new Content
            {
                Id = ObjectId.GenerateNewId(),
                Name = name,
                MimeType = mimeType,
                CreationTime = DateTime.UtcNow,
                Size = data.Length,
                Data = data
            };
        }
    }
}
