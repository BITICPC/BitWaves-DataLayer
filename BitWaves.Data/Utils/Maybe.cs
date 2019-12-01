namespace BitWaves.Data.Utils
{
    /// <summary>
    /// 表示一个可能为空的值的容器。
    /// </summary>
    /// <typeparam name="T">容器包含的值的类型。</typeparam>
    public struct Maybe<T>
    {
        private T _value;
        private bool _hasValue;

        /// <summary>
        /// 初始化 <see cref="Maybe{T}"/> 的新实例。新实例将包含给定的值。
        /// </summary>
        /// <param name="value">被包含的值。</param>
        public Maybe(T value)
        {
            _value = value;
            _hasValue = true;
        }

        /// <summary>
        /// 获取当前 <see cref="Maybe{T}"/> 实例中包含的值。
        /// </summary>
        /// <exception cref="EmptyMaybeException">当前的 <see cref="Maybe{T}"/> 实例不包含任何值。</exception>
        public T Value
        {
            get
            {
                if (!_hasValue)
                    throw new EmptyMaybeException();
                return _value;
            }
            set
            {
                _value = value;
                _hasValue = true;
            }
        }

        /// <summary>
        /// 测试当前的 <see cref="Maybe{T}"/> 实例是否包含任何的值。
        /// </summary>
        public bool HasValue => _hasValue;

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HasValue ? Value.GetHashCode() : typeof(T).GetHashCode();
        }

        /// <summary>
        /// 检查当前的 <see cref="Maybe{T}"/> 实例是否与给定的 <see cref="Maybe{T}"/> 实例相同。
        /// </summary>
        /// <param name="another">另一个 <see cref="Maybe{T}"/> 实例。</param>
        /// <returns>当前的 <see cref="Maybe{T}"/> 实例是否与给定的 <see cref="Maybe{T}"/> 实例相同。</returns>
        public bool Equals(Maybe<T> another)
        {
            if (HasValue != another.HasValue)
            {
                return false;
            }

            if (HasValue)
            {
                return Equals(Value, another.Value);
            }

            return true;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (!(obj is Maybe<T> maybe))
            {
                return false;
            }

            return Equals(maybe);
        }

        /// <summary>
        /// 获取一个空的 <see cref="Maybe{T}"/> 实例。
        /// </summary>
        public static Maybe<T> Empty => new Maybe<T>();

        /// <summary>
        /// 检查给定的两个 <see cref="Maybe{T}"/> 值是否相同。
        /// </summary>
        /// <param name="lhs">左操作数。</param>
        /// <param name="rhs">右操作数。</param>
        /// <returns>给定的两个 <see cref="Maybe{T}"/> 值是否相同。</returns>
        public static bool operator ==(Maybe<T> lhs, Maybe<T> rhs)
        {
            return lhs.Equals(rhs);
        }

        /// <summary>
        /// 检查给定的两个 <see cref="Maybe{T}"/> 值是否不相同。
        /// </summary>
        /// <param name="lhs">左操作数。</param>
        /// <param name="rhs">右操作数。</param>
        /// <returns>给定的两个 <see cref="Maybe{T}"/> 值是否不相同。</returns>
        public static bool operator !=(Maybe<T> lhs, Maybe<T> rhs)
        {
            return !lhs.Equals(rhs);
        }

        /// <summary>
        /// 将给定的值转换为 <see cref="Maybe{T}"/> 包装。
        /// </summary>
        /// <param name="value">要包装的值。</param>
        /// <returns>给定的值的 <see cref="Maybe{T}"/> 包装。</returns>
        public static implicit operator Maybe<T>(T value)
        {
            return new Maybe<T>(value);
        }
    }
}
