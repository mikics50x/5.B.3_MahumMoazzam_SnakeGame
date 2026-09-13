using System.Collections.Generic;
using System.Linq;

namespace SnakeGame.Core
{
    /// <summary>
    /// Owns the snake's segments and current direction. Has no knowledge of
    /// Unity, rendering, or the board - it only knows about grid coordinates.
    /// </summary>
    public class SnakeBody
    {
        private readonly List<GridPos> _segments;
        private bool _pendingGrowth;

        public Direction CurrentDirection { get; private set; }

        /// <summary>Head is always the first element.</summary>
        public GridPos Head => _segments[0];

        public IReadOnlyList<GridPos> Segments => _segments;

        public SnakeBody(GridPos startPosition, Direction startDirection)
        {
            _segments = new List<GridPos> { startPosition };
            CurrentDirection = startDirection;
        }

        /// <summary>
        /// Requests a new direction for the next Move(). Illegal 180-degree
        /// reversals are silently ignored, since a snake can never turn
        /// directly back on itself.
        /// </summary>
        public void ChangeDirection(Direction newDirection)
        {
            if (newDirection.IsOppositeOf(CurrentDirection))
            {
                return;
            }

            CurrentDirection = newDirection;
        }

        /// <summary>
        /// Advances the head by one cell in the current direction. Every
        /// other segment follows the position the segment in front of it
        /// used to occupy. If Grow() was called since the last Move(), the
        /// tail is kept instead of being removed, so the snake gets longer.
        /// </summary>
        public void Move()
        {
            var (dx, dy) = CurrentDirection.ToOffset();
            var newHead = Head.Offset(dx, dy);

            _segments.Insert(0, newHead);

            if (_pendingGrowth)
            {
                _pendingGrowth = false;
            }
            else
            {
                _segments.RemoveAt(_segments.Count - 1);
            }
        }

        /// <summary>
        /// Marks the snake to grow by one segment on the next Move(),
        /// rather than growing instantly - growth should only take visible
        /// effect once the snake actually advances.
        /// </summary>
        public void Grow()
        {
            _pendingGrowth = true;
        }

        /// <summary>
        /// True if the head currently overlaps any other body segment.
        /// </summary>
        public bool CollidesWithSelf()
        {
            return _segments.Skip(1).Any(segment => segment == Head);
        }
    }
}
