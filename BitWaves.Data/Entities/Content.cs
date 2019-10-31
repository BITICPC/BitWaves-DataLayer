using System;
using MongoDB.Bson;

namespace BitWaves.Data
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
        /// 获取静态内容的 MIME 类型。
        /// </summary>
        public string MimeType { get; set; }

        /// <summary>
        /// 获取静态内容的创建时间。
        /// </summary>
        public DateTime CreationTime { get; private set; }

        /// <summary>
        /// 获取静态内容的原始数据。
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// 创建一个新的 <see cref="Content"/> 实例对象。
        /// </summary>
        /// <returns>新创建的 <see cref="Content"/> 实例对象。</returns>
        public static Content Create()
        {
            return new Content
            {
                Id = ObjectId.GenerateNewId(),
                CreationTime = DateTime.UtcNow
            };
        }
    }
}
