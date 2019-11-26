using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BitWaves.Data.Entities
{
    /// <summary>
    /// 表示一个编程语言及其环境。
    /// </summary>
    public sealed class Language
    {
        /// <summary>
        /// 初始化 <see cref="Language"/> 类的新实例。
        /// </summary>
        private Language()
        {
        }

        /// <summary>
        /// 初始化 <see cref="Language"/> 类的新实例。
        /// </summary>
        /// <param name="identifier">语言标识符。</param>
        /// <param name="dialect">语言的方言。</param>
        /// <param name="version">语言的版本。</param>
        /// <param name="displayName">语言的显示名称。</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="identifier"/> 为 null
        ///     或
        ///     <paramref name="dialect"/> 为 null
        ///     或
        ///     <paramref name="version"/> 为 null
        ///     或
        ///     <paramref name="displayName"/> 为 null。
        /// </exception>
        public Language(string identifier, string dialect, string version, string displayName)
        {
            Contract.NotNull(identifier, nameof(identifier));
            Contract.NotNull(dialect, nameof(dialect));
            Contract.NotNull(version, nameof(version));
            Contract.NotNull(displayName, nameof(displayName));

            Id = ObjectId.GenerateNewId();
            Triple = new LanguageTriple(identifier, dialect, version);
            DisplayName = displayName;
        }

        /// <summary>
        /// 获取语言的全局唯一 ID。
        /// </summary>
        [BsonId]
        public ObjectId Id { get; private set; }

        /// <summary>
        /// 获取或设置语言三元组。
        /// </summary>
        public LanguageTriple Triple { get; set; }

        /// <summary>
        /// 获取或设置语言的用户友好名称。
        /// </summary>
        public string DisplayName { get; set; }
    }
}
