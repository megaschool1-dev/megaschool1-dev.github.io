using MegaSchool1.Model.Game.Expense;
using OneOf;
using OneOf.Types;

namespace MegaSchool1.Model.Game.PowerUp;

public class InstantPayRaise : PowerUp
{
    private static readonly (decimal Amount, TimeSpan Epoch) PayRaise = (Random.Shared.Next(300, 600), GameState.BoardEpoch);

    public override (PowerUpResult Result, GameState game) Activate(GameState game)
    {
        var totalPayRaiseDuringBoardEpoch = PayRaise.Amount / PayRaise.Epoch.Days * GameState.BoardEpoch.Days;
        var dailyPayRaise = totalPayRaiseDuringBoardEpoch / GameState.BoardEpoch.Days;

        if (game.Incomes.SelectMany(i => i.Thefts).Any(theft => theft is FederalIncomeTax))
        {
            game = game with { CheckingAccountBalance = game.CheckingAccountBalance + dailyPayRaise };
            game.CurrentDayStats.Income = (game.CurrentDayStats.Income.Gross, game.CurrentDayStats.Income.Net + dailyPayRaise);
        }

        if(!game.Expenses.Any(e => e is TreasureMasterMembership))
        {
            var instantPayRaiseExpense = new InstantPayRaiseExpense(game.Day);

            game = game with { CheckingAccountBalance = game.CheckingAccountBalance - instantPayRaiseExpense.Amount };
            game.Expenses.Add(instantPayRaiseExpense);
        }

        return
        (
             (
            Description.From($"Instant Pay Raise! The {(game.Expenses.Any(e => e is TreasureMasterMembership) ? "Treasure Masters" : "Levy Wizard")} recovered ${dailyPayRaise:N0} from the Big Realm Levy theft."),
            new None())
            ,
            game
        );
    }
}