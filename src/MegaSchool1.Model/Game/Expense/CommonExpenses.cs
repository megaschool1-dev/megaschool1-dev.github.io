namespace MegaSchool1.Model.Game.Expense;

public record TreasureMasterMembership(DayOfYear MembershipStartDate) : Expense(149.97m, GameState.BoardEpoch, MembershipStartDate, "Treasure Masters Guild Fee");
public record InstantPayRaiseExpense(DayOfYear SummonDate) : Expense(200, new OneTime(), SummonDate, "Levy Wizard Fee");
public record BillNegotiatorExpense(DayOfYear SummonDate) : Expense(250, new OneTime(), SummonDate, "Merchant Inducer Fee");
public record HealthSharingContribution(DayOfYear SummonDate) : Expense(300, GameState.BoardEpoch, SummonDate, "Community Healing Shield Contribution");

public record CellPhone() : Expense(40, GameState.BoardEpoch, new((YearalMonth.January, Random.Shared.Next(1, GameState.BoardEpoch.Days))), "Cell Bill"), INegotiableExpense;
