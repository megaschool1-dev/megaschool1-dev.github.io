using OneOf;

namespace MegaSchool1.Model.Game.Expense;

public interface IExpense
{
    public decimal Amount { get; }
    public OneOf<TimeSpan, OneTime> Recurrence { get; }
    public DayOfYear StartDate { get; }
    public string Description { get; }
}

public interface INegotiableExpense : IExpense;

public record Expense(decimal Amount, OneOf<TimeSpan, OneTime> Recurrence, DayOfYear StartDate, string Description) : IExpense
{
    public bool IsDueOn(DayOfYear day)
    {
        return Recurrence.Match(
             recurrence => Math.DivRem(day.DayNumber() - StartDate.DayNumber(), recurrence.Days).Remainder == 0,
             oneTime => day == StartDate);
    }

    public override string ToString() => Description;
}