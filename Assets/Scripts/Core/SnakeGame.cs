using System;

namespace SnakeGame.Core
{
    /// <summary>
    /// The core rules engine for the game. Composes SnakeBody, FoodSpawn and
    /// GameBoard, and owns Score and GameState. Has no Unity dependencies at
    /// all, which is what makes it directly unit testable: a test can create
    /// a SnakeGame, call Tick() a few times, and assert on plain C# state.
    ///
    /// Communicates outward via events (Observer pattern) instead of
    /// reaching into UI or GameObjects directly, so the presentation layer
    /// can listen without this class knowing anything about it.
    /// </summary>
    public class SnakeGame
    {
        private readonly GameBoard _board;
        private readonly FoodSpawn _foodSpawn;

        public SnakeBody Snake { get; }
        public GridPos FoodPosition { get; private set; }
        public int Score { get; private set; }
        public GameState State { get; private set; }

        public event Action<int> OnScoreChanged;
        public event Action OnGameOver;
        public event Action<GridPos> OnFoodSpawned;

        public SnakeGame(GameBoard board, FoodSpawn foodSpawn, SnakeBody snake)
        {
            _board = board;
            _foodSpawn = foodSpawn;
            Snake = snake;

            State = GameState.Playing;
            Score = 0;

            FoodPosition = _foodSpawn.SpawnNewPosition(_board.Width, _board.Height, Snake.Segments);
        }

        /// <summary>
        /// Requests a direction change for the snake. Ignored while the
        /// game isn't actively playing - part of the State pattern: input
        /// has different meaning (or no meaning) depending on GameState.
        /// </summary>
        public void ChangeDirection(Direction direction)
        {
            if (State != GameState.Playing)
            {
                return;
            }

            Snake.ChangeDirection(direction);
        }

        /// <summary>
        /// Advances the game by exactly one step: moves the snake, checks
        /// collisions, handles food, and updates state/score. Does nothing
        /// if the game is not currently in the Playing state.
        /// </summary>
        public void Tick()
        {
            if (State != GameState.Playing)
            {
                return;
            }

            Snake.Move();

            if (Snake.CollidesWithSelf() || !_board.IsWithinBounds(Snake.Head))
            {
                State = GameState.GameOver;
                OnGameOver?.Invoke();
                return;
            }

            if (Snake.Head == FoodPosition)
            {
                Snake.Grow();
                Score++;
                OnScoreChanged?.Invoke(Score);

                FoodPosition = _foodSpawn.SpawnNewPosition(_board.Width, _board.Height, Snake.Segments);
                OnFoodSpawned?.Invoke(FoodPosition);
            }
        }

        /// <summary>
        /// Toggles between Playing and Paused. Has no effect once the game
        /// is over.
        /// </summary>
        public void TogglePause()
        {
            if (State == GameState.Playing)
            {
                State = GameState.Paused;
            }
            else if (State == GameState.Paused)
            {
                State = GameState.Playing;
            }
        }
    }
}
