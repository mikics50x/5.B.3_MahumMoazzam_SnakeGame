namespace SnakeGame.Core
{
    /// <summary>
    /// Holds board dimensions and answers boundary questions. Deliberately
    /// tiny - its only job is knowing what counts as "inside the board".
    /// </summary>
    public class GameBoard
    {
        public int Width { get; }
        public int Height { get; }

        public GameBoard(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public bool IsWithinBounds(GridPos position)
        {
            return position.X >= 0 && position.X < Width
                && position.Y >= 0 && position.Y < Height;
        }
    }
}
