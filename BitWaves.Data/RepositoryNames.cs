namespace BitWaves.Data
{
    /// <summary>
    /// 为数据库中各数据集提供名称。
    /// </summary>
    internal static class RepositoryNames
    {
        /// <summary>
        /// 数据仓库名称。
        /// </summary>
        public const string Repository = "BitWaves";

        /// <summary>
        /// 内容数据集的名称。
        /// </summary>
        public const string Contents = "Contents";

        /// <summary>
        /// 用户数据集的名称。
        /// </summary>
        public const string Users = "Users";

        /// <summary>
        /// 全站公告数据集的名称。
        /// </summary>
        public const string Announcements = "Announcements";

        /// <summary>
        /// 题目数据集的名称。
        /// </summary>
        public const string Problems = "Problems";

        /// <summary>
        /// 题目测试数据文件的 GridFS Bucket 名称。
        /// </summary>
        public const string TestDataArchiveBucket = "TestDataArchives";

        /// <summary>
        /// 题目标签数据字典的名称。
        /// </summary>
        public const string ProblemTags = "ProblemTags";

        /// <summary>
        /// 语言数据集的名称。
        /// </summary>
        public const string Languages = "Languages";
    }
}
