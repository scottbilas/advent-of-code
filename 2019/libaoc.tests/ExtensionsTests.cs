using System;
using System.Linq;
using NUnit.Framework;
using Shouldly;
using static Aoc2019.MiscStatics;

namespace Aoc2019
{
    class ExtensionsTests
    {
        [Test]
        public void TextToGrid_WithSingleLine_ReturnsSingleRowGrid()
        {
            var grid = "abc".ToGrid();
            grid.GetDimensions().ShouldBe(new Int2(3, 1));
            grid[0, 0].ShouldBe('a');
            grid[1, 0].ShouldBe('b');
            grid[2, 0].ShouldBe('c');
        }

        [Test]
        public void SelectBorderCells_Basics()
        {
            var grid = @"
                abcde
                fghij
                klmno
                pqrst
                uvwxy".ToGrid();
            grid.SelectBorderCells().SelectXy().ShouldBe(ignoreOrder: true, expected: Arr(
                (0, 0, 'a'), (1, 0, 'b'), (2, 0, 'c'), (3, 0, 'd'), (4, 0, 'e'),
                (0, 1, 'f'),                                        (4, 1, 'j'),
                (0, 2, 'k'),                                        (4, 2, 'o'),
                (0, 3, 'p'),                                        (4, 3, 't'),
                (0, 4, 'u'), (1, 4, 'v'), (2, 4, 'w'), (3, 4, 'x'), (4, 4, 'y')));
            grid.SelectBorderCells(2).SelectXy().ShouldBe(ignoreOrder: true, expected: Arr(
                (0, 0, 'a'), (1, 0, 'b'), (2, 0, 'c'), (3, 0, 'd'), (4, 0, 'e'),
                (0, 1, 'f'), (1, 1, 'g'), (2, 1, 'h'), (3, 1, 'i'), (4, 1, 'j'),
                (0, 2, 'k'), (1, 2, 'l'),              (3, 2, 'n'), (4, 2, 'o'),
                (0, 3, 'p'), (1, 3, 'q'), (2, 3, 'r'), (3, 3, 's'), (4, 3, 't'),
                (0, 4, 'u'), (1, 4, 'v'), (2, 4, 'w'), (3, 4, 'x'), (4, 4, 'y')));
        }

        [Test]
        public void ReduceFraction()
        {
            new Int2(0, 0).ReduceFraction().ShouldBe(new Int2(0, 0));
            new Int2(1, 0).ReduceFraction().ShouldBe(new Int2(1, 0));
            new Int2(2, 0).ReduceFraction().ShouldBe(new Int2(1, 0));
            new Int2(-2, 0).ReduceFraction().ShouldBe(new Int2(-1, 0));
            new Int2(-3, 24).ReduceFraction().ShouldBe(new Int2(-1, 8));
            new Int2(12, 33).ReduceFraction().ShouldBe(new Int2(4, 11));
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
}
