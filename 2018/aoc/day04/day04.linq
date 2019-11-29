<Query Kind="Program">
  <NuGetReference>morelinq</NuGetReference>
  <NuGetReference>Shouldly</NuGetReference>
  <Namespace>MoreLinq.Extensions</Namespace>
  <Namespace>Shouldly</Namespace>
</Query>

void Main()
{
    // sample

    new[] {
            "[1518-11-05 00:45] falls asleep",
            "[1518-11-01 00:00] Guard #10 begins shift",
            "[1518-11-03 00:29] wakes up",
            "[1518-11-01 00:30] falls asleep",
            "[1518-11-01 23:58] Guard #99 begins shift",
            "[1518-11-02 00:50] wakes up",
            "[1518-11-05 00:03] Guard #99 begins shift",
            "[1518-11-02 00:40] falls asleep",
            "[1518-11-05 00:55] wakes up",
            "[1518-11-04 00:46] wakes up",
            "[1518-11-01 00:05] falls asleep",
            "[1518-11-01 00:25] wakes up",
            "[1518-11-04 00:36] falls asleep",
            "[1518-11-03 00:24] falls asleep",
            "[1518-11-03 00:05] Guard #10 begins shift",
            "[1518-11-04 00:02] Guard #99 begins shift",
            "[1518-11-01 00:55] wakes up",
        }
        .ParseAndExtractSleepLogs()
        .FindSleepiestGuard()
        .ShouldBe((240, 4455));

    // problem

    var scriptDir = Path.GetDirectoryName(Util.CurrentQueryPath);

    var problemSleepLogs = File
        .ReadLines($"{scriptDir}/input.txt")
        .ParseAndExtractSleepLogs()
        .ToList();

    problemSleepLogs.RenderSleep($"{scriptDir}/timeline.txt");

    var sleepiest = problemSleepLogs.FindSleepiestGuard();
    sleepiest.strategy1.Dump().ShouldBe(8421);
    sleepiest.strategy2.Dump().ShouldBe(83359);
}

struct SleepLog
{
    public DateTime Date;
    public int GuardId;
    public int StartMinute;
    public int MinuteCount;
}

static class Extensions
{
    public static IEnumerable<SleepLog> ParseAndExtractSleepLogs(this IEnumerable<string> textLogs)
    {
        var lastGuardId = 0;
        return textLogs
            .OrderBy(_ => _)
            .Select(textLog =>
            {
                // [1518-11-05 00:03] Guard #99 begins shift
                //    ^0 ^1 ^2 ^3 ^4         ^5
                
                var ints = Regex
                    .Matches(textLog, @"\d+").Cast<Match>()
                    .Select(m => int.Parse(m.Value))
                    .ToList();

                var id = 0;
                if (ints.Count() == 6)
                    lastGuardId = ints[5];
                else
                    id = lastGuardId;

                return (id, ints);
            })
            .Where(log => log.id != 0) // skip "begins shift", was only needed to track id's for sleep/wake pairs
            .Batch(2, batch =>
            {
                var (start, stop) = (batch.First(), batch.Last());
                return new SleepLog
                {
                    Date = new DateTime(start.ints[0], start.ints[1], start.ints[2]),
                    GuardId = start.id,
                    StartMinute = start.ints[4],
                    MinuteCount = stop.ints[4] - start.ints[4]
                };
            });
    }

    public static (int strategy1, int strategy2) FindSleepiestGuard(this IEnumerable<SleepLog> sleepLogs)
    {
        var counts =
            from sleepLog in sleepLogs
            group sleepLog by sleepLog.GuardId into guard
            let minutes =
                from guardLog in guard
                from index in Enumerable.Range(guardLog.StartMinute, guardLog.MinuteCount)
                group index by index
            select (
                id: guard.Key,
                total: minutes.Sum(minute => minute.Count()),
                maxMinute: minutes.MaxBy(minute => minute.Count()).First());

        var maxByTotalSleep = counts.MaxBy(guard => guard.total).First();
        var maxByTotalMinute = counts.MaxBy(guard => guard.maxMinute.Count()).First();

        return (
            strategy1: maxByTotalSleep.id * maxByTotalSleep.maxMinute.Key,
            strategy2: maxByTotalMinute.id * maxByTotalMinute.maxMinute.Key);
    }

    public static void RenderSleep(this IEnumerable<SleepLog> sleepLogs, string outputPath)
    {
        using (var f = File.CreateText(outputPath))
        {
            f.WriteLine("Date   ID     Minute");
            f.WriteLine("              000000000011111111112222222222333333333344444444445555555555");
            f.WriteLine("              012345678901234567890123456789012345678901234567890123456789");
            
            foreach (var day in sleepLogs.GroupBy(l => l.Date))
            {
                f.Write($"{day.Key.Month:00}-{day.Key.Day:00}  #{day.First().GuardId.ToString().PadLeft(4)}  ");
                var minutes = Enumerable.Repeat('.', 60).ToArray();
                foreach (var i in day.SelectMany(sleepLog => Enumerable.Range(sleepLog.StartMinute, sleepLog.MinuteCount)))
                    minutes[i] = '#';
                f.WriteLine(minutes);
            }
            f.WriteLine($"");
        }
    }
}