using NUnit.Framework;
using Shouldly;

namespace Aoc2017
{
    class DirTests
    {
        [Test]
        public void Turn()
        {
            Dir.N.TurnLeft().ShouldBe(Dir.W);
            Dir.W.TurnLeft().ShouldBe(Dir.S);
            Dir.S.TurnLeft().ShouldBe(Dir.E);
            Dir.E.TurnLeft().ShouldBe(Dir.N);

            Dir.N.TurnRight().ShouldBe(Dir.E);
            Dir.E.TurnRight().ShouldBe(Dir.S);
            Dir.S.TurnRight().ShouldBe(Dir.W);
            Dir.W.TurnRight().ShouldBe(Dir.N);
        }
    }
}
