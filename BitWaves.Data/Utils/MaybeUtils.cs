using System;
using System.Diagnostics;

namespace BitWaves.Data.Utils
{
    /// <summary>
    /// 为 <see cref="Maybe{T}"/> 提供一些实用方法。
    /// </summary>
    public static class MaybeUtils
    {
        /// <summary>
        /// 检查给定的类型是否为一个 <see cref="Maybe{T}"/> 类型。
        /// </summary>
        /// <param name="type">要检查的类型。</param>
        /// <returns>给定的类型是否为一个 <see cref="Maybe{T}"/> 类型。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> 为 null。</exception>
        public static bool IsMaybeType(Type type)
        {
            Contract.NotNull(type, nameof(type));

            return type.IsConstructedGenericType &&
                   type.GetGenericTypeDefinition() == typeof(Maybe<>);
        }

        /// <summary>
        /// 检查给定的类型是否为一个 <see cref="Maybe{T}"/> 类型。
        /// </summary>
        /// <param name="type">要检查的类型。</param>
        /// <typeparam name="T"><see cref="Maybe{T}"/> 类型的内部类型。</typeparam>
        /// <returns>给定的类型是否为一个 <see cref="Maybe{T}"/> 类型。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> 为 null。</exception>
        public static bool IsMaybeType<T>(Type type)
        {
            Contract.NotNull(type, nameof(type));

            return type == typeof(Maybe<T>);
        }

        /// <summary>
        /// 获取给定的 <see cref="Maybe{T}"/> 类型包装的内部类型。
        /// </summary>
        /// <param name="maybeType"><see cref="Maybe{T}"/> 类型。</param>
        /// <returns>给定的 <see cref="Maybe{T}"/> 类型包装的内部类型。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="maybeType"/> 为 null。</exception>
        /// <exception cref="ArgumentException">
        ///     <see cref="maybeType"/> 不是一个 <see cref="Maybe{T}"/> 类型。
        /// </exception>
        public static Type GetInnerType(Type maybeType)
        {
            Contract.NotNull(maybeType, nameof(maybeType));

            if (!IsMaybeType(maybeType))
                throw new ArgumentException($"{maybeType} is not a maybe type.", nameof(maybeType));

            return maybeType.GetGenericArguments()[0];
        }

        /// <summary>
        /// 检查给定的对象是否为一个装箱的 <see cref="Maybe{T}"/> 实例。
        /// </summary>
        /// <param name="value">要检查的对象。</param>
        /// <returns>给定的对象是否为一个装箱的 <see cref="Maybe{T}"/> 实例。</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="value"/> 为 null。
        /// </exception>
        public static bool IsMaybe(object value)
        {
            Contract.NotNull(value, nameof(value));

            return IsMaybeType(value.GetType());
        }

        /// <summary>
        /// 检查给定的对象是否为一个装箱的 <see cref="Maybe{T}"/> 实例。
        /// </summary>
        /// <param name="value">要检查的对象。</param>
        /// <typeparam name="T"><see cref="Maybe{T}"/> 类型的内部类型。</typeparam>
        /// <returns>给定的对象是否为一个装箱的 <see cref="Maybe{T}"/> 实例。</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="value"/> 为 null。
        /// </exception>
        public static bool IsMaybe<T>(object value)
        {
            Contract.NotNull(value, nameof(value));

            return IsMaybeType<T>(value.GetType());
        }

        /// <summary>
        /// 使用反射的手段获得给定的装箱后的 <see cref="Maybe{T}"/> 实例的内部值。
        /// </summary>
        /// <param name="maybe">装箱后的 <see cref="Maybe{T}"/> 实例。</param>
        /// <returns>包含给定的装箱后的 <see cref="Maybe{T}"/> 实例的内部值的 <see cref="Maybe{T}"/> 实例。</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="maybe"/> 为 null。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="maybe"/> 不是一个 <see cref="Maybe{T}"/> 类型的实例。
        /// </exception>
        public static Maybe<object> Unbox(object maybe)
        {
            Contract.NotNull(maybe, nameof(maybe));
            if (!IsMaybe(maybe))
                throw new ArgumentException("Not valid Maybe<T>", nameof(maybe));

            var hasValueProperty = maybe.GetType().GetProperty(nameof(Maybe<object>.HasValue));
            var valueProperty = maybe.GetType().GetProperty(nameof(Maybe<object>.Value));

            Debug.Assert(hasValueProperty != null, nameof(hasValueProperty) + " != null");
            Debug.Assert(valueProperty != null, nameof(valueProperty) + " != null");

            var hasValue = (bool) hasValueProperty.GetValue(maybe);
            if (!hasValue)
            {
                return Maybe<object>.Empty;
            }

            var value = valueProperty.GetValue(maybe);
            return new Maybe<object>(value);
        }

        /// <summary>
        /// 设置给定的 <see cref="Maybe{T}"/> 类型对象的内部值为给定的值。
        /// </summary>
        /// <param name="maybe">要设置内部值的 <see cref="Maybe{T}"/> 对象。</param>
        /// <param name="value">要设置的内部值。</param>
        /// <exception cref="ArgumentNullException"><paramref name="maybe"/> 为 null。</exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="maybe"/> 不是一个 <see cref="Maybe{T}"/> 类型的实例对象。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     <paramref name="value"/> 的类型与给定的 <see cref="Maybe{T}"/> 类型的内部值类型不匹配。
        /// </exception>
        public static void SetInnerValue(object maybe, object value)
        {
            Contract.NotNull(maybe, nameof(maybe));

            if (!IsMaybe(maybe))
                throw new ArgumentException(nameof(maybe) + " is not a maybe type.", nameof(maybe));

            var maybeType = maybe.GetType();
            if (!GetInnerType(maybeType).IsInstanceOfType(value))
                throw new InvalidOperationException(
                    $"Cannot assign {nameof(value)} as inner value of {nameof(maybe)}.");

            var valueProperty = maybeType.GetProperty(nameof(Maybe<object>.Value));
            Debug.Assert(valueProperty != null, nameof(valueProperty) + " != null");

            valueProperty.SetValue(maybe, value);
        }

        public static object CreateEmpty(Type innerType)
        {
            Contract.NotNull(innerType, nameof(innerType));

            var maybeType = typeof(Maybe<>).MakeGenericType(innerType);
            return Activator.CreateInstance(maybeType);
        }

        public static object Create(Type innerType, object value)
        {
            Contract.NotNull(innerType, nameof(innerType));

            var maybe = CreateEmpty(innerType);
            SetInnerValue(maybe, value);

            return maybe;
        }

        public static object Create(object value)
        {
            Contract.NotNull(value, nameof(value));

            return Create(value.GetType(), value);
        }
    }
}
