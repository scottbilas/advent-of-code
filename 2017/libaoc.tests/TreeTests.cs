using System;
using System.Linq;
using NUnit.Framework;
using Shouldly;
using static Aoc2017.MiscStatics;

namespace Aoc2017
{
    class TreeTests
    {
    }

    class TreeWalkingTests
    {
        [Test] public void SelectTree_WithSingleNodeTree_ReturnsSelf()
        {
            var root = new Tree<Void>();
            var tree = root.SelectTree().ToArray();

            tree.ShouldBe(Arr(root));
        }

        [TestCase("r-c0 c0-c0a c0a-c0a0 r-c1 c1-c1a", "r c0 c0a c0a0 c1 c1a")] // base
        [TestCase("c0a-c0a0 c1-c1a c0-c0a r-c0 r-c1", "r c0 c0a c0a0 c1 c1a")] // rearrange leaves
        [TestCase("r-c1 r-c0 c0-c0a c0a-c0a0 c1-c1a", "r c1 c1a c0 c0a c0a0")] // swap subtrees
        public void SelectTree_WithMultiNodeTree_ReturnsTreeDepthFirstChildOrder(string linksText, string expectedKeysText)
        {
            var links = linksText.SelectWords().Batch2();
            var expectedKeys = expectedKeysText.SelectWords();

            var root = Tree.Create(links).GetRoot();
            var actualKeys = root.SelectTree().SelectKeys();

            actualKeys.ShouldBe(expectedKeys);
        }

        [TestCase("a", RecurseResult.Continue, "a b c")]
        [TestCase("b", RecurseResult.Continue, "a b c")]
        [TestCase("c", RecurseResult.Continue, "a b c")]
        [TestCase("a", RecurseResult.StopAndKeep, "a")]
        [TestCase("b", RecurseResult.StopAndKeep, "a b")]
        [TestCase("c", RecurseResult.StopAndKeep, "a b c")]
        [TestCase("a", RecurseResult.StopAndSkip, "")]
        [TestCase("b", RecurseResult.StopAndSkip, "a")]
        [TestCase("c", RecurseResult.StopAndSkip, "a b")]
        public void SelectTree_WithSimpleRecurseFilter(string test, RecurseResult result, string expectedKeysText)
        {
            var root = Tree.Create(("a", "b"), ("b", "c")).GetRoot();
            var actualKeys = root.SelectTree(n => n.Key == test ? result : RecurseResult.Continue).SelectKeys();
            var expectedKeys = expectedKeysText.SelectWords();

            actualKeys.ShouldBe(expectedKeys);
        }

        [Test]
        public void SelectTree_WithRecurseFilter_StopsRecurseAndGetsNextChild()
        {
            var root = Tree.Create(("a", "b"), ("b", "c"), ("a", "d")).GetRoot();
            var actualKeys = root.SelectTree(n => n.Key == "b" ? RecurseResult.StopAndSkip : RecurseResult.Continue).SelectKeys();

            actualKeys.ShouldBe(Arr("a", "d"));
        }
    }
}
