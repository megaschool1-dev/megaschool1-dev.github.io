using OneOf;

namespace Flow.Model.Expense;

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
             recurrence => Math.DivRem((int)(day.DayNumber() - StartDate.DayNumber()), (int)recurrence.Days).Remainder == 0,
             oneTime => day == StartDate);
    }

    public override string ToString() => Description;
}