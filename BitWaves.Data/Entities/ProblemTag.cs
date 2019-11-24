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
