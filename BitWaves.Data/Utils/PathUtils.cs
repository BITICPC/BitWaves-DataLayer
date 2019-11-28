using System;

namespace BitWaves.Data.Utils
{
    /// <summary>
    /// 提供路径相关的功能方法。
    /// </summary>
    internal static class PathUtils
    {
        /// <summary>
        /// 返回不包含扩展名的路径。
        /// </summary>
        /// <param name="path">要处理的路径。</param>
        /// <returns>不包含扩展名的路径。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> 为 null。</exception>
        public static string RemoveExtension(string path)
        {
            Contract.NotNull(path, nameof(path));

            var extensionStart = path.LastIndexOf('.');
            if (extensionStart == -1)
            {
                return path;
            }

            return path.Substring(0, extensionStart);
        }
    }
}
