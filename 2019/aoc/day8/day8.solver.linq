<Query Kind="Program">
  <Reference Relative="..\..\libaoc\bin\Debug\netstandard2.1\libaoc2019.dll">C:\proj\advent-of-code\2019\libaoc\bin\Debug\netstandard2.1\libaoc2019.dll</Reference>
  <Reference Relative="..\..\libaoc\bin\Debug\netstandard2.1\libutils.dll">C:\proj\advent-of-code\2019\libaoc\bin\Debug\netstandard2.1\libutils.dll</Reference>
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
  <Namespace>static System.Linq.Enumerable</Namespace>
  <Namespace>static System.Linq.EnumerableEx</Namespace>
  <Namespace>System.Drawing</Namespace>
  <Namespace>Unity.Coding.Editor</Namespace>
  <Namespace>Unity.Coding.Utils</Namespace>
  <Namespace>System.Drawing.Imaging</Namespace>
</Query>

void Main()
{
    var folder = Util.CurrentQueryPath.ToNPath().Parent;
    var inputPath = folder.Combine($"{folder.FileName}.input.txt");

    var pixels = inputPath.ReadAllText().Trim();
    var size = new Size(25, 6);
    var layerLen = size.Width * size.Height;
    var layerCount = pixels.Length / layerLen;

// --- PART 1 ---

    With(Range(0, layerCount)
            .Select(layer => Range(layer * layerLen, layerLen)
                .Select(i => pixels[i])
                .DistinctWithCount())
            .MinBy(counts => counts['0']).First(),
        counts => (counts['1'] * counts['2']).Dump().ShouldBe(1330));

// --- PART 2 ---

    var image = new bool[size.Width, size.Height];
    for (var y = 0; y < size.Height; ++y)
    {
        for (var x = 0; x < size.Width; ++x)
        {
            for (var layer = 0; layer < layerCount; ++layer)
            {
                var pixel = pixels[layer * layerLen + (size.Width * y) + x];
                if (pixel != '2')
                {
                    image[x, y] = pixel == '1';
                    break;
                }
            }
        }
    }

    var renderPath = inputPath.Parent.Combine(inputPath.Parent.FileName + ".results.png");
    try
    {
        Utils.RenderForOcr(renderPath, image);
        Utils.Ocr(renderPath).Dump().ShouldBe("FAHEF");
    }
    finally
    {
        renderPath.DeleteIfExists();
    }
}
