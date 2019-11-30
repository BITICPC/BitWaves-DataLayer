using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BitWaves.Data.Utils;
using MongoDB.Driver;

namespace BitWaves.Data.DML
{
    /// <summary>
    /// 为实体对象更新模型提供抽象基类。
    /// </summary>
    /// <typeparam name="TRootEntity">目标实体对象在数据模式中的根实体对象类型。</typeparam>
    public abstract class UpdateInfo<TRootEntity>
    {
        /// <summary>
        /// 从当前的数据更新模型创建 <see cref="UpdateDefinition{TEntity"/> 实例。
        /// </summary>
        /// <param name="parentPath">从更新模型的根到当前更新模型的父节点的路径。</param>
        /// <returns>
        ///     当前的数据更新模型创建的 <see cref="UpdateDefinition{TEntity"/> 实例。若没有任何数据需要更新，返回 null。
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="parentPath"/> 为 null。</exception>
        internal UpdateDefinition<TRootEntity> CreateUpdateDefinition(ObjectPath parentPath)
        {
            Contract.NotNull(parentPath, nameof(parentPath));

            var myType = GetType();
            var myFields = myType.GetRuntimeFields().Cast<MemberInfo>();
            var myProperties = myType.GetRuntimeProperties().Cast<MemberInfo>();

            var updateDefinitions = myFields.Concat(myProperties)
                                            .Select(member => ResolveMemberUpdateDefinition(member, parentPath))
                                            .Where(x => x != null)
                                            .ToList();
            if (updateDefinitions.Count == 0)
            {
                return null;
            }

            updateDefinitions.AddRange(CreatePostUpdateDefinitions());

            return Builders<TRootEntity>.Update.Combine(updateDefinitions);
        }

        /// <summary>
        /// 从当前的数据更新模型创建 <see cref="UpdateDefinition{TEntity"/> 实例。
        /// </summary>
        /// <returns>
        ///     当前的数据更新模型创建的 <see cref="UpdateDefinition{TEntity"/> 实例。若没有任何数据需要更新，返回 null。
        /// </returns>
        internal UpdateDefinition<TRootEntity> CreateUpdateDefinition()
        {
            return CreateUpdateDefinition(ObjectPath.Root);
        }

        /// <summary>
        /// 获取当前数据更新模型上的给定成员上的数据更新定义。
        /// </summary>
        /// <param name="member">当前数据更新模型上的成员。</param>
        /// <param name="parentPath">从更新模型的根到当前更新模型的父节点的路径。</param>
        /// <returns>当前数据更新模型上的给定成员上的数据更新定义。若给定的成员没有任何更新数据，返回 null。</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="member"/> 为 null。
        /// </exception>
        private UpdateDefinition<TRootEntity> ResolveMemberUpdateDefinition(MemberInfo member, ObjectPath parentPath)
        {
            Contract.NotNull(member, nameof(member));

            // 如果成员的值是一个 Maybe<T> 类型，则将其展开为内部值
            var memberValue = GetMemberValue(member);
            if (memberValue != null && MaybeUtils.IsMaybe(memberValue))
            {
                var maybe = MaybeUtils.Unbox(memberValue);
                if (maybe.HasValue)
                {
                    memberValue = maybe.Value;
                }
                else
                {
                    return null;
                }
            }

            return member.GetCustomAttribute<UpdateVerbAttribute>()
                         ?.Resolve<TRootEntity>(member, memberValue, parentPath);
        }

        /// <summary>
        /// 获取给定成员的值。
        /// </summary>
        /// <param name="member">要获取值的成员定义。</param>
        /// <returns>给定成员的值。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="member"/> 为 null。</exception>
        /// <exception cref="ArgumentException"><paramref name="member"/> 既不是一个数据字段也不是一个属性。</exception>
        private object GetMemberValue(MemberInfo member)
        {
            Contract.NotNull(member, nameof(member));

            switch (member)
            {
                case FieldInfo field:
                    return field.GetValue(this);

                case PropertyInfo property:
                    return property.GetValue(this);

                default:
                    throw new ArgumentException($"{nameof(member)} is neither a field nor a property.", nameof(member));
            }
        }

        /// <summary>
        /// 当在子类中重写时，创建一个或多个 <see cref="UpdateDefinition{TEntity}"/> 对象；这些额外的更新定义将被一同加入到最终的
        /// 更新定义中。
        /// </summary>
        /// <returns>额外的数据更新定义。</returns>
        protected virtual IEnumerable<UpdateDefinition<TRootEntity>> CreatePostUpdateDefinitions()
        {
            return Enumerable.Empty<UpdateDefinition<TRootEntity>>();
        }
    }
}
