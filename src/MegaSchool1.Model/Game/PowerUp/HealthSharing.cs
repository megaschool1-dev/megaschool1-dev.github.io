using MegaSchool1.Model.Game.Expense;
using OneOf;
using OneOf.Types;

namespace MegaSchool1.Model.Game.PowerUp;

public class HealthSharing : PowerUp
{
    private static readonly (decimal Amount, TimeSpan Epoch) PayRaise = (Random.Shared.Next(300, 600), GameState.BoardEpoch);

    public override PowerUpResult Activate(GameState game)
    {
        var totalPayRaiseDuringBoardEpoch = PayRaise.Amount / PayRaise.Epoch.Days * GameState.BoardEpoch.Days;
        var dailyPayRaise = totalPayRaiseDuringBoardEpoch / GameState.BoardEpoch.Days;

        if (game.Incomes.SelectMany(i => i.Thefts).Any(theft => theft is FederalIncomeTax))
        {
            game.CheckingAccountBalance += dailyPayRaise;
            game.Days[game.DayOfYear.AsT0.DayOfMonth - 1].Income += dailyPayRaise;
        }

        if(!game.Expenses.Any(e => e is TreasureMasterMembership))
        {
            var instantPayRaiseExpense = new InstantPayRaiseExpense(game.DayOfYear);

            game.CheckingAccountBalance -= instantPayRaiseExpense.Amount;

            game.Expenses.Add(instantPayRaiseExpense);
        }

        return (
            Description.From($"Healing boost! The {(game.Expenses.Any(e => e is TreasureMasterMembership) ? "Treasure Masters" : "Community Healing Shield")} will now pay for all healings."),
            new None()
            );
    }
}