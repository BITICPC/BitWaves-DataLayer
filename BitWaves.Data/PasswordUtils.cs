using System;
using System.Security.Cryptography;
using System.Text;

namespace BitWaves.Data
{
    /// <summary>
    /// 提供密码哈希相关的方法。
    /// </summary>
    public static class PasswordUtils
    {
        private static readonly Encoding Encoding;
        private static readonly HashAlgorithm Hasher;

        static PasswordUtils()
        {
            Encoding = Encoding.UTF8;
            Hasher = SHA256.Create();
        }

        /// <summary>
        /// 获取给定密码的哈希值。
        /// </summary>
        /// <param name="password">要计算哈希的密码明文。</param>
        /// <returns>给定密码的哈希值。</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="password"/> 为 null。
        /// </exception>
        public static byte[] GetPasswordHash(string password)
        {
            Contract.NotNull(password, nameof(password));

            var encoded = Encoding.GetBytes(password);
            using (var hasher = SHA256.Create())
            {
                return hasher.ComputeHash(encoded);
            }
        }

        /// <summary>
        /// 检查给定的密码明文经过哈希后是否与给定的哈希一致。
        /// </summary>
        /// <param name="passwordHash">密码哈希。</param>
        /// <param name="password">要检查的密码明文。</param>
        /// <returns>检查给定的密码明文经过哈希后是否与给定的哈希一致。</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="passwordHash"/> 为 null
        ///     或
        ///     <paramref name="password"/> 为 null。
        /// </exception>
        public static bool Challenge(byte[] passwordHash, string password)
        {
            Contract.NotNull(passwordHash, nameof(passwordHash));
            Contract.NotNull(password, nameof(password));

            return BufferUtils.Equals(passwordHash, GetPasswordHash(password));
        }
    }
}
