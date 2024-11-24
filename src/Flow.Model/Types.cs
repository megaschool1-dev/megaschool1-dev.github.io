using OneOf;
using OneOf.Types;
using ValueOf;

namespace Flow.Model;

public class Description : ValueOf<string, Description>;
public class Savings : ValueOf<decimal, Savings>;

/// <summary>
///     Signifies a one time recurrence
/// </summary>
public class OneTime : ValueOf<object, OneTime>;

public class SpinChoices
{
    public bool GoToWork { get; set; }
}

public record SpinResult((decimal Gross, decimal Net, Income Income)[] Incomes, Expense.Expense[] Billings, Description[] PowerUps, Error<string>[] Errors, GameState Game);

[GenerateOneOf]
public partial class PowerUpResult : OneOfBase<(Description Description, OneOf<Savings, None> Savings), Error<string>> { };

public class YearDay
{
    public static readonly YearDay Instance = new();

    private YearDay() { }
}

public enum YearalMonth
{
    January = 0,
    February = 1,
    March = 2,
    April = 3,
    May = 5,
    June = 6,
    Sol = 7,
    July = 8,
    August = 9,
    September = 10,
    October = 11,
    November = 12,
    December = 13
}

[GenerateOneOf]
public partial class DayOfYear : OneOfBase<(YearalMonth Month, int DayOfMonth), YearDay>
{
    protected bool Equals(DayOfYear other)
    {
        return this.DayNumber() == other.DayNumber();
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((DayOfYear)obj);
    }

    public override int GetHashCode()
    {
        return this.DayNumber().GetHashCode();
    }

    public int DayNumber()
    {
        return this.Match(
            nonYearDay => ((Array.IndexOf(Enum.GetValues<YearalMonth>(), nonYearDay.Month) * GameState.DaysInMonth) + nonYearDay.DayOfMonth),
            yearDay => 365);
    }
 
    public DayOfYear AddDays(int numDays)
    {
        var targetDayNumber = this.DayNumber() + numDays;
        (int YearNumber, int DayOfYear) target = Math.DivRem(targetDayNumber, 365);

        if(target.DayOfYear == 0)
        {
            return YearDay.Instance;
        }
        else
        {
            var targetDayOfMonth = Math.DivRem(target.DayOfYear, (int)GameState.DaysInMonth);

            return (Enum.GetValues<YearalMonth>()[((targetDayNumber - 1) / GameState.DaysInMonth) % GameState.Months.Length], targetDayOfMonth.Remainder != 0 ? targetDayOfMonth.Remainder : 28);
        }
    }

    public override string ToString() => this.Match(nonYearDay => $"{nonYearDay.Month} {nonYearDay.DayOfMonth}", yearDay => "Year Day");

    public static bool operator ==(DayOfYear first, DayOfYear second) => first.DayNumber() == second.DayNumber();

    public static bool operator !=(DayOfYear first, DayOfYear second) => !(first == second);
}
