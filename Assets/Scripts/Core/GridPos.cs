using System;

namespace SnakeGame.Core
{
    /// <summary>
    /// A single cell coordinate on the game board. Immutable value type -
    /// two GridPos instances with the same X/Y are considered equal.
    /// </summary>
    public readonly struct GridPos : IEquatable<GridPos>
    {
        public int X { get; }
        public int Y { get; }

        public GridPos(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Returns a new GridPos offset by the given amount. GridPos itself
        /// never mutates - moving always produces a new instance.
        /// </summary>
        public GridPos Offset(int dx, int dy) => new GridPos(X + dx, Y + dy);

        public bool Equals(GridPos other) => X == other.X && Y == other.Y;

        public override bool Equals(object obj) => obj is GridPos other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(X, Y);

        public static bool operator ==(GridPos left, GridPos right) => left.Equals(right);

        public static bool operator !=(GridPos left, GridPos right) => !left.Equals(right);

        public override string ToString() => $"({X}, {Y})";
    }
}
