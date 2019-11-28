using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BitWaves.Data.Entities
{
    /// <summary>
    /// 表示一个用户。
    /// </summary>
    public sealed class User
    {
        /// <summary>
        /// 初始化 <see cref="User"/> 类的新实例。
        /// </summary>
        private User()
        {
        }

        /// <summary>
        /// 初始化 <see cref="User"/> 类的新实例。
        /// </summary>
        /// <param name="username">用户名。</param>
        /// <param name="passwordHash">用户密码哈希值。</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="username"/> 为 null
        ///     或
        ///     <paramref name="passwordHash"/> 为 null。
        /// </exception>
        public User(string username, byte[] passwordHash)
        {
            Contract.NotNull(username, nameof(username));
            Contract.NotNull(passwordHash, nameof(passwordHash));

            Id = ObjectId.GenerateNewId();
            Username = username;
            PasswordHash = passwordHash;
            JoinTime = DateTime.UtcNow;
        }

        /// <summary>
        /// 获取用户 ID。
        /// </summary>
        [BsonId]
        public ObjectId Id { get; private set; }

        /// <summary>
        /// 获取或设置用户名。
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 获取或设置用户密码的 SHA256 哈希值。
        /// </summary>
        public byte[] PasswordHash { get; set; }

        /// <summary>
        /// 获取或设置用户的昵称。
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// 获取或设置用户手机号。
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 获取或设置用户电子邮箱。
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 获取或设置用户学校。
        /// </summary>
        public string School { get; set; }

        /// <summary>
        /// 获取或设置用户学号。
        /// </summary>
        public string StudentId { get; set; }

        /// <summary>
        /// 获取或设置用户的个人博客的 URL。
        /// </summary>
        public string BlogUrl { get; set; }

        /// <summary>
        /// 获取用户的注册时间。
        /// </summary>
        public DateTime JoinTime { get; private set; }

        /// <summary>
        /// 获取或设置用户的总提交数。
        /// </summary>
        public int TotalSubmissions { get; set; }

        /// <summary>
        /// 获取或设置用户的总 AC 提交数。
        /// </summary>
        public int TotalAcceptedSubmissions { get; set; }

        /// <summary>
        /// 获取或设置用户曾经提交过的题目总数。
        /// </summary>
        public int TotalProblemsAttempted { get; set; }

        /// <summary>
        /// 获取或设置用户通过的题目总数。
        /// </summary>
        public int TotalProblemsAccepted { get; set; }

        /// <summary>
        /// 获取或设置用户是否为管理员。
        /// </summary>
        public bool IsAdmin { get; set; }
    }
}
