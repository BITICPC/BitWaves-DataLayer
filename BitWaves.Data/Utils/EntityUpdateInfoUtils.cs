using System;
using System.Diagnostics;
using System.Reflection;
using BitWaves.Data.DML;
using MongoDB.Driver;

namespace BitWaves.Data.Utils
{
    /// <summary>
    /// 为 <see cref="UpdateInfo{TRootEntity}"/> 提供实用方法。s
    /// </summary>
    internal static class EntityUpdateInfoUtils
    {
        /// <summary>
        /// 检查给定的类型是否为 <see cref="UpdateInfo{TRootEntity}"/> 类型。
        /// </summary>
        /// <param name="type">要检查的类型。</param>
        /// <returns>给定的类型是否为 <see cref="UpdateInfo{TRootEntity}"/> 类型。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> 为 null。</exception>
        public static bool IsEntityUpdateInfoType(Type type)
        {
            Contract.NotNull(type, nameof(type));

            return type.IsConstructedGenericType &&
                   type.GetGenericTypeDefinition() == typeof(UpdateInfo<>);
        }

        /// <summary>
        /// 检查给定的类型是否为 <see cref="UpdateInfo{TRootEntity}"/> 类型。
        /// </summary>
        /// <param name="type">要检查的类型。</param>
        /// <typeparam name="TRootEntity">根实体对象类型。</typeparam>
        /// <returns>给定的类型是否为 <see cref="UpdateInfo{TRootEntity}"/> 类型。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> 为 null。</exception>
        public static bool IsEntityUpdateInfoType<TRootEntity>(Type type)
        {
            Contract.NotNull(type, nameof(type));

            return type == typeof(UpdateInfo<TRootEntity>);
        }

        /// <summary>
        /// 检查给定的对象是否为 <see cref="UpdateInfo{TRootEntity}"/> 的实例对象。
        /// </summary>
        /// <param name="value">要检查的对象。</param>
        /// <returns>给定的对象是否为 <see cref="UpdateInfo{TRootEntity}"/> 的实例对象。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> 为 null。</exception>
        public static bool IsEntityUpdateInfo(object value)
        {
            Contract.NotNull(value, nameof(value));

            var valueType = value.GetType();
            // Walk up the inheritance chain and check if EntityUpdateInfo<TRootEntity, ???> is included.
            do
            {
                if (IsEntityUpdateInfoType(valueType))
                {
                    return true;
                }

                valueType = valueType.BaseType;
            } while (valueType != null);

            return false;
        }

        /// <summary>
        /// 检查给定的值是否为 <see cref="UpdateInfo{TRootEntity}"/> 的实例对象。
        /// </summary>
        /// <param name="value">要检查的值。</param>
        /// <typeparam name="TRootEntity">根实体对象类型。</typeparam>
        /// <returns>给定的值是否为 <see cref="UpdateInfo{TRootEntity}"/> 的实例对象。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> 为 null。</exception>
        public static bool IsEntityUpdateInfo<TRootEntity>(object value)
        {
            Contract.NotNull(value, nameof(value));

            return value.GetType().IsSubclassOf(typeof(UpdateInfo<TRootEntity>));
        }

        /// <summary>
        /// 使用运行时反射调用给定的 <see cref="UpdateInfo{TRootEntity}"/> 对象上的
        /// <see cref="UpdateInfo{TRootEntity}.CreateUpdateDefinition(ObjectPath)"/> 方法并返回其返回值。
        /// </summary>
        /// <param name="updateDefinition"><see cref="UpdateInfo{TRootEntity}"/> 对象。</param>
        /// <param name="parentPath">从实体对象模式根到当前更新节点的父节点的对象路径。</param>
        /// <typeparam name="TRootEntity">实体对象模式根对象类型。s</typeparam>
        /// <returns>创建的 <see cref="UpdateDefinition{TRootEntity}"/> 对象。</returns>
        public static UpdateDefinition<TRootEntity> CreateUpdateDefinition<TRootEntity>(object updateDefinition,
                                                                                        ObjectPath parentPath)
        {
            Contract.NotNull(updateDefinition, nameof(updateDefinition));
            Contract.NotNull(parentPath, nameof(parentPath));

            if (!IsEntityUpdateInfo<TRootEntity>(updateDefinition))
                throw new ArgumentException(
                    "The given update definition is not an instance of EntityUpdateInfo<TRootEntity, ???>.",
                    nameof(updateDefinition));

            var updateMethod = updateDefinition
                               .GetType()
                               .GetRuntimeMethod(nameof(UpdateInfo<object>.CreateUpdateDefinition),
                                                 new[] { typeof(ObjectPath) });
            Debug.Assert(updateMethod != null, nameof(updateMethod) + " != null");

            return (UpdateDefinition<TRootEntity>) updateMethod.Invoke(updateDefinition, new object[] { parentPath });
        }
    }
}
