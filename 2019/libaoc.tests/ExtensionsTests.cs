using System;
using System.Drawing;
using NUnit.Framework;
using Shouldly;

namespace Aoc2019
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

        [Test]
        public void PatternSeekingGetItemAt_WithBasicPattern_ReturnsCorrectIndex()
        {
            var list = new[] { 1, 2, 1, 2, };

            list.PatternSeekingGetItemAt(4, 2).ShouldBe(1);
            list.PatternSeekingGetItemAt(5, 2).ShouldBe(2);
            list.PatternSeekingGetItemAt(21, 2).ShouldBe(1);
            list.PatternSeekingGetItemAt(16, 2).ShouldBe(2);

            Should.Throw<IndexOutOfRangeException>(() => list.PatternSeekingGetItemAt(2, 4));
            Should.Throw<IndexOutOfRangeException>(() => list.PatternSeekingGetItemAt(4, 4));
        }
    }
}
