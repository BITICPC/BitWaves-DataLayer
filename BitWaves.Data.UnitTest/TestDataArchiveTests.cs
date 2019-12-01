using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using NUnit.Framework;

namespace BitWaves.Data.UnitTest
{
    /// <summary>
    /// 为 <see cref="TestDataArchive"/> 及其相关组件提供单元测试。
    /// </summary>
    public sealed class TestDataArchiveTests
    {
        private string _goodArchiveFileName;

        [OneTimeSetUp]
        public void Setup()
        {
            SetupGoodArchiveHierarchy();
        }

        private void SetupGoodArchiveHierarchy()
        {
            // Create a test file hierarchy in a temp directory with the following facade:
            // /
            // |- checker.py
            // |- test_1.in
            // |- test_1.ans
            // |- dir
            // |  |- test_2.in
            // |--|- test_2.ans
            var tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDir);
            File.WriteAllText(Path.Combine(tempDir, "checker.py"), "print(\"hello world\")");
            File.WriteAllText(Path.Combine(tempDir, "test_1.in"), "1 2\n");
            File.WriteAllText(Path.Combine(tempDir, "test_1.ans"), "3\n");
            Directory.CreateDirectory(Path.Combine(tempDir, "dir"));
            File.WriteAllText(Path.Combine(tempDir, "dir", "test_2.in"), "3 4\n");
            File.WriteAllText(Path.Combine(tempDir, "dir", "test_2.ans"), "7\n");

            _goodArchiveFileName = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".zip");
            ZipFile.CreateFromDirectory(tempDir, _goodArchiveFileName);
            Directory.Delete(tempDir, true);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            File.Delete(_goodArchiveFileName);
        }

        [Test]
        public void TestFromFileNull()
        {
            Assert.Throws<ArgumentNullException>(() => TestDataArchive.FromFile(null));
        }

        [Test]
        public void TestFromStreamNull()
        {
            Assert.Throws<ArgumentNullException>(() => TestDataArchive.FromStream(null));
        }

        [Test]
        public void TestFromArchiveNull()
        {
            Assert.Throws<ArgumentNullException>(() => TestDataArchive.FromZipArchive(null));
        }

        [Test]
        public void TestFromGoodArchive()
        {
            var archive = TestDataArchive.FromFile(_goodArchiveFileName);
            Assert.IsNotNull(archive.CheckerSource);
            Assert.IsNull(archive.InteractorSource);

            var checkerEntry = archive.CheckerSource;
            Assert.AreEqual("checker.py", checkerEntry.Name);

            var testCases = archive.TestCases.ToList();
            Assert.AreEqual(2, testCases.Count);

            var sep = Path.DirectorySeparatorChar;
            var testCasePairs = new Dictionary<string, string>
            {
                { "test_1.in", "test_1.ans" },
                { $"dir{sep}test_2.in", $"dir{sep}test_2.ans" }
            };

            foreach (var entry in testCases)
            {
                Assert.IsTrue(testCasePairs.ContainsKey(entry.InputFileName));
                Assert.AreEqual(testCasePairs[entry.InputFileName], entry.OutputFileName);
                testCasePairs.Remove(entry.InputFileName);
            }
        }
    }
}
