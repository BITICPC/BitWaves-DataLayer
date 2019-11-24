using System;

namespace BitWaves.Data.Entities
{
    /// <summary>
    /// 为题目提供一组样例。
    /// </summary>
    public sealed class ProblemSampleTest
    {
        /// <summary>
        /// 初始化 <see cref="ProblemSampleTest"/> 类的新实例。
        /// </summary>
        private ProblemSampleTest()
        {
        }

        /// <summary>
        /// 初始化 <see cref="ProblemSampleTest"/> 类的新实例。
        /// </summary>
        /// <param name="input">输入数据。</param>
        /// <param name="output">输出数据。</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="input"/> 为 null
        ///     或
        ///     <paramref name="output"/> 为 null。
        /// </exception>
        public ProblemSampleTest(string input, string output)
        {
            Contract.NotNull(input, nameof(input));
            Contract.NotNull(output, nameof(output));

            Input = input;
            Output = output;
        }

        /// <summary>
        /// 获取或设置样例输入数据。
        /// </summary>
        public string Input { get; set; }

        /// <summary>
        /// 获取或设置样例输出数据。
        /// </summary>
        public string Output { get; set; }
    }
}
