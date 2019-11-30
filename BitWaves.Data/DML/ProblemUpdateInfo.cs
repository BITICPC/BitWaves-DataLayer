using System;
using System.Collections.Generic;
using BitWaves.Data.Entities;
using BitWaves.Data.Utils;
using MongoDB.Driver;

namespace BitWaves.Data.DML
{
    /// <summary>
    /// 为 <see cref="Problem"/> 实体对象提供数据更新定义。
    /// </summary>
    public sealed class ProblemUpdateInfo : UpdateInfo<Problem>
    {
        /// <summary>
        /// 初始化 <see cref="ProblemUpdateInfo"/> 类的新实例。
        /// </summary>
        public ProblemUpdateInfo()
        {
            Description = new ProblemDescriptionUpdateInfo();
            JudgeInfo = new ProblemJudgeInfoUpdateInfo();
        }

        /// <summary>
        /// 获取或设置更新的标题。
        /// </summary>
        [Set]
        public Maybe<string> Title { get; set; }

        /// <summary>
        /// 获取或设置更新的题目来源。
        /// </summary>
        [Set]
        public Maybe<string> Source { get; set; }

        /// <summary>
        /// 获取或设置更新的题目难度。
        /// </summary>
        [Set]
        public Maybe<int> Difficulty { get; set; }

        /// <summary>
        /// 获取或设置更新的题目标签。
        /// </summary>
        [Set]
        public Maybe<List<string>> Tags { get; set; }

        /// <summary>
        /// 获取题目描述的更新数据。
        /// </summary>
        public ProblemDescriptionUpdateInfo Description { get; }

        /// <summary>
        /// 获取题目评测信息的更新数据。
        /// </summary>
        public ProblemJudgeInfoUpdateInfo JudgeInfo { get; }

        /// <inheritdoc />
        protected override IEnumerable<UpdateDefinition<Problem>> CreatePostUpdateDefinitions()
        {
            return new[]
            {
                Builders<Problem>.Update.Set(p => p.LastUpdateTime, DateTime.UtcNow)
            };
        }
    }
}
