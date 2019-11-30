using System;

namespace BitWaves.Data.Entities
{
    /// <summary>
    /// 为题目标签提供信息。
    /// </summary>
    public sealed class ProblemTag
    {
        /// <summary>
        /// 初始化 <see cref="ProblemTag"/> 类的新实例。
        /// </summary>
        /// <param name="name">标签名称。</param>
        /// <param name="problems">携带该标签的题目数量。</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> 为 null。</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="problems"/> 为 null。</exception>
        public ProblemTag(string name, int problems)
        {
            Contract.NotNull(name, nameof(name));
            Contract.NonNegative(problems, "Number of problems cannot be negative.", nameof(problems));

            Name = name;
            Problems = problems;
        }

        /// <summary>
        /// 获取标签的名称。
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 获取包含此标签的题目的数量。
        /// </summary>
        public int Problems { get; }
    }
}
