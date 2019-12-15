using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;
using Shouldly;
using Unity.Coding.Editor;
using Unity.Coding.Utils;

namespace AllDays
{
    class LinqpadTests
    {
        [Test, Parallelizable(ParallelScope.Children)]
        public void TestDay([Range(1, 25)] int day)
        {
            var testDir = TestContext.CurrentContext.TestDirectory.ToNPath()
                .Combine($"../../../../aoc/day{day}");
            if (!testDir.DirectoryExists())
                Assert.Ignore("No solution for this day yet");

            var script = testDir.Combine($"day{day}.solver.linq");
            if (!script.FileExists())
                Assert.Ignore("This day solved with something other than linqpad");

            var expected = Regex
                .Matches(
                    testDir.Combine($"day{day}.results.txt").ReadAllText(),
                    @"Your puzzle answer was (.*)\.")
                .Select(m => m.Groups[1].Value)
                .ToArray();

            var (stdout, stderr) = (new List<string>(), new List<string>());
            ProcessUtility.ExecuteCommandLine("lprun", new[] { script.FileName }, testDir, stdout, stderr);
            if (stderr.Any())
                Assert.Fail(stderr.StringJoin("\n"));

            var results = stdout.Select(s => s.Trim()).Where(s => s.Any()).ToArray();
            results.ShouldBe(expected);
        }
    }
}
