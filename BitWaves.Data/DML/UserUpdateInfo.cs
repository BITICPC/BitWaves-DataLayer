using BitWaves.Data.Entities;
using BitWaves.Data.Utils;

namespace BitWaves.Data.DML
{
    /// <summary>
    /// 为用户信息提供字段更新数据。
    /// </summary>
    public sealed class UserUpdateInfo : UpdateInfo<User>
    {
        /// <summary>
        /// 获取或设置对昵称的更新数据。
        /// </summary>
        [Set]
        public Maybe<string> Nickname { get; set; }

        /// <summary>
        /// 获取或设置对手机号的更新数据。
        /// </summary>
        [Set]
        public Maybe<string> Phone { get; set; }

        /// <summary>
        /// 获取或设置对电子邮件的更新数据。
        /// </summary>
        [Set]
        public Maybe<string> Email { get; set; }

        /// <summary>
        /// 获取或设置对学校的更新数据。
        /// </summary>
        [Set]
        public Maybe<string> School { get; set; }

        /// <summary>
        /// 获取或设置对学号的更新数据。
        /// </summary>
        [Set]
        public Maybe<string> StudentId { get; set; }

        /// <summary>
        /// 获取或设置对博客 URL 的更新数据。
        /// </summary>
        [Set]
        public Maybe<string> BlogUrl { get; set; }

        /// <summary>
        /// 获取或设置对管理员标记的更新数据。
        /// </summary>
        [Set]
        public Maybe<bool> IsAdmin { get; set; }
    }
}
