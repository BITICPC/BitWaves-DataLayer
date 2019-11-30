using System.Collections.Generic;
using BitWaves.Data.Entities;
using BitWaves.Data.Utils;

namespace BitWaves.Data.DML
{
    /// <summary>
    /// 为 <see cref="ProblemDescription"/> 实体对象提供数据更新模型。
    /// </summary>
    public sealed class ProblemDescriptionUpdateInfo : UpdateInfo<Problem>
    {
        /// <summary>
        /// 获取或设置更新的题目叙述。
        /// </summary>
        [Set]
        public Maybe<string> Legend { get; set; }

        /// <summary>
        /// 获取或设置更新的题目输入描述。
        /// </summary>
        [Set]
        public Maybe<string> Input { get; set; }

        /// <summary>
        /// 获取或设置更新的题目输出描述。
        /// </summary>
        [Set]
        public Maybe<string> Output { get; set; }

        /// <summary>
        /// 获取或设置更新的题目样例。
        /// </summary>
        [Set]
        public Maybe<List<ProblemSampleTest>> SampleTests { get; set; }

        /// <summary>
        /// 获取或设置更新的题目提示。
        /// </summary>
        [Set]
        public Maybe<string> Notes { get; set; }
    }
}
