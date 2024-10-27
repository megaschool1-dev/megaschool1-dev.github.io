using OneOf;
using OneOf.Types;
using ValueOf;

namespace MegaSchool1.Model.Game;

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
        var dayOfYear = targetDayNumber % 365;
        (int YearNumber, int DayOfYear) target = Math.DivRem(targetDayNumber, 365);

        if(target.DayOfYear == 0)
        {
            return new YearDay();
        }
        else
        {
            var (monthZeroIndexed, dayZeroIndexed) = Math.DivRem(target.DayOfYear, GameState.DaysInMonth);
            // return (Enum.GetValues<YearalMonth>()[((targetDayNumber - 1) / GameState.DaysInMonth) % GameState.Months.Length], ((targetDayNumber - 1) % GameState.DaysInMonth) + 1);
            var targetDayOfMonth = Math.DivRem(target.DayOfYear, GameState.DaysInMonth);
            return (Enum.GetValues<YearalMonth>()[((targetDayNumber - 1) / GameState.DaysInMonth) % GameState.Months.Length], targetDayOfMonth.Remainder != 0 ? targetDayOfMonth.Remainder : 28);
        }

        //var dayOfYearZeroIndexed = Math.DivRem(currentDayNumber, 365).Remainder;
        //var (monthZeroIndexed, dayZeroIndexed) = Math.DivRem((dayOfYearZeroIndexed + 1) + numDays, GameState.DaysInMonth);

        //return Math.DivRem(currentDayNumber + numDays, 365).Remainder == 0 
        //    ? new YearDay()
        //    : (Enum.GetValues<YearalMonth>()[monthZeroIndexed], dayZeroIndexed + 1);
    }

    public override string ToString() => this.Match(nonYearDay => $"{nonYearDay.Month} {nonYearDay.DayOfMonth}", yearDay => "Year Day");

    public static bool operator ==(DayOfYear first, DayOfYear second) => first.DayNumber() == second.DayNumber();

    public static bool operator !=(DayOfYear first, DayOfYear second) => !(first == second);
}
