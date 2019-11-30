using System;
using System.Collections.Generic;
using BitWaves.Data.Entities;
using BitWaves.Data.Utils;
using MongoDB.Driver;

namespace BitWaves.Data.DML
{
    /// <summary>
    /// 为提交实体对象提供更新定义。
    /// </summary>
    public sealed class SubmissionUpdateInfo : UpdateInfo<Submission>
    {
        /// <summary>
        /// 获取或设置更新的评测状态。
        /// </summary>
        [Set]
        public Maybe<JudgeStatus> Status { get; set; }

        /// <summary>
        /// 获取或设置更新的评测结果。
        /// </summary>
        [Set]
        public Maybe<JudgeResult> Result { get; set; }

        /// <inheritdoc />
        protected override IEnumerable<UpdateDefinition<Submission>> CreatePostUpdateDefinitions()
        {
            var updates = new List<UpdateDefinition<Submission>>();
            if (Status.HasValue && Status.Value == JudgeStatus.Judging)
            {
                updates.Add(Builders<Submission>.Update.Set(s => s.JudgeTime, DateTime.UtcNow));
            }

            return updates;
        }
    }
}
