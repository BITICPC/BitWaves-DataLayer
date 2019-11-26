using System.Collections.Generic;

namespace BitWaves.Data.Entities
{
    /// <summary>
    /// 表示用户提交的评测结果。
    /// </summary>
    public sealed class JudgeResult
    {
        /// <summary>
        /// 获取或设置总评测结果。
        /// </summary>
        public Verdict Verdict { get; set; }

        /// <summary>
        /// 获取或设置用户程序在所有的测试用例中消耗的 CPU 时间的最大值，单位为毫秒。
        /// </summary>
        public int Time { get; set; }

        /// <summary>
        /// 获取或设置用户程序在所有的测试用例中占用的内存大小的最大值，单位为 MB。
        /// </summary>
        public int Memory { get; set; }

        /// <summary>
        /// 获取或设置每个测试用例上的详细的评测结果。
        /// </summary>
        public List<TestCaseResult> TestCaseResults { get; set; }
    }
}
