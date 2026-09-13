using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeGame.Core
{
    /// <summary>
    /// Chooses a random grid cell for the next piece of food. Kept separate
    /// from SnakeGame so spawning rules can change independently of the
    /// main game loop.
    /// </summary>
    public class FoodSpawn
    {
        private readonly Random _random;

        public FoodSpawn(Random random = null)
        {
            // Allows a seeded Random to be injected from unit tests for
            // deterministic results; defaults to a real random sequence
            // during actual play.
            _random = random ?? new Random();
        }

        /// <summary>
        /// Returns a random position within a boardWidth x boardHeight grid
        /// that is not present in occupiedCells (i.e. not inside the snake).
        /// </summary>
        public GridPos SpawnNewPosition(int boardWidth, int boardHeight, IReadOnlyList<GridPos> occupiedCells)
        {
            var freeCells = new List<GridPos>();

            for (var x = 0; x < boardWidth; x++)
            {
                for (var y = 0; y < boardHeight; y++)
                {
                    var candidate = new GridPos(x, y);
                    if (!occupiedCells.Contains(candidate))
                    {
                        freeCells.Add(candidate);
                    }
                }
            }

            if (freeCells.Count == 0)
            {
                // Board is completely full - this generally means the
                // player has won. Caller decides how to handle it.
                throw new InvalidOperationException("No free cells available to spawn food.");
            }

            var index = _random.Next(freeCells.Count);
            return freeCells[index];
        }
    }
}
