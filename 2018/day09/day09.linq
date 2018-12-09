<Query Kind="Program">
  <NuGetReference>morelinq</NuGetReference>
  <NuGetReference>Shouldly</NuGetReference>
  <NuGetReference>YC.QuickGraph</NuGetReference>
  <Namespace>MoreLinq.Extensions</Namespace>
  <Namespace>QuickGraph</Namespace>
  <Namespace>Shouldly</Namespace>
  <Namespace>System.Linq</Namespace>
</Query>

void Main()
{
    // sample

    CalcScore(10, 1618).ShouldBe(8317);
    CalcScore(13, 7999).ShouldBe(146373);
    CalcScore(17, 1104).ShouldBe(2764);
    CalcScore(21, 6111).ShouldBe(54718);
    CalcScore(30, 5807).ShouldBe(37305);

    // problem
    CalcScore(400, 71864).Dump().ShouldBe(437654);
    CalcScore(400, 7186400).Dump().ShouldBe(3689913905);
}

long CalcScore(int numPlayers, int lastMarbleValue)
{
    var circle = new LinkedList<int>();
    circle.AddLast(0);

    var current = circle.First;
    var serial = 1;
    var player = 1;
    var scores = new long[numPlayers];

    for (; ; )
    {
        if (serial % 23 == 0)
        {
            scores[player] += serial;
            for (int i = 0; i < 7; ++i)
                current = current.Previous ?? circle.Last;
            scores[player] += current.Value;
            var remove = current;
            current = current.Next ?? circle.First;
            circle.Remove(remove);
        }
        else
        {
            for (int i = 0; i < 2; ++i)
                current = current.Next ?? circle.First;
            current = circle.AddBefore(current, serial);
        }

        if (serial == lastMarbleValue)
            return scores.Max();

        ++player;
        if (player == numPlayers)
            player = 0;
        ++serial;
    }
}
