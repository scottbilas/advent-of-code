<Query Kind="Statements">
  <NuGetReference>morelinq</NuGetReference>
  <NuGetReference>Shouldly</NuGetReference>
  <Namespace>MoreLinq.Extensions</Namespace>
  <Namespace>Shouldly</Namespace>
</Query>

// sample

React("dabAcCaCBAcCcaDA").Length.ShouldBe(10);
ReactMinLength("dabAcCaCBAcCcaDA").ShouldBe(4);

// problem

var scriptDir = Path.GetDirectoryName(Util.CurrentQueryPath);

React(File.ReadLines($"{scriptDir}/input.txt").First()).Length.Dump().ShouldBe(9370);
ReactMinLength(File.ReadLines($"{scriptDir}/input.txt").First()).Dump().ShouldBe(6390);

string React(string input, char ignore = '\0')
{
    var output = new Stack<char>();

    for (var i = 0; i < input.Length; ++i)
    {
        var cur = input[i];
        if ((cur & ~32) == ignore)
            continue;
        
        if (output.Count > 0)
        {
            var prv = output.Peek();
            if (((int)cur & ~32) == ((int)prv & ~32) && cur != prv)
            {
                output.Pop();
                continue;
            }
        }

        output.Push(cur);
    }

    return new string(output.ToArray());
}

int ReactMinLength(string input)
{
    input = React(input);
    return Enumerable.Range(0, 26)
        .AsParallel()
        .Select(i => React(input, (char)('A' + i)).Length)
        .Min();
}
