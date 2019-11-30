using System;
using System.Collections.Generic;
using BitWaves.Data.Entities;
using MongoDB.Driver;

namespace BitWaves.Data.DML
{
    /// <summary>
    /// 为题目查找创建筛选器。
    /// </summary>
    public sealed class ProblemFilterBuilder : FilterBuilder<Problem>
    {
        /// <summary>
        /// 筛选是否在公开题目集中。
        /// </summary>
        /// <param name="inArchive">是否在公开题目集中。</param>
        public ProblemFilterBuilder InArchive(bool inArchive)
        {
            if (inArchive)
            {
                AddFilter(Builders<Problem>.Filter.Exists(p => p.ArchiveId));
            }
            else
            {
                AddFilter(Builders<Problem>.Filter.Not(Builders<Problem>.Filter.Exists(p => p.ArchiveId)));
            }

            return this;
        }

        /// <summary>
        /// 查找指定的题目标题，
        /// </summary>
        /// <param name="title">要查找的题目标题。</param>
        /// <exception cref="ArgumentNullException"><paramref name="title"/> 为 null。s</exception>
        public ProblemFilterBuilder Title(string title)
        {
            Contract.NotNull(title, nameof(title));

            AddFilter(Builders<Problem>.Filter.Eq(p => p.Title, title));
            return this;
        }

        /// <summary>
        /// 查找指定的题目作者。
        /// </summary>
        /// <param name="author">要查找的题目作者。</param>
        /// <exception cref="ArgumentNullException"><paramref name="author"/> 为 null。s</exception>
        public ProblemFilterBuilder Author(string author)
        {
            Contract.NotNull(author, nameof(author));

            AddFilter(Builders<Problem>.Filter.Eq(p => p.Author, author));
            return this;
        }

        /// <summary>
        /// 查找指定的题目来源。
        /// </summary>
        /// <param name="source">要查找的题目来源。该参数可以为 null。</param>
        public ProblemFilterBuilder Source(string source)
        {
            AddFilter(Builders<Problem>.Filter.Eq(p => p.Source, source));
            return this;
        }

        /// <summary>
        /// 查找难度系数不低于指定值的题目。
        /// </summary>
        /// <param name="minValue">最小的难度系数。</param>
        public ProblemFilterBuilder MinDifficulty(int minValue)
        {
            AddFilter(Builders<Problem>.Filter.Gte(p => p.Difficulty, minValue));
            return this;
        }

        /// <summary>
        /// 查找难度系数不高于指定值的题目。
        /// </summary>
        /// <param name="maxValue">最大的难度系数。</param>
        public ProblemFilterBuilder MaxDifficulty(int maxValue)
        {
            AddFilter(Builders<Problem>.Filter.Lte(p => p.Difficulty, maxValue));
            return this;
        }

        /// <summary>
        /// 查找难度系数区间。
        /// </summary>
        /// <param name="minValue">最小的难度系数。</param>
        /// <param name="maxValue">最大的难度系数。</param>
        public ProblemFilterBuilder Difficulty(int minValue, int maxValue)
        {
            return MinDifficulty(minValue)
                .MaxDifficulty(maxValue);
        }

        /// <summary>
        /// 查找包含所有给定的标签的题目。
        /// </summary>
        /// <param name="tags">要查找的题目标签。</param>
        /// <exception cref="ArgumentNullException"><paramref name="tags"/> 为 null。</exception>
        public ProblemFilterBuilder Tags(IEnumerable<string> tags)
        {
            Contract.NotNull(tags, nameof(tags));

            AddFilter(Builders<Problem>.Filter.All(p => p.Tags, tags));
            return this;
        }

        /// <summary>
        /// 获取空的 <see cref="ProblemFilterBuilder"/> 对象。
        /// </summary>
        public new static ProblemFilterBuilder Empty => new ProblemFilterBuilder();
    }
}
