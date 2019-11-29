<Query Kind="Statements">
  <NuGetReference>morelinq</NuGetReference>
  <NuGetReference>Shouldly</NuGetReference>
  <Namespace>MoreLinq.Extensions</Namespace>
  <Namespace>Shouldly</Namespace>
  <Namespace>System.Linq</Namespace>
</Query>

string scriptDir = Path.GetDirectoryName(Util.CurrentQueryPath);

// sample

Solve(9).ShouldBe("5158916779");
Solve(5).ShouldBe("0124515891");
Solve(18).ShouldBe("9251071085");
Solve(2018).ShouldBe("5941429882");

Solve2("51589").ShouldBe(9);
Solve2("01245").ShouldBe(5);
Solve2("92510").ShouldBe(18);
Solve2("59414").ShouldBe(2018);

Solve3("51589").ShouldBe(9);
Solve3("01245").ShouldBe(5);
Solve3("92510").ShouldBe(18);
Solve3("59414").ShouldBe(2018);

Solve3("880751").ShouldBe(20333868); // diki's

// problem

Solve(909441).Dump().ShouldBe("2615161213");
//Solve2("909441").Dump();
//Console.WriteLine(DateTime.Now);
Solve3("909441").Dump().ShouldBe(20403320);

string Solve(int until)
{
    var (elf0, elf1) = (0, 1);
    var recipes = new List<int> { 3, 7 };

    while (recipes.Count < (until + 10))
    {
        var sum = recipes[elf0] + recipes[elf1];
        if (sum > 9)
        {
            recipes.Add(sum / 10);
            recipes.Add(sum % 10);
        }
        else
            recipes.Add(sum);

        elf0 += 1 + recipes[elf0];
        while (elf0 >= recipes.Count)
            elf0 -= recipes.Count;
        elf1 += 1 + recipes[elf1];
        while (elf1 >= recipes.Count)
            elf1 -= recipes.Count;
    }

    return new string(recipes.Skip(until).Take(10).Select(i => (char)(i + '0')).ToArray());
}

int Solve2(string until)
{
    var (elf0, elf1) = (0, 1);
    var recipes = new List<int> { 3, 7 };

    for (;;)
    {
        var sum = recipes[elf0] + recipes[elf1];
        if (sum > 9)
        {
            recipes.Add(sum / 10);
            recipes.Add(sum % 10);
        }
        else
            recipes.Add(sum);

        elf0 += 1 + recipes[elf0];
        while (elf0 >= recipes.Count)
            elf0 -= recipes.Count;
        elf1 += 1 + recipes[elf1];
        while (elf1 >= recipes.Count)
            elf1 -= recipes.Count;

        if (recipes.TakeLast(until.Length).Select(i => (char)(i + '0')).SequenceEqual(until))
            return recipes.Count - until.Length;
            
    }
}

int Solve3(string until)
{
    var lut = new (byte, byte)[10, 10];
    for (var i = 0; i < 10; ++i)
        for (var j = 0; j < 10; ++j)
        {
            var sum = i + j;
            if (sum > 9)
                lut[i, j] = ((byte)(sum / 10), (byte)(sum % 10));
            else
                lut[i, j] = ((byte)sum, 255);
        }

    var (elf0, elf1) = (0, 1);
    var recipes = new List<byte> { 3, 7 };
    var iuntil = until.Select(c => (byte)(c - '0')).ToArray();

    for (; ; )
    {
        var created = lut[recipes[elf0], recipes[elf1]];
        recipes.Add(created.Item1);
        if (created.Item2 != 255)
            recipes.Add(created.Item2);

        elf0 += 1 + recipes[elf0];
        while (elf0 >= recipes.Count)
            elf0 -= recipes.Count;
        elf1 += 1 + recipes[elf1];
        while (elf1 >= recipes.Count)
            elf1 -= recipes.Count;

/*
        var sb = new StringBuilder();
        for (int i = 0; i < recipes.Count; ++i)
        {
            var c = (char)(recipes[i] + '0');
            if (i == elf0)
                sb.Append($"({c})");
            else if (i == elf1)
                sb.Append($"[{c}]");
            else
                sb.Append($" {c} ");
        }
        Console.WriteLine(sb.ToString());
*/

        if (recipes.Count > until.Length + 1)
        {
            var found = true;
            for (var (r, u) = (recipes.Count - until.Length, 0) ; u < until.Length; ++r, ++u)
            {
                if (recipes[r] != iuntil[u])
                {
                    found = false;
                    break;
                }
            }

            if (found)
                return recipes.Count - until.Length;


            if (!found)
            {
                found = true;
                for (var (r, u) = (recipes.Count - until.Length - 1, 0); u < until.Length; ++r, ++u)
                {
                    if (recipes[r] != iuntil[u])
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                    return recipes.Count - until.Length - 1;
            }
        }
    }
}