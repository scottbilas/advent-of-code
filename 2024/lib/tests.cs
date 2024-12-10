using System.Reflection;
using System.Text.RegularExpressions;

[Parallelizable(ParallelScope.All)]
abstract class Fixture
{
    protected Fixture()
    {
        Day = GetType().Name.Ints().Single();

        InputDir = Assembly
            .GetExecutingAssembly()
            .GetCustomAttribute<InputLocationAttribute>()!.Location
            .ToNPath().DirectoryMustExist();

        InputFile = InputDir.Combine($"day{Day}.input.txt");
        ResultsFile = InputDir.Combine($"day{Day}.results.md");
    }

    protected readonly int Day;
    protected readonly NPath InputDir;
    protected readonly NPath InputFile;
    protected string Input => InputFile.ReadAllText().Trim();

    protected readonly NPath ResultsFile;
    protected NPath Results => ResultsFile.ReadAllText();

    protected string GetSampleInput(int index) => Regex
        .Split(Results, "```")
        .Where((_, i) => i % 2 == 1 && i/2== index)
        .First()
        .Trim();

    protected string SampleInput => GetSampleInput(0);
    protected string SampleInput1 => GetSampleInput(0);
    protected string SampleInput2 => GetSampleInput(1);
}

[AttributeUsage(AttributeTargets.Assembly)]
class InputLocationAttribute(string location) : Attribute
{
    public string Location => location;
}
