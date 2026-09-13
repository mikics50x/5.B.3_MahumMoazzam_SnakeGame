using System;
using NUnit.Framework;
using SnakeGame.Core;

namespace SnakeGame.Core.Tests
{
    public class SnakeGameTests
    {
        private static SnakeGame CreateGame(int width, int height, GridPos start, Direction direction, int seed)
        {
            var board = new GameBoard(width, height);
            var foodSpawn = new FoodSpawn(new Random(seed));
            var snake = new SnakeBody(start, direction);
            return new SnakeGame(board, foodSpawn, snake);
        }

        [Test]
        public void Tick_MovesSnakeOneCellForward()
        {
            var game = CreateGame(10, 10, new GridPos(5, 5), Direction.Right, seed: 1);

            game.Tick();

            Assert.AreEqual(new GridPos(6, 5), game.Snake.Head);
        }

        [Test]
        public void Tick_OnWallCollision_SetsStateToGameOver()
        {
            // Board is only 3 wide; snake starts at the rightmost cell
            // already facing further right, so the very next Tick() walks
            // it off the board.
            var game = CreateGame(3, 3, new GridPos(2, 1), Direction.Right, seed: 1);

            game.Tick();

            Assert.AreEqual(GameState.GameOver, game.State);
        }

        [Test]
        public void Tick_AfterGameOver_DoesNothingOnFurtherTicks()
        {
            var game = CreateGame(3, 3, new GridPos(2, 1), Direction.Right, seed: 1);
            game.Tick(); // triggers game over

            var headBeforeSecondTick = game.Snake.Head;
            game.Tick();

            Assert.AreEqual(headBeforeSecondTick, game.Snake.Head);
            Assert.AreEqual(GameState.GameOver, game.State);
        }

        [Test]
        public void ChangeDirection_IsIgnoredWhenNotPlaying()
        {
            var game = CreateGame(3, 3, new GridPos(2, 1), Direction.Right, seed: 1);
            game.Tick(); // now GameOver

            game.ChangeDirection(Direction.Up);

            Assert.AreEqual(Direction.Right, game.Snake.CurrentDirection);
        }

        [Test]
        public void TogglePause_SwitchesBetweenPlayingAndPaused()
        {
            var game = CreateGame(10, 10, new GridPos(5, 5), Direction.Right, seed: 1);

            game.TogglePause();
            Assert.AreEqual(GameState.Paused, game.State);

            game.TogglePause();
            Assert.AreEqual(GameState.Playing, game.State);
        }

        [Test]
        public void Constructor_SpawnsFoodWithinBoardBounds()
        {
            var game = CreateGame(5, 5, new GridPos(0, 0), Direction.Right, seed: 42);

            var board = new GameBoard(5, 5);
            Assert.IsTrue(board.IsWithinBounds(game.FoodPosition));
        }
    }
}
