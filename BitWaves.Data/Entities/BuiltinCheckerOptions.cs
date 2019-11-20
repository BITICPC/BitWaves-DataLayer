using System;

namespace BitWaves.Data.Entities
{
    /// <summary>
    /// 为 Standard 评测模式的内置答案检查器提供选项。
    /// </summary>
    [Flags]
    public enum BuiltinCheckerOptions : uint
    {
        /// <summary>
        /// 执行浮点数语义检查。
        /// </summary>
        FloatingPointAware = 1,

        /// <summary>
        /// 比较词素时忽略大小写。
        /// </summary>
        IgnoreCase = 2
    }
}
