using System.Collections.Generic;
using System.Threading.Tasks;
using BitWaves.Data.Entities;
using BitWaves.Data.Extensions;
using MongoDB.Driver;

namespace BitWaves.Data.DML
{
    /// <summary>
    /// 为查找全站公告提供查询管道。
    /// </summary>
    public sealed class AnnouncementFindPipeline : FindPipeline<Announcement>
    {
        /// <summary>
        /// 初始化 <see cref="AnnouncementFindPipeline"/> 类的新实例。
        /// </summary>
        public AnnouncementFindPipeline() : base(FilterBuilder<Announcement>.Empty)
        {
        }

        /// <inheritdoc />
        protected override IFindFluent<Announcement, Announcement> Sort(IFindFluent<Announcement, Announcement> find)
        {
            var sorts = new[]
            {
                Builders<Announcement>.Sort.Descending(a => a.IsPinned),
                Builders<Announcement>.Sort.Descending(a => a.CreationTime)
            };

            return find.Sort(Builders<Announcement>.Sort.Combine(sorts));
        }

        /// <inheritdoc />
        protected override async Task<List<Announcement>> CollectAsync(IFindFluent<Announcement, Announcement> find)
        {
            return await find.Project(Builders<Announcement>.Projection.Exclude(a => a.Content))
                             .ToEntityListAsync();
        }

        /// <summary>
        /// 获取 <see cref="AnnouncementFindPipeline"/> 的默认实例。
        /// </summary>
        public static AnnouncementFindPipeline Default => new AnnouncementFindPipeline();
    }
}
