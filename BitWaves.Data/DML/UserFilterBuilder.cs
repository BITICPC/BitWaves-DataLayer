using System;
using BitWaves.Data.Entities;
using MongoDB.Driver;

namespace BitWaves.Data.DML
{
    /// <summary>
    /// 为用户实体对象提供筛选器建造器。
    /// </summary>
    public sealed class UserFilterBuilder : FilterBuilder<User>
    {
        /// <summary>
        /// 筛选用户昵称。
        /// </summary>
        /// <param name="nickname">要筛选的用户昵称。</param>
        /// <exception cref="ArgumentNullException"><paramref name="nickname"/> 为 null。</exception>
        public UserFilterBuilder Nickname(string nickname)
        {
            Contract.NotNull(nickname, nameof(nickname));

            AddFilter(Builders<User>.Filter.Eq(u => u.Nickname, nickname));
            return this;
        }

        /// <summary>
        /// 筛选用户手机号。
        /// </summary>
        /// <param name="phone">要筛选的手机号。</param>
        /// <exception cref="ArgumentNullException"><paramref name="phone"/> 为 null。</exception>
        public UserFilterBuilder Phone(string phone)
        {
            Contract.NotNull(phone, nameof(phone));

            AddFilter(Builders<User>.Filter.Eq(u => u.Phone, phone));
            return this;
        }

        /// <summary>
        /// 筛选用户的电子邮箱地址。
        /// </summary>
        /// <param name="email">要筛选的电子邮箱地址。</param>
        /// <exception cref="ArgumentNullException"><paramref name="email"/> 为 null。s</exception>
        public UserFilterBuilder Email(string email)
        {
            Contract.NotNull(email, nameof(email));

            AddFilter(Builders<User>.Filter.Eq(u => u.Email, email));
            return this;
        }

        /// <summary>
        /// 筛选用户的学校。
        /// </summary>
        /// <param name="school">要筛选的学校。</param>
        /// <exception cref="ArgumentNullException"><paramref name="school"/> 为 null。</exception>
        public UserFilterBuilder School(string school)
        {
            Contract.NotNull(school, nameof(school));

            AddFilter(Builders<User>.Filter.Eq(u => u.School, school));
            return this;
        }

        /// <summary>
        /// 筛选用户的学号。
        /// </summary>
        /// <param name="studentId">要筛选的学号。</param>
        /// <exception cref="ArgumentNullException"><paramref name="studentId"/> 为 null。</exception>
        public UserFilterBuilder StudentId(string studentId)
        {
            Contract.NotNull(studentId, nameof(studentId));

            AddFilter(Builders<User>.Filter.Eq(u => u.StudentId, studentId));
            return this;
        }

        /// <summary>
        /// 获取一个空的 <see cref="UserFilterBuilder"/> 对象。
        /// </summary>
        public new static UserFilterBuilder Empty => new UserFilterBuilder();
    }
}
