using System;
using System.Collections.Generic;
using System.IO.Compression;

namespace BitWaves.Data
{
    /// <summary>
    /// 封装测试数据集的元数据。
    /// </summary>
    internal sealed class TestDataArchiveMetadata
    {
        /// <summary>
        /// 初始化 <see cref="TestDataArchiveMetadata"/> 类的新实例。
        /// </summary>
        internal TestDataArchiveMetadata()
        {
            TestCases = new List<TestCaseEntry>();
            Checker = null;
            Interactor = null;
        }

        /// <summary>
        /// 获取测试数据集中的所有测试用例。
        /// </summary>
        public List<TestCaseEntry> TestCases { get; }

        /// <summary>
        /// 获取包含答案检查器源代码的文件的 <see cref="TestDataArchiveEntry"/> 封装。
        /// </summary>
        public TestDataArchiveEntry Checker { get; internal set; }

        /// <summary>
        /// 获取包含交互器源代码的文件的 <see cref="TestDataArchiveEntry"/> 封装。
        /// </summary>
        public TestDataArchiveEntry Interactor { get; internal set; }

        /// <summary>
        /// 从给定的 ZIP 文件构造 <see cref="TestDataArchiveMetadata"/> 类的新实例。
        /// </summary>
        /// <param name="archive">包含测试数据集的 ZIP 文件。</param>
        /// <returns>测试数据集元数据的 <see cref="TestDataArchiveMetadata"/> 封装。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="archive"/> 为 null。</exception>
        /// <exception cref="BadTestDataArchiveException">给定的 ZIP 文件中不包含有效的测试数据集结构。</exception>
        public static TestDataArchiveMetadata FromZipArchive(ZipArchive archive)
        {
            Contract.NotNull(archive, nameof(archive));

            var builder = new TestDataArchiveMetadataBuilder();
            foreach (var zipEntry in archive.Entries)
            {
                try
                {
                    builder.AddZipArchiveEntry(zipEntry);
                }
                catch (ArgumentException ex)
                {
                    if (ex is ArgumentNullException)
                    {
                        throw;
                    }

                    throw new BadTestDataArchiveException("无效的测试数据集。", ex);
                }
            }

            try
            {
                return builder.GetMetadata();
            }
            catch (InvalidOperationException e)
            {
                throw new BadTestDataArchiveException("无效的测试数据集。", e);
            }
        }
    }
}
