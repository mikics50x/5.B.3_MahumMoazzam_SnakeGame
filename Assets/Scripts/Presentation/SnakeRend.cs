using System.Collections.Generic;
using UnityEngine;
using SnakeGame.Core;

namespace SnakeGame.Presentation
{
    /// <summary>
    /// Keeps a pool of segment GameObjects in sync with the core
    /// SnakeBody's list of GridPos segments. Converts grid coordinates to
    /// world-space positions - the core logic never has to know Unity's
    /// coordinate system exists.
    /// </summary>
    public class SnakeRend : MonoBehaviour
    {
        [SerializeField] private GameObject segmentPrefab;
        [Tooltip("World-space size of one grid cell.")]
        [SerializeField] private float cellSize = 1f;

        private readonly List<GameObject> _segmentInstances = new List<GameObject>();

        // Tracks which SnakeGame instance we're currently subscribed to, so
        // Update() can notice when GameManager hands out a brand new one
        // (i.e. after a restart) and resync accordingly. This is checked
        // every frame instead of relying on an event fired once, since this
        // component freezes its own visual updates on game over and would
        // otherwise never see the restart happen.
        private SnakeGame.Core.SnakeGame _subscribedGame;
        private bool _isFrozen;

        private void Update()
        {
            var game = GameManager.Instance.Game;

            if (game != _subscribedGame)
            {
                Resubscribe(game);
            }

            if (_isFrozen)
            {
                return;
            }

            var snake = game.Snake;
            SyncSegmentCount(snake.Segments.Count);

            for (var i = 0; i < snake.Segments.Count; i++)
            {
                _segmentInstances[i].transform.position = GridToWorld(snake.Segments[i]);
            }
        }

        private void Resubscribe(SnakeGame.Core.SnakeGame newGame)
        {
            if (_subscribedGame != null)
            {
                _subscribedGame.OnGameOver -= HandleGameOver;
            }

            newGame.OnGameOver += HandleGameOver;
            _subscribedGame = newGame;

            // A fresh game means a fresh snake - clear out any leftover
            // segment instances from the previous run so SyncSegmentCount
            // rebuilds them at the new snake's positions instead of
            // reusing stale ones.
            foreach (var instance in _segmentInstances)
            {
                Destroy(instance);
            }
            _segmentInstances.Clear();

            _isFrozen = false;
        }

        private void OnDestroy()
        {
            if (_subscribedGame != null)
            {
                _subscribedGame.OnGameOver -= HandleGameOver;
            }
        }

        private void SyncSegmentCount(int targetCount)
        {
            while (_segmentInstances.Count < targetCount)
            {
                var instance = Instantiate(segmentPrefab, transform);
                _segmentInstances.Add(instance);
            }

            while (_segmentInstances.Count > targetCount)
            {
                var last = _segmentInstances[_segmentInstances.Count - 1];
                _segmentInstances.RemoveAt(_segmentInstances.Count - 1);
                Destroy(last);
            }
        }

        private Vector3 GridToWorld(GridPos position)
        {
            return new Vector3(position.X * cellSize, position.Y * cellSize, 0f);
        }

        private void HandleGameOver()
        {
            // Freeze the snake's visuals in place on game over rather than
            // destroying them, so the player can see where they died.
            // Update() keeps running (unlike disabling the component)
            // specifically so it can detect a restart and unfreeze itself.
            _isFrozen = true;
        }
    }
}
