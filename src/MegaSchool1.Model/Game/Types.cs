﻿using OneOf;
using OneOf.Types;
using ValueOf;

namespace MegaSchool1.Model.Game;

public class Description : ValueOf<string, Description>;
public class Savings : ValueOf<decimal, Savings>;

/// <summary>
///     Signifies a one time recurrence
/// </summary>
public class OneTime : ValueOf<object, OneTime>;

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
        var monthAndDay = Math.DivRem(currentDayNumber + numDays, Enum.GetValues<YearalMonth>().Length);

        return Math.DivRem(currentDayNumber + numDays, 365).Remainder == 0 
            ? new YearDay()
            : (Enum.GetValues<YearalMonth>()[monthAndDay.Quotient], monthAndDay.Remainder);
    }

    public static bool operator ==(DayOfYear first, DayOfYear second) => first.DayNumber() == second.DayNumber();

    public static bool operator !=(DayOfYear first, DayOfYear second) => !(first == second);
}
