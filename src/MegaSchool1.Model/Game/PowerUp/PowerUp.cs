using OneOf;
using OneOf.Types;

namespace MegaSchool1.Model.Game.PowerUp;

public abstract class PowerUp
{
    public abstract PowerUpResult Activate(GameState game);
}