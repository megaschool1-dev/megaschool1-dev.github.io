using MegaSchool1.Model.Game.Expense;
using OneOf;
using OneOf.Types;

namespace MegaSchool1.Model.Game.PowerUp;

public class HealthSharing : PowerUp
{
    private static readonly (decimal Amount, TimeSpan Epoch) PayRaise = (Random.Shared.Next(300, 600), GameState.BoardEpoch);

    public override (PowerUpResult Result, GameState game) Activate(GameState game)
    {
        return
        (
            (
            Description.From($"Healing boost! The {(game.Expenses.Any(e => e is TreasureMasterMembership) ? "Treasure Masters" : "Community Healing Shield")} will now pay for all healings."),
            new None()
            ),
            game
        );
    }
}