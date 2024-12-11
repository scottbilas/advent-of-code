class Day11 : Fixture
{
    long Solve(string input, int blinks)
    {
        var rocks = input.GetInts().Grouped().ToDefaultDictionary(g => (long)g.Key, g => (long)g.Count());

        for (var i = 0; i < blinks; ++i)
        {
            var last = rocks.ToArray();
            rocks.Clear();
            foreach (var (rock, count) in last)
            {
                var digits = rock.ToString();
                if (digits.Length % 2 == 0)
                {
                    var half = digits.Length / 2;
                    rocks[digits[..half].Long()] += count;
                    rocks[digits[half..].Long()] += count;
                }
                else if (rock != 0)
                    rocks[rock * 2024] += count;
                else
                    rocks[1] += count;
            }
        }

        return rocks.Values.Sum();
    }

    [TestCase("0 1 10 99 999", 1, 7)]
    [TestCase("125 17", 1, 3)]
    [TestCase("125 17", 2, 4)]
    [TestCase("125 17", 3, 5)]
    [TestCase("125 17", 4, 9)]
    [TestCase("125 17", 5, 13)]
    [TestCase("125 17", 6, 22)]
    [Test] public void Sample1(string input, int blinks, int result) => Solve(input, blinks).ShouldBe(result);

    [Test] public void Part1() => Solve(Input, 25).ShouldBe(218079);
    [Test] public void Part2() => Solve(Input, 75).ShouldBe(259755538429618);
}
