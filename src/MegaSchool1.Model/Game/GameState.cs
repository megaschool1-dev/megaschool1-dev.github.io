﻿using System.Collections.ObjectModel;
using MegaSchool1.Model.Game.Expense;
using OneOf;
using OneOf.Types;

namespace MegaSchool1.Model.Game;

public record GameState(
    decimal CheckingAccountBalance,
    decimal SavingsAccountBalance,
    int SuccessiveWorkDays,
    Percentage SickDayLikelihood,
    DayOfYear Day,
    OneOf<TimeSpan, None> DialogAutoClose)
{
    public static readonly int DaysInMonth = 28;
    public static readonly YearalMonth[] Months = Enum.GetValues<YearalMonth>();
    public static readonly TimeSpan BoardEpoch = TimeSpan.FromDays(DaysInMonth);

    private readonly List<DayStats> _days = Enumerable.Range(1, BoardEpoch.Days).Select(i => new DayStats((YearalMonth.January, i))).ToList();

    public ReadOnlyCollection<DayStats> Days => _days.AsReadOnly();

    private Dictionary<DayOfYear, (PowerUp.PowerUp PowerUp, decimal Savings)> _savings = [];
    public ReadOnlyDictionary<DayOfYear, (PowerUp.PowerUp PowerUp, decimal Savings)> Savings => _savings.AsReadOnly();

    public List<Income> Incomes { get; } = [];
    public List<Expense.Expense> Expenses { get; } = [];
    public List<Loan> Debts { get; } = [];
    public List<PowerUp.PowerUp> PowerUps { get; } = [];
    public DayStats CurrentDayStats => GetDayStats(this.Day);

    public DayStats GetDayStats(DayOfYear day) => Days[day.DayNumber() - 1];

    public GameState GoToNextDay()
    {
        var lastDay = _days.Last().Day.DayNumber();
        var nextDay = Day.AddDays(1);

        if(nextDay.DayNumber() > lastDay)
        {
            var day = new DayStats(nextDay);

            _days.Add(day);
        }

        return this with { Day = nextDay };
    }

    public static GameState Moderate()
    {
        const int AverageAmericanAnnualSalary = 60 * 1000;

        var payFrequency = TimeSpan.FromDays(1);
        var primaryIncome = new W2Income(AverageAmericanAnnualSalary / (int)(TimeSpan.FromDays(365) / payFrequency), payFrequency);

        var game = new GameState(
            // 2 weeks of salary ready to spend
            primaryIncome.GrossDuring(TimeSpan.FromDays(14)),

             // 3 month emergency fund
            primaryIncome.GrossDuring(3 * BoardEpoch),
            0,
            Percentage.From(0),
            (YearalMonth.January, 1),
            new None()
        );

        game.Incomes.Add(primaryIncome);

        // expenses
        game.Expenses.Add(new(1000, BoardEpoch, new ((YearalMonth.January, Random.Shared.Next(1, BoardEpoch.Days))), "Rent"));
        game.Expenses.Add(new(200, BoardEpoch, new((YearalMonth.January, Random.Shared.Next(1, BoardEpoch.Days))), "Health Insurance"));
        game.Expenses.Add(new CellPhone());

        // debts
        {
            // credit card
            var balance = 2000.0m;
            var apy = Percentage.From(0.22m);
            game.Debts.Add(new("Credit Card", balance, apy,
                balance * apy.Value / Enum.GetValues<YearalMonth>().Length));
        }

        {
            // student loan
            var balance = 10000.0m;
            var apy = Percentage.From(0.055m);
            game.Debts.Add(new("Student Loan", balance, apy, balance * apy.Value / Enum.GetValues<YearalMonth>().Length));
        }

        return game;
    }
}
