using System;

namespace BitWaves.Data.Utils
{
    /// <summary>
    /// 表示一个不可变的对象图路径。
    /// </summary>
    internal sealed class ObjectPath
    {
        /// <summary>
        /// 初始化一个空的 <see cref="ObjectPath"/> 对象。
        /// </summary>
        public ObjectPath()
        {
            Path = string.Empty;
        }

        /// <summary>
        /// 初始化新的 <see cref="ObjectPath"/> 对象。
        /// </summary>
        /// <param name="path">已有的对象图路径。</param>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> 为 null。</exception>
        public ObjectPath(string path)
        {
            Contract.NotNull(path, nameof(path));

            Path = path;
        }

        /// <summary>
        /// 获取路径的字符串表示。
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// 向路径末尾添加一个新的路径部分。返回一个新的 <see cref="ObjectPath"/> 对象，当前对象不会被修改。
        /// </summary>
        /// <param name="component">要添加的路径部分。</param>
        /// <returns>新的 <see cref="ObjectPath"/> 对象。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="component"/> 为 null。</exception>
        public ObjectPath Push(string component)
        {
            Contract.NotNull(component, nameof(component));

            var newPath = Path.Length == 0
                ? component
                : string.Concat(Path, ".", component);
            return new ObjectPath(newPath);
        }

        /// <summary>
        /// 获取路径的字符串表示。
        /// </summary>
        public override string ToString()
        {
            return Path;
        }

        /// <summary>
        /// 获取表示对象图根位置处的 <see cref="ObjectPath"/> 对象。
        /// </summary>
        public static ObjectPath Root => new ObjectPath();
    }
}
