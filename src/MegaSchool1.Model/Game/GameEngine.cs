using MegaSchool1.Model.Game.Expense;
using MegaSchool1.Model.Game.PowerUp;
using OneOf;
using OneOf.Types;
using ValueOf;

namespace MegaSchool1.Model.Game;

public static class GameEngine
{
    public static OneOf<Description[], Error<string>> ProcessPowerUpsForCurrentDay(GameState game)
    {
        var processedPowerUps = new List<Description>();

        foreach (var powerUp in game.PowerUps.Where( p => p is not BillNegotiator))
        {
            if (powerUp.Activate(game).TryPickT0(out var description, out var error))
            {
                processedPowerUps.Add(description.Description);
            }
            else
            {
                return error;
            }
        }

        return processedPowerUps.ToArray();
    }

    public static Expense.Expense[] ProcessExpensesForCurrentDay(GameState game)
    {
        var processedExpenses = new List<Expense.Expense>();

        foreach (var expense in game.Expenses.Where(e => e.IsDueOn(game.DayOfYear)))
        {
            game.CheckingAccountBalance -= expense.Amount;

            processedExpenses.Add(expense);
        }

        return processedExpenses.ToArray();
    }

    public static void IncreaseDailyJobIncome(GameState game)
    {
        game.SuccessiveWorkDays += 1;

        var dailyIncome = game.Incomes
            .Where(i => i.PayFrequency == TimeSpan.FromDays(1))
            .Sum(i => i.Net(game.DayOfYear));

        game.Days[game.DayOfYear.AsT0.DayOfMonth - 1].Income += dailyIncome;
        game.CheckingAccountBalance += dailyIncome;
    }

    public static OneOf<PowerUpResult, None> InstantPayRaise(GameState game)
    {
        if (!game.PowerUps.Any(p => p is InstantPayRaise))
        {
            var instantPayRaise = new InstantPayRaise();

            game.PowerUps.Add(instantPayRaise);

            instantPayRaise.Activate(game);
        }

        return new None();
    }

    public static OneOf<PowerUpResult, None> SummonBillNegotiator(GameState game)
    {
        if (!game.PowerUps.Any(p => p is BillNegotiator))
        {
            var negotiator = new BillNegotiator();

            var powerUpResult = negotiator.Activate(game);

            game.PowerUps.Add(negotiator);

            return powerUpResult;
        }

        return new None();
    }

    public static OneOf<MonthlySavings, None> SummonTreasureMasters(GameState game)
    {
        var treasureMastersMembership = new TreasureMasterMembership(game.DayOfYear);
        game.Expenses.Add(treasureMastersMembership);

        var checkingBeforeMembership = game.CheckingAccountBalance;

        game.CheckingAccountBalance -= treasureMastersMembership.Amount;

        var monthlySavings = 0.0m;

        // instant pay raise
        var instantPayRaise = InstantPayRaise(game);
        monthlySavings += instantPayRaise.Match(
            powerUpResult =>
                powerUpResult.Match(
                    result => result.Savings.Match(
                        savings => savings.Value,
                        none => 0.0m),
                    error => 0.0m),
            none => 0.0m);

        // bill negotiator
        var billNegotiator = SummonBillNegotiator(game);
        monthlySavings += billNegotiator.Match(
            powerUpResult =>
                powerUpResult.Match(
                    result => result.Savings.Match(
                        savings => savings.Value,
                        none => 0.0m),
                    error => 0.0m),
            none => 0.0m);
    
        // health sharing
        var healthSharing = SummonHealthSharing(game);
        monthlySavings += healthSharing.Match(
            powerUpResult =>
                powerUpResult.Match(
                    result => result.Savings.Match(
                        savings => savings.Value,
                        none => 0.0m),
                    error => 0.0m),
            none => 0.0m);

        if (monthlySavings > treasureMastersMembership.Amount)
        {
            return MonthlySavings.From(monthlySavings - treasureMastersMembership.Amount);
        }
        else
        {
            game.CheckingAccountBalance += treasureMastersMembership.Amount;
            return new None();
        }
    }

    public static OneOf<PowerUpResult, None> SummonHealthSharing(GameState game)
    {
        if (!game.Expenses.Any(e => e is HealthSharingContribution or TreasureMasterMembership))
        {
            var healthSharingExpense = new HealthSharingContribution(game.DayOfYear);

            game.Expenses.Add(healthSharingExpense);

            game.CheckingAccountBalance -= healthSharingExpense.Amount;
        }

        return new None();
    }

    public class MonthlySavings : ValueOf<decimal, MonthlySavings>;
}