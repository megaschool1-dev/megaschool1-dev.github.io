using MegaSchool1.Model.Game.Expense;
using OneOf.Types;

namespace MegaSchool1.Model.Game.PowerUp;

public class BillNegotiator : PowerUp
{
    private readonly List<Negotiation> _negotiations = [];

    public override (PowerUpResult Result, GameState game) Activate(GameState game)
    {
        var billNegotiatorExpense = new BillNegotiatorExpense(game.Day);

        if (!game.Expenses.Any(e => e is TreasureMasterMembership))
        {
            game = game with { CheckingAccountBalance = game.CheckingAccountBalance - billNegotiatorExpense.Amount }; 

            game.Expenses.Add(billNegotiatorExpense);
        }

        var billNegotiatorCost = game.Expenses.Any(e => e is TreasureMasterMembership) ? 0.0m : billNegotiatorExpense.Amount;

        game.Expenses.RemoveAll(e => e is Negotiation);

        _negotiations.Clear();
        _negotiations.AddRange(game.Expenses.OfType<INegotiableExpense>().Select(e => new Negotiation(e, GetBillNegotiationDay(game.Day))));
        game.Expenses.AddRange(_negotiations);

        var savings = Savings.From(_negotiations.Sum(n => n.Discount) - billNegotiatorCost);

        return 
        (
            (
            Description.From(_negotiations.Any(n => n.Discount > 0.0m)
                ? "Reduced " + _negotiations.Where(n => n.Discount > 0.0m).Select(e => $"{e.Description} by {e.Discount:C}").Aggregate((accumulated, next) => $"{accumulated}, {next}")
                : "No bills have been successfully"),
            savings.Value > 0 ? savings : new None()),
            game
        );
    }

    private static DayOfYear GetBillNegotiationDay(DayOfYear today)
        => today.AddDays(Random.Shared.Next(0, GameState.DaysInMonth - 1));
}