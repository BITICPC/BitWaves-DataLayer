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
        /// 获取语言的全局唯一 ID。
        /// </summary>
        [BsonId]
        public ObjectId Id { get; private set; }

        /// <summary>
        /// 获取或设置语言的标识符。
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// 获取或设置语言的方言标识符。
        /// </summary>
        public string Dialect { get; set; }

        /// <summary>
        /// 获取或设置语言的版本。
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 获取或设置语言的用户友好名称。
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 创建一个新的 <see cref="Language"/> 实例对象。
        /// </summary>
        /// <param name="identifier">语言标识符。</param>
        /// <param name="dialect">方言标识符。</param>
        /// <param name="version">版本标识符。</param>
        /// <param name="displayName">语言的显示名称。</param>
        /// <returns>新创建的 <see cref="Language"/> 实例对象。</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="identifier"/> 为 null
        ///     或
        ///     <paramref name="dialect"/> 为 null
        ///     或
        ///     <paramref name="version"/> 为 null
        ///     或
        ///     <paramref name="displayName"/> 为 null
        /// </exception>
        public static Language Create(string identifier, string dialect, string version, string displayName)
        {
            Contract.NotNull(identifier, nameof(identifier));
            Contract.NotNull(dialect, nameof(dialect));
            Contract.NotNull(version, nameof(version));
            Contract.NotNull(displayName, nameof(displayName));

            return new Language
            {
                Id = ObjectId.GenerateNewId(),
                Identifier = identifier,
                Dialect = dialect,
                Version = version,
                DisplayName = displayName
            };
        }
    }
}
