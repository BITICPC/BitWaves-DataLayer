using System;
using MongoDB.Bson;

namespace BitWaves.Data.Entities
{
    /// <summary>
    /// 表示一个全站公告。
    /// </summary>
    public sealed class Announcement
    {
        /// <summary>
        /// 获取公告的全局唯一 ID。
        /// </summary>
        public ObjectId Id { get; private set; }

        /// <summary>
        /// 获取或设置公告的作者的用户名。
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 获取公告的创建时间。
        /// </summary>
        public DateTime CreationTime { get; private set; }

        /// <summary>
        /// 获取或设置公告的上次修改时间。
        /// </summary>
        public DateTime LastUpdateTime { get; set; }

        /// <summary>
        /// 获取或设置公告是否置顶。
        /// </summary>
        public bool IsPinned { get; set; }

        /// <summary>
        /// 获取或设置公告标题。
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 获取或设置公告内容。
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 创建一个新的 <see cref="Announcement"/> 实例对象。
        /// </summary>
        /// <param name="author">公告作者的用户名。</param>
        /// <param name="title">公告标题。</param>
        /// <param name="content">公告内容。</param>
        /// <returns>新创建的 <see cref="Announcement"/> 实例对象。</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="author"/> 为 null
        ///     或
        ///     <paramref name="title"/> 为 null
        ///     或
        ///     <paramref name="content"/> 为 null。
        /// </exception>
        public static Announcement Create(string author, string title, string content)
        {
            Contract.NotNull(author, nameof(author));
            Contract.NotNull(title, nameof(title));
            Contract.NotNull(content, nameof(content));

            return new Announcement
            {
                Id = ObjectId.GenerateNewId(),
                Author = author,
                CreationTime = DateTime.UtcNow,
                LastUpdateTime = DateTime.UtcNow,
                IsPinned = false,
                Title = title,
                Content = content
            };
        }
    }
}
