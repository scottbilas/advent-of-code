using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using NUnit.Framework;
using Unity.Coding.Utils;

namespace Aoc2019
{
    [Parallelizable(ParallelScope.All)]
    public class AocFixture
    {
        NPath ThisDir([CallerFilePath] string path = null) => path.ToNPath();

        protected string DayName => GetType().Namespace.Split('.').Last();
        protected NPath AocDir => ThisDir().Combine("../../aoc");
        protected NPath ScriptDir => AocDir.Combine(DayName.ToLower());

        public string[] ExtractResults(int day = 0)
        {
            var dayName = day == 0 ? DayName : $"day{day}";

            return Regex
                .Matches(
                    AocDir.Combine(dayName, $"{dayName}.results.txt").ReadAllText(),
                    @"Your puzzle answer was (.*)\.")
                .Select(m => m.Groups[1].Value)
                .ToArray();
        }
    }
}
