using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BitWaves.Data.Entities
{
    /// <summary>
    /// 表示题目标签数据字典实体对象。
    /// </summary>
    public sealed class ProblemTag
    {
        /// <summary>
        /// 初始化 <see cref="ProblemTag"/> 类的新实例。
        /// </summary>
        private ProblemTag()
        {
        }

        /// <summary>
        /// 初始化 <see cref="ProblemTag"/> 类的新实例。
        /// </summary>
        /// <param name="name">题目标签名称。</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="name"/> 为 null。
        /// </exception>
        public ProblemTag(string name)
        {
            Contract.NotNull(name, nameof(name));

            Id = ObjectId.GenerateNewId();
            Name = name;
        }

        /// <summary>
        /// 获取或设置题目标签的全局唯一 ID。
        /// </summary>
        [BsonId]
        public ObjectId Id { get; private set; }

        /// <summary>
        /// 获取或设置题目标签的名称。
        /// </summary>
        public string Name { get; set; }
    }
}
