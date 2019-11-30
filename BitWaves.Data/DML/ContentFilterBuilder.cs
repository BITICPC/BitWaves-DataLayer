using System;
using BitWaves.Data.Entities;
using MongoDB.Driver;

namespace BitWaves.Data.DML
{
    /// <summary>
    /// 为静态对象实体对象提供筛选器建造器。
    /// </summary>
    public sealed class ContentFilterBuilder : FilterBuilder<Content>
    {
        /// <summary>
        /// 筛选给定的静态对象名称。
        /// </summary>
        /// <param name="name">要筛选的静态对象的名称。</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> 为 null。</exception>
        public ContentFilterBuilder Name(string name)
        {
            Contract.NotNull(name, nameof(name));

            AddFilter(Builders<Content>.Filter.Eq(c => c.Name, name));
            return this;
        }

        /// <summary>
        /// 筛选给定的静态对象的 MIME 类型。
        /// </summary>
        /// <param name="mimeType">要筛选的静态对象的 MIME 类型。</param>
        /// <exception cref="ArgumentNullException"><paramref name="mimeType"/> 为 null。s</exception>
        public ContentFilterBuilder MimeType(string mimeType)
        {
            Contract.NotNull(mimeType, nameof(mimeType));

            AddFilter(Builders<Content>.Filter.Eq(c => c.MimeType, mimeType));
            return this;
        }

        /// <summary>
        /// 获取一个空的 <see cref="ContentFilterBuilder"/> 对象。
        /// </summary>
        public new static ContentFilterBuilder Empty => new ContentFilterBuilder();
    }
}
