using ValueOf;

namespace MegaSchool1.Model.Game.Expense;

public record Negotiation(INegotiableExpense Negotiated, DayOfYear StartDate) : Expense(GetDiscount(Negotiated) * 0.4m, Negotiated.Recurrence, StartDate, $"Bill Negotiators reduced your {Negotiated.Description} bill by {GetDiscount(Negotiated):C} and charged {GetDiscount(Negotiated) * 0.4m:C}, which is 40% of your savings")
{
    private class DiscountValue : ValueOf<int, DiscountValue>;

    private static readonly Dictionary<INegotiableExpense, DiscountValue> Discounts = new();

    private static decimal GetDiscount(INegotiableExpense expense)
    {
        if (!Discounts.ContainsKey(expense))
        {
            Discounts.Add(expense, DiscountValue.From(Random.Shared.Next(0, (int)(expense.Amount / 2))));
        }

        return Discounts[expense].Value;
    }

    public decimal Discount => Discounts[Negotiated].Value;
}