using System;
using NUnit.Framework;
using Shouldly;
using Unity.Coding.Tests;

namespace Aoc2019
{
    class NPathExtensionsTests : TestFileSystemFixture
    {
        [Test]
        public void ReadAllInts_WithNoIntsInFile_ReturnsEmpty()
        {
            BaseDir.Combine("1.txt").WriteAllText("").ReadAllInts().ShouldBeEmpty();
            BaseDir.Combine("2.txt").WriteAllText("\n   \n").ReadAllInts().ShouldBeEmpty();
        }

        [Test]
        public void ReadAllFloats_WithNoFloatsInFile_ReturnsEmpty()
        {
            BaseDir.Combine("1.txt").WriteAllText("").ReadAllFloats().ShouldBeEmpty();
            BaseDir.Combine("2.txt").WriteAllText("\n   \n").ReadAllFloats().ShouldBeEmpty();
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

        [Test]
        public void ReadAllFloats_WithFloatsInFile_ReturnsFloats()
        {
            BaseDir.Combine("1.txt").WriteAllText("0").ReadAllFloats().ShouldBe(new[] { 0f });
            BaseDir.Combine("2.txt").WriteAllText("-123.456").ReadAllFloats().ShouldBe(new[] { -123.456f });
            BaseDir.Combine("3.txt").WriteAllText("\n  456.789 \n").ReadAllFloats().ShouldBe(new[] { 456.789f });
            BaseDir.Combine("4.txt").WriteAllText("\n  1.2  \r\n  2.3").ReadAllFloats().ShouldBe(new[] { 1.2f, 2.3f });
            BaseDir.Combine("5.txt").WriteAllText("\n  1.3    2.4").ReadAllFloats().ShouldBe(new[] { 1.3f, 2.4f });
            BaseDir.Combine("6.txt").WriteAllText("\n  1.4    2.5\n ").ReadAllFloats().ShouldBe(new[] { 1.4f, 2.5f });
        }
    }
}
