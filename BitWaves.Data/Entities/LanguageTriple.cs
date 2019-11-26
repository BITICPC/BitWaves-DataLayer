using System;

namespace BitWaves.Data.Entities
{
    /// <summary>
    /// 表示语言三元组。
    /// </summary>
    public sealed class LanguageTriple
    {
        /// <summary>
        /// 初始化 <see cref="LanguageTriple"/> 类的新实例。
        /// </summary>
        private LanguageTriple()
        {
        }

        /// <summary>
        /// 初始化 <see cref="LanguageTriple"/> 类的新实例。
        /// </summary>
        /// <param name="identifier">语言标识符。</param>
        /// <param name="dialect">语言方言。</param>
        /// <param name="version">语言版本。</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="identifier"/> 为 null
        ///     或
        ///     <paramref name="dialect"/> 为 null
        ///     或
        ///     <paramref name="version"/> 为 null。
        /// </exception>
        public LanguageTriple(string identifier, string dialect, string version)
        {
            Contract.NotNull(identifier, nameof(identifier));
            Contract.NotNull(dialect, nameof(dialect));
            Contract.NotNull(version, nameof(version));

            Identifier = identifier;
            Dialect = dialect;
            Version = version;
        }

        /// <summary>
        /// 获取语言标识符。
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// 获取语言方言。
        /// </summary>
        public string Dialect { get; set; }

        /// <summary>
        /// 获取语言版本。
        /// </summary>
        public string Version { get; set; }
    }
}
