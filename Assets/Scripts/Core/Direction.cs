namespace SnakeGame.Core
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    /// <summary>
    /// Small helper methods for Direction. Kept as an extension class rather
    /// than bloating GridPos or SnakeBody with direction-only logic.
    /// </summary>
    public static class DirectionExtensions
    {
        /// <summary>
        /// True if this direction is the direct 180-degree opposite of
        /// the other. Used by SnakeBody to block the snake from reversing
        /// directly into its own neck.
        /// </summary>
        public static bool IsOppositeOf(this Direction direction, Direction other)
        {
            return (direction == Direction.Up && other == Direction.Down)
                || (direction == Direction.Down && other == Direction.Up)
                || (direction == Direction.Left && other == Direction.Right)
                || (direction == Direction.Right && other == Direction.Left);
        }

        /// <summary>
        /// Converts a direction into a one-cell (dx, dy) offset.
        /// </summary>
        public static (int dx, int dy) ToOffset(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Up: return (0, 1);
                case Direction.Down: return (0, -1);
                case Direction.Left: return (-1, 0);
                case Direction.Right: return (1, 0);
                default: return (0, 0);
            }
        }
    }
}
