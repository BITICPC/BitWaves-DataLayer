using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace BitWaves.Data
{
    /// <summary>
    /// 为测试数据包提供结构定义和访问控制。
    /// </summary>
    public sealed class TestDataArchive : IDisposable
    {
        private readonly ZipArchive _file;
        private readonly TestDataArchiveMetadata _meta;
        private bool _disposed;

        /// <summary>
        /// 初始化 <see cref="TestDataArchive"/> 类的新实例。
        /// </summary>
        /// <param name="file">包含测试数据集的压缩文件。</param>
        /// <param name="metadata">提供测试数据集元数据。</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="file"/> 为 null
        ///     或
        ///     <paramref name="metadata"/> 为 null。
        /// </exception>
        private TestDataArchive(ZipArchive file, TestDataArchiveMetadata metadata)
        {
            Contract.NotNull(file, nameof(file));
            Contract.NotNull(metadata, nameof(metadata));

            _file = file;
            _meta = metadata;

            _disposed = false;
        }

        /// <summary>
        /// 获取测试数据包中包含的所有测试用例数据。
        /// </summary>
        /// <exception cref="ObjectDisposedException">当前对象已经被 dispose。</exception>
        public IEnumerable<TestCaseEntry> TestCases
        {
            get
            {
                EnsureNotDisposed();
                return _meta.TestCases;
            }
        }

        /// <summary>
        /// 若测试数据包中包含答案检查器源文件，获取该文件所对应的 <see cref="TestDataArchiveEntry"/> 对象；否则返回 null。
        /// </summary>
        /// <exception cref="ObjectDisposedException">当前对象已经被 dispose。</exception>
        public TestDataArchiveEntry CheckerSource
        {
            get
            {
                EnsureNotDisposed();
                return _meta.Checker;
            }
        }

        /// <summary>
        /// 若测试数据包中包含交互器源文件，获取该文件所对应的 <see cref="TestDataArchiveEntry"/> 对象；否则返回 null。
        /// </summary>
        /// <exception cref="ObjectDisposedException">当前对象已经被 dispose。</exception>
        public TestDataArchiveEntry InteractorSource
        {
            get
            {
                EnsureNotDisposed();
                return _meta.Interactor;
            }
        }

        /// <summary>
        /// 检查当前对象是否被 dispose。如果当前对象已经被 dispose，抛出 <see cref="ObjectDisposedException"/> 异常。
        /// </summary>
        /// <exception cref="ObjectDisposedException">当前对象已经被 dispose。</exception>
        private void EnsureNotDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().Name);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (!_disposed)
            {
                _file?.Dispose();
                _disposed = true;
            }
        }

        /// <summary>
        /// 从给定的数据流中加载测试数据包结构。
        /// </summary>
        /// <param name="stream">包含测试数据包数据的数据流。</param>
        /// <returns>封装测试数据包中数据描述的 <see cref="TestDataArchive"/> 对象。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/> 为 null。</exception>
        /// <exception cref="BadTestDataArchiveException">给定的数据流中不包含有效的测试数据包结构。</exception>
        public static TestDataArchive FromStream(Stream stream)
        {
            Contract.NotNull(stream, nameof(stream));

            ZipArchive archive;
            try
            {
                archive = new ZipArchive(stream);
            }
            catch (InvalidDataException ex)
            {
                throw new BadTestDataArchiveException("无效的测试数据集文件结构。", ex);
            }

            var meta = TestDataArchiveMetadata.FromZipArchive(archive);
            return new TestDataArchive(archive, meta);
        }

        /// <summary>
        /// 从给定的文件中加载测试数据包结构。
        /// </summary>
        /// <param name="fileName">包含测试数据包的文件名。</param>
        /// <returns>封装测试数据包中数据描述的 <see cref="TestDataArchive"/> 对象。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="fileName"/> 为 null。</exception>
        /// <exception cref="BadTestDataArchiveException">给定的数据流中不包含有效的测试数据包结构。</exception>
        public static TestDataArchive FromFile(string fileName)
        {
            Contract.NotNull(fileName, nameof(fileName));

            return FromStream(File.OpenRead(fileName));
        }
    }
}
