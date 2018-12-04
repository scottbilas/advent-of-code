<Query Kind="Statements">
  <NuGetReference>morelinq</NuGetReference>
  <NuGetReference>Shouldly</NuGetReference>
  <Namespace>MoreLinq.Extensions</Namespace>
  <Namespace>Shouldly</Namespace>
</Query>

// sample

FindSleepiestGuard(ParseAndExtractSleepLogs(new[] {
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
    }))
    .ShouldBe((240, 4455));

// problem

var scriptDir = Path.GetDirectoryName(Util.CurrentQueryPath);

var problemSleepLogs = ParseAndExtractSleepLogs(File.ReadLines($"{scriptDir}/input.txt"));

FindSleepiestGuard(problemSleepLogs)
    .Dump()
    .ShouldBe((8421, 83359));

IEnumerable<(int id, int start, int stop)> ParseAndExtractSleepLogs(IEnumerable<string> textLogs)
{
    var lastGuardId = 0;
    return textLogs
        .OrderBy(_ => _)
        .Select(textLog =>
        {
            var groups = Regex.Match(textLog, @"(\d+)].*(?:#(\d+) begins|(asleep)|wakes)").Groups;

            var id = 0;
            if (groups[2].Success)
                lastGuardId = int.Parse(groups[2].Value);
            else
                id = lastGuardId;

            return (id, min: int.Parse(groups[1].Value), asleep: groups[3].Success);
        })
        .Where(log => log.id != 0) // skip "begins shift"
        .Batch(2, batch => (id: batch.First().id, start: batch.First().min, stop: batch.Last().min));
}

(int strategy1, int strategy2) FindSleepiestGuard(IEnumerable<(int id, int start, int stop)> sleepLogs)
{
    // run start->stop and count all minutes
    var counts = sleepLogs
        .GroupBy(guard => guard.id)
        .Select(guard =>
            {
                // count all minutes
                var minutes = new int[60];
                guard
                    .SelectMany(pair => Enumerable.Range(pair.start, pair.stop - pair.start))
                    .ForEach(i => ++minutes[i]);
                    
                // associate minute with count, and find most common minute
                var maxMinute = minutes
                    .Select((count, index) => (count, index))
                    .MaxBy(v => v.count);
                return (id: guard.Key, total: minutes.Sum(), maxMinute: maxMinute.First());
            });
            
    var maxByTotalSleep = counts.MaxBy(guard => guard.total).First();
    var maxByTotalMinute = counts.MaxBy(guard => guard.maxMinute.count).First();

    return (
        strategy1: maxByTotalSleep.id * maxByTotalSleep.maxMinute.index,
        strategy2: maxByTotalMinute.id * maxByTotalMinute.maxMinute.index);
}
