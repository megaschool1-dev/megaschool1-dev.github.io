namespace Flow.Model.PowerUp;

public abstract class PowerUp
{
    public abstract (PowerUpResult Result, GameState game) Activate(GameState game);
}