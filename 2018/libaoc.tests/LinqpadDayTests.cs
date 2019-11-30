using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using NiceIO;
using NUnit.Framework;
using Shouldly;

namespace AllDays
{
    class LinqpadTests
    {
        [Test, Parallelizable(ParallelScope.Children)]
        public void TestDay([Range(1, 25)] int day)
        {
            if (day == 12)
                Assert.Ignore("Didn't leave this in working state last year :(");

            var testDir = TestContext.CurrentContext.TestDirectory.ToNPath()
                .Combine($"../../../../aoc/day{day:00}")
                .DirectoryMustExist();

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

                string[] results;
                using (var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "lprun",
                    Arguments = $"-optimize {script}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                }))
                {
                    results = process.StandardOutput.ReadToEnd()
                        .Split('\n').Select(s => s.Trim()).Where(s => s.Any())
                        .ToArray();

                    process.StandardError.ReadToEnd().ShouldBe("");

                    process.WaitForExit();
                }

                results.ShouldBe(expected);
            }
        }
    }
}
