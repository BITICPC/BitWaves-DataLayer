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

        /// <summary>
        /// 获取一个空的 <see cref="Maybe{T}"/> 实例。
        /// </summary>
        public static Maybe<T> Empty => new Maybe<T>();

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
