using System;
using System.Drawing;
using NUnit.Framework;
using Shouldly;
using Unity.Coding.Tests;

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

        [Test, Ignore("Function still WIP apparently")]
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

    class NPathExtensionsTests : TestFileSystemFixture
    {
        [Test]
        public void ReadAllInts_WithNoIntsInFile_ReturnsEmpty()
        {
            BaseDir.Combine("1.txt").WriteAllText("").ReadAllInts().ShouldBeEmpty();
            BaseDir.Combine("2.txt").WriteAllText("\n   \n").ReadAllInts().ShouldBeEmpty();
        }

        [Test]
        public void ReadAllInts_WithIntsInFile_ReturnsInts()
        {
            BaseDir.Combine("1.txt").WriteAllText("0").ReadAllInts().ShouldBe(new[] { 0 });
            BaseDir.Combine("2.txt").WriteAllText("-123").ReadAllInts().ShouldBe(new[] { -123 });
            BaseDir.Combine("3.txt").WriteAllText("\n  456 \n").ReadAllInts().ShouldBe(new[] { 456 });
            BaseDir.Combine("4.txt").WriteAllText("\n  1  \r\n  2").ReadAllInts().ShouldBe(new[] { 1, 2 });
            BaseDir.Combine("5.txt").WriteAllText("\n  1    2").ReadAllInts().ShouldBe(new[] { 1, 2 });
            BaseDir.Combine("6.txt").WriteAllText("\n  1    2\n ").ReadAllInts().ShouldBe(new[] { 1, 2 });
        }
    }
}
