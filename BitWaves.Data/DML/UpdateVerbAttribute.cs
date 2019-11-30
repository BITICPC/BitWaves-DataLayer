using System;
using System.Reflection;
using BitWaves.Data.Utils;
using MongoDB.Driver;

namespace BitWaves.Data.DML
{
    /// <summary>
    /// 为更新动词标注提供公共抽象基类。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    internal abstract class UpdateVerbAttribute : Attribute
    {
        /// <summary>
        /// 初始化 <see cref="UpdateVerbAttribute"/> 类的新实例。
        /// </summary>
        protected UpdateVerbAttribute()
        {
            Name = null;
        }

        /// <summary>
        /// 获取或设置要更新的实体对象属性的名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 将给定的被标注成员转换到 <see cref="UpdateDefinition{T}"/> 上。
        /// </summary>
        /// <param name="member">数据更新模型上的成员。该成员上应该带有当前的标注对象。</param>
        /// <param name="memberValue">数据成员的值。</param>
        /// <param name="parentPath">从更新模型根到当前更新模型的父节点的路径，以点隔开。</param>
        /// <typeparam name="TRootEntity">目标实体对象在数据模式中的根实体对象类型。</typeparam>
        /// <returns>
        ///     当前字段更新的 <see cref="UpdateDefinition{T}"/> 定义。若当前字段不包含有效的数据更新定义，返回 null。
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="member"/> 为 null
        ///     或
        ///     <paramref name="parentPath"/> 为 null。
        /// </exception>
        public UpdateDefinition<TRootEntity> Resolve<TRootEntity>(
            MemberInfo member, object memberValue, ObjectPath parentPath)
        {
            Contract.NotNull(member, nameof(member));
            Contract.NotNull(parentPath, nameof(parentPath));

            // 检查 memberValue 的类型是否为 EntityUpdateInfo<TRootEntity>。如果是，则递归生成 UpdateDefinition<TRootEntity>
            if (EntityUpdateInfoUtils.IsEntityUpdateInfo<TRootEntity>(memberValue))
            {
                var subUpdate = (UpdateInfo<TRootEntity>) memberValue;
                var currentPath = parentPath.Push(GetFieldName(member));
                return subUpdate.CreateUpdateDefinition(currentPath);
            }

            var field = GetEntityField<TRootEntity>(member, parentPath);
            return CreateVerb(field, memberValue);
        }

        /// <summary>
        /// 获取数据更新模型成员的名称。
        /// </summary>
        /// <param name="member">数据更新模型的成员。</param>
        /// <returns>数据更新模型成员的名称。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="member"/> 为 null。</exception>
        private string GetFieldName(MemberInfo member)
        {
            Contract.NotNull(member, nameof(member));

            return Name ?? member.Name;
        }

        /// <summary>
        /// 获取目标实体对象上与当前标注的属性或数据成员相对应的属性或数据成员定义。
        /// </summary>
        /// <param name="member">当前标注的属性或数据成员。</param>
        /// <param name="parentPath">从更新模型根到当前更新节点的父节点的路径。</param>
        /// <typeparam name="TRootEntity">目标实体对象在数据模式中的根实体对象类型。</typeparam>
        /// <returns>目标实体对象上与当前标注的属性或数据成员相对应的属性或数据成员定义。</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="member"/> 为 null
        ///     或
        ///     <paramref name="parentPath"/> 为 null。
        /// </exception>
        private FieldDefinition<TRootEntity, object> GetEntityField<TRootEntity>(MemberInfo member,
                                                                                 ObjectPath parentPath)
        {
            Contract.NotNull(member, nameof(member));
            Contract.NotNull(parentPath, nameof(parentPath));

            var fieldPath = parentPath.Push(GetFieldName(member));
            return new StringFieldDefinition<TRootEntity, object>(fieldPath.ToString());
        }

        /// <summary>
        /// 当在子类中实现时，依据给定的目标实体对象数据成员以及给定的目标更新值创建 <see cref="UpdateDefinition{TEntity}"/> 的新实
        /// 例。
        /// </summary>
        /// <param name="field">目标实体对象数据成员。</param>
        /// <param name="value">更新值。</param>
        /// <typeparam name="TRootEntity">目标实体对象在数据模式中的根实体对象类型。</typeparam>
        /// <returns>创建的 <see cref="UpdateDefinition{TEntity"/> 实例。</returns>
        protected abstract UpdateDefinition<TRootEntity> CreateVerb<TRootEntity>(
            FieldDefinition<TRootEntity, object> field, object value);
    }
}
