using System.Collections.Generic;
using System.Linq;
using Aoc2019;
using NUnit.Framework;
using Shouldly;
using Unity.Coding.Editor;
using Unity.Coding.Utils;

namespace AllDays
{
    class LinqpadTests : AocFixture
    {
        [Test, Parallelizable(ParallelScope.Children)]
        public void TestDay([Range(1, 25)] int day)
        {
            var testDir = AocDir.Combine($"day{day}");
            if (!testDir.DirectoryExists())
                Assert.Ignore($"No solution for this day yet ({testDir})");

            var script = testDir.Combine($"day{day}.solver.linq");
            if (!script.FileExists())
                Assert.Ignore("This day solved with something other than linqpad");

            var expected = ExtractResults(day);

            var (stdout, stderr) = (new List<string>(), new List<string>());
            ProcessUtility.ExecuteCommandLine("lprun", new[] { "-recompile", script.FileName }, testDir, stdout, stderr);
            if (stderr.Any())
                Assert.Fail(stderr.StringJoin("\n"));

            var results = stdout.Select(s => s.Trim()).Where(s => s.Any()).ToArray();
            if (results.Any() && results[0].Contains("Exception"))
                Assert.Fail(results.StringJoin("\n"));
            else
                results.ShouldBe(expected);
        }
    }
}
