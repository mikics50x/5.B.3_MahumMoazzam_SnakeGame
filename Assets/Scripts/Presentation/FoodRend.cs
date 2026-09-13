using UnityEngine;
using SnakeGame.Core;

namespace SnakeGame.Presentation
{
    /// <summary>
    /// Moves this GameObject to match the core logic's current food
    /// position. Reacts to the OnFoodSpawned event rather than polling
    /// position every frame, since food only actually moves when eaten -
    /// but does check every frame whether GameManager has handed out a
    /// new SnakeGame (i.e. after a restart), so it can resubscribe to the
    /// new game's events instead of staying attached to the old one.
    /// </summary>
    public class FoodRend : MonoBehaviour
    {
        [SerializeField] private float cellSize = 1f;

        private SnakeGame.Core.SnakeGame _subscribedGame;

        private void Update()
        {
            var game = GameManager.Instance.Game;

            if (game != _subscribedGame)
            {
                Resubscribe(game);
            }
        }

        private void Resubscribe(SnakeGame.Core.SnakeGame newGame)
        {
            if (_subscribedGame != null)
            {
                _subscribedGame.OnFoodSpawned -= HandleFoodSpawned;
            }

            newGame.OnFoodSpawned += HandleFoodSpawned;
            _subscribedGame = newGame;

            // Snap to the new game's first food position immediately,
            // since that food was already spawned in SnakeGame's
            // constructor before we had a chance to hear about it.
            MoveToPosition(newGame.FoodPosition);
        }

        private void OnDestroy()
        {
            if (_subscribedGame != null)
            {
                _subscribedGame.OnFoodSpawned -= HandleFoodSpawned;
            }
        }

        private void HandleFoodSpawned(GridPos newPosition)
        {
            MoveToPosition(newPosition);
        }

        private void MoveToPosition(GridPos position)
        {
            transform.position = new Vector3(position.X * cellSize, position.Y * cellSize, 0f);
        }
    }
}
