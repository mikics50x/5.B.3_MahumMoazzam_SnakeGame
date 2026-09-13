using UnityEngine;
using SnakeGame.Core;

namespace SnakeGame.Presentation
{
    /// <summary>
    /// Owns the single core SnakeGame instance for the scene and drives it
    /// forward on a fixed timer. Every other presentation script reaches
    /// the game's state through GameManager.Instance rather than holding
    /// its own reference - this is the Singleton pattern, justified here
    /// because exactly one game runs per scene and several unrelated
    /// scripts (input, rendering, UI) all need to reach it.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Board")]
        [SerializeField] private int boardWidth = 20;
        [SerializeField] private int boardHeight = 20;

        [Header("Snake")]
        [SerializeField] private Vector2Int startPosition = new Vector2Int(10, 10);
        [SerializeField] private Direction startDirection = Direction.Right;

        [Header("Timing")]
        [Tooltip("Seconds between each game step. Lower = faster snake.")]
        [SerializeField] private float tickInterval = 0.15f;

        /// <summary>The core rules engine. Presentation scripts read from this, never write game rules themselves.</summary>
        public SnakeGame.Core.SnakeGame Game { get; private set; }

        public int BoardWidth => boardWidth;
        public int BoardHeight => boardHeight;

        private float _tickTimer;

        private void Awake()
        {
            // Standard singleton guard: if a duplicate GameManager somehow
            // ends up in the scene, remove it rather than having two
            // separate games running.
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            StartNewGame();
        }

        private void Update()
        {
            if (Game.State != GameState.Playing)
            {
                return;
            }

            _tickTimer += Time.deltaTime;
            if (_tickTimer >= tickInterval)
            {
                _tickTimer = 0f;
                Game.Tick();
            }
        }

        /// <summary>
        /// Builds a fresh SnakeGame from the configured settings. Public so
        /// the UI can call this to restart after game over.
        /// </summary>
        public void StartNewGame()
        {
            var board = new GameBoard(boardWidth, boardHeight);
            var foodSpawn = new FoodSpawn();
            var snakeStart = new GridPos(startPosition.x, startPosition.y);
            var snake = new SnakeBody(snakeStart, startDirection);

            Game = new SnakeGame.Core.SnakeGame(board, foodSpawn, snake);
            _tickTimer = 0f;
        }
    }
}
