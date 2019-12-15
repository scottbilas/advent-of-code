<Query Kind="Program">
  <Reference Relative="..\..\..\temp\bin\aoc2019\Debug\netstandard2.1\libaoc2019.dll">C:\proj\advent-of-code\temp\bin\aoc2019\Debug\netstandard2.1\libaoc2019.dll</Reference>
  <Reference Relative="..\..\..\temp\bin\aoc2019\Debug\netstandard2.1\libutils.dll">C:\proj\advent-of-code\temp\bin\aoc2019\Debug\netstandard2.1\libutils.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\netstandard.dll</Reference>
  <NuGetReference>Combinatorics</NuGetReference>
  <NuGetReference>morelinq</NuGetReference>
  <NuGetReference>Shouldly</NuGetReference>
  <NuGetReference>System.Interactive</NuGetReference>
  <Namespace>Aoc2019</Namespace>
  <Namespace>Combinatorics.Collections</Namespace>
  <Namespace>JetBrains.Annotations</Namespace>
  <Namespace>Shouldly</Namespace>
  <Namespace>static Aoc2019.MiscStatics</Namespace>
  <Namespace>static Aoc2019.Utils</Namespace>
  <Namespace>static System.Linq.Enumerable</Namespace>
  <Namespace>static System.Linq.EnumerableEx</Namespace>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Numerics</Namespace>
  <Namespace>Unity.Coding.Editor</Namespace>
  <Namespace>Unity.Coding.Utils</Namespace>
</Query>

void Main()
{
    var folder = Util.CurrentQueryPath.ToNPath().Parent;
    var inputPath = folder.Combine($"{folder.FileName}.input.txt");

    var mem = inputPath.ReadAllBigInts().ToArray();

// --- PART 1 ---

    Solve1(mem).Dump().ShouldBe(2276);

// --- PART 2 ---

    Solve2(mem).Dump().ShouldBe("CBLPJZCU");

}

enum Color { Black, White }
enum Dir { Up, Right, Down, Left }
Dir[] DirRight = new[] { Dir.Right, Dir.Down, Dir.Left, Dir.Up };
Dir[] DirLeft = new[] { Dir.Left, Dir.Up, Dir.Right, Dir.Down };
Int2[] Move = new[] { new Int2(0, -1), new Int2(1, 0), new Int2(0, 1), new Int2(-1, 0) };

IDictionary<Int2, Color> Solve(Color startColor, BigInteger[] mem)
{
    var grid = new Dictionary<Int2, Color>().ToAutoDictionary(_ => Color.Black);
    var pos = Int2.Zero;
    var dir = Dir.Up;

    grid[pos] = startColor;

    var vm = new IntCodeVM(mem, () => grid[pos] == Color.Black ? 0 : 1);
    foreach (var (newColor, newDir) in vm.Run().Batch2())
    {
        grid[pos] = (Color)newColor;
        dir = newDir == 1 ? DirRight[(int)dir] : DirLeft[(int)dir];
        pos += Move[(int)dir];
    }

    return grid;
}

int Solve1(BigInteger[] mem) => Solve(Color.Black, mem).Count;

string Solve2(BigInteger[] mem)
{
    var grid = Solve(Color.White, mem);
    var (min, max) = grid.MinMax();
    return OcrSmallText(max - min + Int2.One, pos => grid[pos] == Color.White);
}