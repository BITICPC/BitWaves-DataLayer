using System.IO;
using System.IO.Compression;

namespace BitWaves.Data
{
    /// <summary>
    /// 表示测试数据包中的一个 entry。每个 entry 封装一个独立数据段，可以彼此独立地读取数据。
    /// </summary>
    public sealed class TestDataArchiveEntry
    {
        /// <summary>
        /// 初始化 <see cref="TestDataArchiveEntry"/> 类的新实例。
        /// </summary>
        /// <param name="entry">底层 zip 压缩包的 entry 封装。</param>
        internal TestDataArchiveEntry(ZipArchiveEntry entry)
        {
            Contract.NotNull(entry, nameof(entry));

            ZipEntry = entry;
        }

        /// <summary>
        /// 获取底层 zip 压缩包的 entry 封装。
        /// </summary>
        internal ZipArchiveEntry ZipEntry { get; }

        /// <summary>
        /// 获取当前 entry 的名称。
        /// </summary>
        public string Name => ZipEntry.FullName;

        /// <summary>
        /// 获取当前 entry 的原始大小，单位为字节。
        /// </summary>
        public long Size => ZipEntry.Length;

        /// <summary>
        /// 打开一个数据流供读取当前 entry 中的数据。
        /// </summary>
        /// <returns>打开的数据流。</returns>
        public Stream Open()
        {
            return ZipEntry.Open();
        }
    }
}
