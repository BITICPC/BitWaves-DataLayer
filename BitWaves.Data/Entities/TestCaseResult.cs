namespace BitWaves.Data.Entities
{
    /// <summary>
    /// 表示用户提交在一个单独的测试用例上的评测结果。
    /// </summary>
    public sealed class TestCaseResult
    {
        /// <summary>
        /// 获取或设置评测结果。
        /// </summary>
        public Verdict Verdict { get; set; }

        /// <summary>
        /// 获取或设置消耗的 CPU 时间，单位为毫秒。
        /// </summary>
        public int Time { get; set; }

        /// <summary>
        /// 获取或设置占用的内存大小，单位为 MB。
        /// </summary>
        public int Memory { get; set; }

        /// <summary>
        /// 获取或设置提交程序的退出代码。
        /// </summary>
        public int ExitCode { get; set; }

        /// <summary>
        /// 获取或设置输入数据的视图。
        /// </summary>
        public string InputView { get; set; }

        /// <summary>
        /// 获取或设置答案数据的视图。
        /// </summary>
        public string AnswerView { get; set; }

        /// <summary>
        /// 获取或设置输出数据的视图。
        /// </summary>
        public string OutputView { get; set; }

        /// <summary>
        /// 获取评测系统产生的注释。
        /// </summary>
        public string Comment { get; set; }
    }
}
