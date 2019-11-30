using System;
using System.Collections.Generic;
using BitWaves.Data.Entities;
using BitWaves.Data.Utils;
using MongoDB.Driver;

namespace BitWaves.Data.DML
{
    /// <summary>
    /// 为全站公告实体对象提供更新定义。
    /// </summary>
    public sealed class AnnouncementUpdateInfo : UpdateInfo<Announcement>
    {
        /// <summary>
        /// 获取或设置公告是否置顶。
        /// </summary>
        [Set]
        public Maybe<bool> IsPinned { get; set; }

        /// <summary>
        /// 获取或设置公告的标题。
        /// </summary>
        [Set]
        public Maybe<string> Title { get; set; }

        /// <summary>
        /// 获取或设置公告的内容。
        /// </summary>
        [Set]
        public Maybe<string> Content { get; set; }

        /// <inheritdoc />
        protected override IEnumerable<UpdateDefinition<Announcement>> CreatePostUpdateDefinitions()
        {
            return new[]
            {
                Builders<Announcement>.Update.Set(a => a.LastUpdateTime, DateTime.UtcNow)
            };
        }
    }
}
