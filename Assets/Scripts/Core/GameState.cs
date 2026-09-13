namespace SnakeGame.Core
{
    /// <summary>
    /// Drives the State pattern in SnakeGame.Tick() - behaviour branches
    /// on which state the game is currently in, rather than scattering
    /// boolean flags (isGameOver, isPaused, ...) throughout the class.
    /// </summary>
    public enum GameState
    {
        Playing,
        Paused,
        GameOver
    }
}
