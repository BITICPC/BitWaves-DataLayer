using System;

namespace BitWaves.Data.DML
{
    /// <summary>
    /// 提供分页参数。
    /// </summary>
    public sealed class Pagination
    {
        /// <summary>
        /// 初始化 <see cref="Pagination"/> 类的新实例。
        /// </summary>
        /// <param name="page">页面编号。页面编号从 0 开始。</param>
        /// <param name="itemsPerPage">每一页上的元素数量。</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="page"/> 小于零
        ///     或
        ///     <paramref name="itemsPerPage"/> 小于等于零。
        /// </exception>
        public Pagination(int page, int itemsPerPage)
        {
            Contract.NonNegative(page, "Page number cannot be negative.", nameof(page));
            Contract.Positive(itemsPerPage, "Number of items per page should be positive.", nameof(itemsPerPage));

            Page = page;
            ItemsPerPage = itemsPerPage;
        }

        /// <summary>
        /// 获取页面编号。
        /// </summary>
        public int Page { get; }

        /// <summary>
        /// 获取每一页上的元素数量。
        /// </summary>
        public int ItemsPerPage { get; }

        /// <summary>
        /// 获取为到达指定页面的第一个元素需要跳过的元素数量。返回结果为 <see cref="Int32"/> 类型。
        /// </summary>
        /// <exception cref="OverflowException">计算过程中发生整数溢出。</exception>
        public int Skip => checked(Page * ItemsPerPage);

        /// <summary>
        /// 获取为到达指定页面的第一个元素需要跳过的元素数量。返回结果为 <see cref="Int64"/> 类型。
        /// </summary>
        public long LongSkip => unchecked((long) Page * ItemsPerPage);

        /// <summary>
        /// 获取在一页中显示所有元素的 <see cref="Pagination"/> 对象。
        /// </summary>
        public static Pagination AllElements { get; } = new Pagination(0, int.MaxValue);
    }
}
