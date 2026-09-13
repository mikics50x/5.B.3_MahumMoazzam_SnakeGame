using UnityEngine;

namespace SnakeGame.Presentation
{
    /// <summary>
    /// Draws a simple rectangular outline around the playable board area
    /// using a LineRenderer, so the invisible grid boundary that
    /// GameBoard.IsWithinBounds() checks against is actually visible to
    /// the player. Purely cosmetic - has no effect on game logic.
    /// </summary>
    [RequireComponent(typeof(LineRenderer))]
    public class BoardBorder : MonoBehaviour
    {
        [Tooltip("Should match GameManager's Board Width.")]
        [SerializeField] private int boardWidth = 20;

        [Tooltip("Should match GameManager's Board Height.")]
        [SerializeField] private int boardHeight = 20;

        [SerializeField] private float lineWidth = 0.1f;
        [SerializeField] private Color borderColor = Color.white;

        private void Start()
        {
            var line = GetComponent<LineRenderer>();

            // The board's cells occupy 0..boardWidth-1 / 0..boardHeight-1.
            // Draw the border half a cell outside that range so it wraps
            // around the outermost cells rather than cutting through them.
            const float margin = 0.5f;
            var minX = -margin;
            var minY = -margin;
            var maxX = boardWidth - 1 + margin;
            var maxY = boardHeight - 1 + margin;

            line.positionCount = 5;
            line.loop = false;
            line.useWorldSpace = true;
            line.startWidth = lineWidth;
            line.endWidth = lineWidth;
            line.startColor = borderColor;
            line.endColor = borderColor;

            line.SetPosition(0, new Vector3(minX, minY, 0f));
            line.SetPosition(1, new Vector3(maxX, minY, 0f));
            line.SetPosition(2, new Vector3(maxX, maxY, 0f));
            line.SetPosition(3, new Vector3(minX, maxY, 0f));
            line.SetPosition(4, new Vector3(minX, minY, 0f));
        }
    }
}
