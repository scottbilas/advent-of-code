<Query Kind="Statements">
  <NuGetReference>morelinq</NuGetReference>
  <NuGetReference>Shouldly</NuGetReference>
  <NuGetReference>YC.QuickGraph</NuGetReference>
  <Namespace>MoreLinq.Extensions</Namespace>
  <Namespace>QuickGraph</Namespace>
  <Namespace>Shouldly</Namespace>
  <Namespace>System.Linq</Namespace>
</Query>

var valuesStore = new int[10000000];
var nextsStore = new int[10000000];
var prevsStore = new int[10000000];

// sample

CalcScore(10, 1618).ShouldBe(8317);
CalcScore(13, 7999).ShouldBe(146373);
CalcScore(17, 1104).ShouldBe(2764);
CalcScore(21, 6111).ShouldBe(54718);
CalcScore(30, 5807).ShouldBe(37305);

// problem
CalcScore(400, 71864).Dump().ShouldBe(437654);
CalcScore(400, 7186400).Dump().ShouldBe(3689913905);

unsafe long CalcScore(int numPlayers, int lastMarbleValue)
{
    fixed (int* values = valuesStore, nexts = nextsStore, prevs = prevsStore)
    {
        var valueCount = 1;
        values[0] = nexts[0] = prevs[0] = 0;

        var current = 0;
        var removeCount = 22;
        var scores = new long[numPlayers];

        for (var (serial, player) = (1, 1); serial <= lastMarbleValue; ++player, ++serial, --removeCount)
        {
            if (player == numPlayers)
                player = 0;

            if (removeCount == 0)
            {
                removeCount = 23;
                
                for (int i = 0; i < 7; ++i)
                    current = prevs[current];

                scores[player] += serial + values[current];

                var (prev, next) = (prevs[current], nexts[current]);
                prevs[next] = prev;
                current = nexts[prev] = next;
            }
            else
            {
                current = nexts[current];
                current = nexts[current];

                values[valueCount] = serial;
                prevs[valueCount] = prevs[current];
                nexts[valueCount] = current;

                var alloc = valueCount++;
                prevs[current] = nexts[prevs[current]] = alloc;
                current = alloc;
            }
        }

        return scores.Max();
    }    
}
