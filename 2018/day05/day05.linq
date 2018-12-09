<Query Kind="Statements">
  <NuGetReference>Shouldly</NuGetReference>
  <Namespace>Shouldly</Namespace>
</Query>

// sample

ReactLength("dabAcCaCBAcCcaDA").ShouldBe(10);
ReactMinLength("dabAcCaCBAcCcaDA").ShouldBe(4);

// problem

var scriptDir = Path.GetDirectoryName(Util.CurrentQueryPath);

ReactLength(File.ReadLines($"{scriptDir}/input.txt").First()).Dump().ShouldBe(9370);
ReactMinLength(File.ReadLines($"{scriptDir}/input.txt").First()).Dump().ShouldBe(6390);

(char[] output, int outputLen) React(string input, char ignore = '\0')
{
    var output = new char[input.Length];
    var outputLen = 0;

    for (var i = 0; i < input.Length; ++i)
    {
        var cur = input[i];
        if ((cur & ~32) == ignore)
            continue;
        
        if (outputLen > 0)
        {
            var prv = output[outputLen - 1];
            if (((int)cur & ~32) == ((int)prv & ~32) && cur != prv)
            {
                --outputLen;
                continue;
            }
        }

        output[outputLen++] = cur;
    }

    return (output, outputLen);
}

int ReactLength(string input)
{
    return React(input).outputLen;
}

int ReactMinLength(string input)
{
    var simplify = React(input);
    input = new string(simplify.output, 0, simplify.outputLen);
    return Enumerable.Range(0, 26)
        .AsParallel()
        .Select(i => React(input, (char)('A' + i)).outputLen)
        .Min();
}
