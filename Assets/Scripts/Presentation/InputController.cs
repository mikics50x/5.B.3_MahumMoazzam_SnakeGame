using UnityEngine;
using SnakeGame.Core;

namespace SnakeGame.Presentation
{
    /// <summary>
    /// Reads player input each frame and forwards it to the core game as a
    /// direction change. Contains no game rules of its own - legality of a
    /// direction change (e.g. blocking 180-degree reversals) is entirely
    /// SnakeBody's responsibility, not this script's.
    /// </summary>
    public class InputController : MonoBehaviour
    {
        private void Update()
        {
            var game = GameManager.Instance.Game;

            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                game.ChangeDirection(Direction.Up);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                game.ChangeDirection(Direction.Down);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                game.ChangeDirection(Direction.Left);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                game.ChangeDirection(Direction.Right);
            }
        }
    }
}
