using BitWaves.Data.Entities;

namespace BitWaves.Data.DML
{
    /// <summary>
    /// 为 <see cref="Announcement"/> 提供筛选器建造器。
    /// </summary>
    public sealed class AnnouncementFilterBuilder : FilterBuilder<Announcement>
    {
        /// <summary>
        /// 获取空的 <see cref="AnnouncementFilterBuilder"/> 实例对象。
        /// </summary>
        public new static AnnouncementFilterBuilder Empty => new AnnouncementFilterBuilder();
    }
}
