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
        var currentDayNumber = this.DayNumber();
        var (month, dayZeroIndexed) = Math.DivRem(currentDayNumber + numDays - 1, GameState.DaysInMonth);

        return Math.DivRem(currentDayNumber + numDays, 365).Remainder == 0 
            ? new YearDay()
            : (Enum.GetValues<YearalMonth>()[month], dayZeroIndexed + 1);
    }

    public static bool operator ==(DayOfYear first, DayOfYear second) => first.DayNumber() == second.DayNumber();

    public static bool operator !=(DayOfYear first, DayOfYear second) => !(first == second);
}
