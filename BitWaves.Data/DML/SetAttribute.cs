using MongoDB.Driver;

namespace BitWaves.Data.DML
{
    /// <summary>
    /// 使用 Set 更新动词更新相应的字段。
    /// </summary>
    internal sealed class SetAttribute : UpdateVerbAttribute
    {
        /// <inheritdoc />
        protected override UpdateDefinition<TEntity> CreateVerb<TEntity>(
            FieldDefinition<TEntity, object> field, object value)
        {
            return Builders<TEntity>.Update.Set(field, value);
        }
    }
}
