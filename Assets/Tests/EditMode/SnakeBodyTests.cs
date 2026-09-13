using NUnit.Framework;
using SnakeGame.Core;

namespace SnakeGame.Core.Tests
{
    public class SnakeBodyTests
    {
        [Test]
        public void Move_AdvancesHeadByOneCellInCurrentDirection()
        {
            var snake = new SnakeBody(new GridPos(5, 5), Direction.Right);

            snake.Move();

            Assert.AreEqual(new GridPos(6, 5), snake.Head);
        }

        [Test]
        public void Move_WithoutGrowth_KeepsSameLength()
        {
            var snake = new SnakeBody(new GridPos(0, 0), Direction.Up);
            var lengthBefore = snake.Segments.Count;

            snake.Move();

            Assert.AreEqual(lengthBefore, snake.Segments.Count);
        }

        [Test]
        public void Grow_AddsOneSegmentOnNextMove()
        {
            var snake = new SnakeBody(new GridPos(0, 0), Direction.Up);
            var lengthBefore = snake.Segments.Count;

            snake.Grow();
            snake.Move();

            Assert.AreEqual(lengthBefore + 1, snake.Segments.Count);
        }

        [Test]
        public void ChangeDirection_IgnoresDirect180DegreeReversal()
        {
            var snake = new SnakeBody(new GridPos(0, 0), Direction.Up);

            snake.ChangeDirection(Direction.Down);

            Assert.AreEqual(Direction.Up, snake.CurrentDirection);
        }

        [Test]
        public void ChangeDirection_AllowsATurn()
        {
            var snake = new SnakeBody(new GridPos(0, 0), Direction.Up);

            snake.ChangeDirection(Direction.Left);

            Assert.AreEqual(Direction.Left, snake.CurrentDirection);
        }

        [Test]
        public void CollidesWithSelf_TrueWhenHeadOverlapsBody()
        {
            // Build a snake that will loop back onto itself after 4 moves
            // in a tight square: right, up, left, down.
            var snake = new SnakeBody(new GridPos(0, 0), Direction.Right);
            snake.Grow();
            snake.Move(); // (1,0), length 2

            snake.Grow();
            snake.ChangeDirection(Direction.Up);
            snake.Move(); // (1,1), length 3

            snake.Grow();
            snake.ChangeDirection(Direction.Left);
            snake.Move(); // (0,1), length 4

            snake.Grow();
            snake.ChangeDirection(Direction.Down);
            snake.Move(); // (0,0) again - overlaps the original tail segment, which
                          // is only still present because Grow() prevented it being trimmed

            Assert.IsTrue(snake.CollidesWithSelf());
        }

        [Test]
        public void CollidesWithSelf_FalseWhenNoOverlap()
        {
            var snake = new SnakeBody(new GridPos(0, 0), Direction.Right);
            snake.Move();

            Assert.IsFalse(snake.CollidesWithSelf());
        }
    }
}
