<Query Kind="Statements">
  <NuGetReference>morelinq</NuGetReference>
  <NuGetReference>Shouldly</NuGetReference>
  <Namespace>MoreLinq.Extensions</Namespace>
  <Namespace>Shouldly</Namespace>
</Query>

// sample

React("dabAcCaCBAcCcaDA").ShouldBe(10);
ReactMinimize("dabAcCaCBAcCcaDA").ShouldBe(4);

// problem

var scriptDir = Path.GetDirectoryName(Util.CurrentQueryPath);

React(File.ReadLines($"{scriptDir}/input.txt").First()).Dump().ShouldBe(9370);
ReactMinimize(File.ReadLines($"{scriptDir}/input.txt").First()).Dump().ShouldBe(6390);

// todo: well this is simple, but the slowest thing in the world. switch to a "deleted cell" method instead.
int React(string input)
{
    var chars = new LinkedList<char>(input);
    
    for (var i = chars.First; i != chars.Last;)
    {
        var next = i.Next;
        if (char.ToLower(i.Value) == char.ToLower(next.Value) && i.Value != next.Value)
        {
            chars.Remove(i);
            chars.Remove(next);
            i = chars.First;
        }
        else
            i = next;
    }
    
    return chars.ToArray().Length;
}

int ReactMinimize(string input)
{
    // todo's:
    // * react before doing char-specific reacting (test to see if would be faster..should be)
    // * char-specific reacting gets new input string, rather than just seeding the delete cell array
    // * parallelize
    // * single simultaneous pass for all 26 (make deleted cell a bit array? nah)
    return Enumerable.Range(0, 26)
        .Select(i => (char)('a' + i))
        .Select(c => $"[{c}{char.ToUpper(c)}]")
        .Distinct()
        .Select(pattern => React(Regex.Replace(input, pattern, "")))
        .Min();
}
