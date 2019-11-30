using System;
using BitWaves.Data.Entities;
using MongoDB.Driver;

namespace BitWaves.Data.DML
{
    /// <summary>
    /// 为用户查询提供查询管道。
    /// </summary>
    public sealed class UserFindPipeline : FindPipeline<User>
    {
        /// <summary>
        /// 初始化 <see cref="UserFindPipeline"/> 类的新实例。
        /// </summary>
        /// <param name="filterBuilder">用户实体对象的筛选器建造器。</param>
        /// <exception cref="ArgumentNullException"><paramref name="filterBuilder"/> 为 null。</exception>
        public UserFindPipeline(UserFilterBuilder filterBuilder) : base(filterBuilder)
        {
        }

        /// <summary>
        /// 获取或设置排序键。
        /// </summary>
        public UserSortKey SortKey { get; set; }

        /// <summary>
        /// 获取或设置是否按照给定排序键降序进行排序。
        /// </summary>
        public bool SortByDescending { get; set; }

        /// <inheritdoc />
        protected override IFindFluent<User, User> Sort(IFindFluent<User, User> find)
        {
            if (SortByDescending)
            {
                find = find.Sort(Builders<User>.Sort.Descending(SortKey.GetField()));
            }
            else
            {
                find = find.Sort(Builders<User>.Sort.Ascending(SortKey.GetField()));
            }

            return find;
        }

        /// <summary>
        /// 获取默认的 <see cref="UserFindPipeline"/> 对象。
        /// </summary>
        public static UserFindPipeline Default => new UserFindPipeline(UserFilterBuilder.Empty);
    }
}
