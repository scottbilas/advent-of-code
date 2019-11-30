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
                .Combine($"../../../../aoc/day{day:00}");
            if (!testDir.DirectoryExists())
                Assert.Ignore("No solution for this day yet");

            var scripts = testDir.Files("day*.linq").ToList();
            if (!scripts.Any())
                Assert.Ignore("This day solved with something other than linqpad");

            foreach (var script in scripts)
            {
                var expected = Regex
                    .Matches(
                        script.ChangeExtension(".txt").ReadAllText(),
                        @"Your puzzle answer was (.*)\.")
                    .Select(m => m.Groups[1].Value)
                    .ToArray();

                var (stdout, stderr) = (new List<string>(), new List<string>());
                ProcessUtility.ExecuteCommandLine("lprun", new[] { "-optimize", script.FileName }, testDir, stdout, stderr);
                if (stderr.Any())
                    Assert.Fail(stderr.StringJoin("\n"));

                var results = stdout.Select(s => s.Trim()).Where(s => s.Any()).ToArray();
                results.ShouldBe(expected);
            }
        }
    }
}
