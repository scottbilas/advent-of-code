using System.Drawing;
using NUnit.Framework;
using Shouldly;

namespace AoC
{
    class ExtensionsTests
    {
        [Test]
        public void TextToGrid_WithSingleLine_ReturnsSingleRowGrid()
        {
            var grid = "abc".ToGrid();
            grid.GetDimensions().ShouldBe(new Size(3, 1));
            grid[0, 0].ShouldBe('a');
            grid[1, 0].ShouldBe('b');
            grid[2, 0].ShouldBe('c');
        }
    }
}
