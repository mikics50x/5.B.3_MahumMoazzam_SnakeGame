using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SnakeGame.Presentation
{
    /// <summary>
    /// Reacts to the core game's OnScoreChanged and OnGameOver events to
    /// update on-screen UI. Never polls game state directly - this is the
    /// Observer pattern from the listening side, matching how SnakeGame
    /// raises those events without knowing anything about Unity UI.
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private Button restartButton;

        private void Start()
        {
            var game = GameManager.Instance.Game;
            game.OnScoreChanged += HandleScoreChanged;
            game.OnGameOver += HandleGameOver;

            gameOverPanel.SetActive(false);
            UpdateScoreText(game.Score);

            if (restartButton != null)
            {
                restartButton.onClick.AddListener(HandleRestartClicked);
            }
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null && GameManager.Instance.Game != null)
            {
                GameManager.Instance.Game.OnScoreChanged -= HandleScoreChanged;
                GameManager.Instance.Game.OnGameOver -= HandleGameOver;
            }
        }

        private void HandleScoreChanged(int newScore)
        {
            UpdateScoreText(newScore);
        }

        private void UpdateScoreText(int score)
        {
            scoreText.text = $"Score: {score}";
        }

        private void HandleGameOver()
        {
            gameOverPanel.SetActive(true);
        }

        private void HandleRestartClicked()
        {
            // Unsubscribe from the old game before GameManager replaces it,
            // then resubscribe to the new one so events keep working.
            var oldGame = GameManager.Instance.Game;
            oldGame.OnScoreChanged -= HandleScoreChanged;
            oldGame.OnGameOver -= HandleGameOver;

            GameManager.Instance.StartNewGame();

            var newGame = GameManager.Instance.Game;
            newGame.OnScoreChanged += HandleScoreChanged;
            newGame.OnGameOver += HandleGameOver;

            gameOverPanel.SetActive(false);
            UpdateScoreText(newGame.Score);
        }
    }
}
