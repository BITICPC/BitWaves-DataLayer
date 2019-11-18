using System.IO;

namespace BitWaves.Data
{
    /// <summary>
    /// 表示测试数据集中的一个测试用例。
    /// </summary>
    public sealed class TestCaseEntry
    {
        /// <summary>
        /// 初始化 <see cref="TestCaseEntry"/> 类的新实例。
        /// </summary>
        /// <param name="inputFileEntry">输入数据文件的 <see cref="TestDataArchiveEntry"/> 封装。</param>
        /// <param name="outputFileEntry">输出数据文件的 <see cref="TestDataArchiveEntry"/> 封装。</param>
        internal TestCaseEntry(TestDataArchiveEntry inputFileEntry,
                               TestDataArchiveEntry outputFileEntry)
        {
            Contract.NotNull(inputFileEntry, nameof(inputFileEntry));
            Contract.NotNull(outputFileEntry, nameof(outputFileEntry));

            InputFileEntry = inputFileEntry;
            OutputFileEntry = outputFileEntry;
        }

        /// <summary>
        /// 获取当前测试用例的输入文件的 <see cref="TestDataArchiveEntry"/> 封装。
        /// </summary>
        private TestDataArchiveEntry InputFileEntry { get; }

        /// <summary>
        /// 获取当前测试用例的输出文件的 <see cref="TestDataArchiveEntry"/> 封装。
        /// </summary>
        private TestDataArchiveEntry OutputFileEntry { get; }

        /// <summary>
        /// 获取输入文件名称。
        /// </summary>
        public string InputFileName => InputFileEntry.Name;

        /// <summary>
        /// 获取输出文件名称。
        /// </summary>
        public string OutputFileName => OutputFileEntry.Name;

        /// <summary>
        /// 打开输入文件的数据流对象供读取。
        /// </summary>
        /// <returns>输入文件的数据流对象。</returns>
        public Stream OpenInputFile()
        {
            return InputFileEntry.Open();
        }

        /// <summary>
        /// 打开输出文件的数据流对象供读取。
        /// </summary>
        /// <returns>输出文件的数据流对象。</returns>
        public Stream OpenOutputFile()
        {
            return OutputFileEntry.Open();
        }
    }
}
