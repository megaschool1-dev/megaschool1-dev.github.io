using MegaSchool1.Model.Game.Expense;
using OneOf;
using OneOf.Types;

namespace MegaSchool1.Model.Game;

public class GameState
{
    public static readonly int DaysInMonth = 28;
    public static readonly YearalMonth[] Months = Enum.GetValues<YearalMonth>();
    public static readonly TimeSpan BoardEpoch = TimeSpan.FromDays(DaysInMonth);

    public GameState()
    {
        Days = Enumerable.Range(1, BoardEpoch.Days).Select(i => new DayStats((YearalMonth.January, i))).ToArray();
    }

    public int SuccessiveWorkDays { get; set; }
    public decimal CheckingAccountBalance { get; set; }
    public decimal SavingsAccountBalance { get; set; }
    public int SickDayLikelihood { get; set; }
    public DayOfYear DayOfYear { get; set; } = (YearalMonth.January, 1);
    public DayStats[] Days { get; }
    public List<Income> Incomes { get; } = [];
    public List<Expense.Expense> Expenses { get; } = [];
    public List<Loan> Debts { get; } = [];
    public List<PowerUp.PowerUp> PowerUps { get; } = [];
    public OneOf<TimeSpan, None> DialogAutoClose { get; set; } = new None();
    public DayStats CurrentDayStats => Days[DayOfYear.DayNumber() - 1];

    public static GameState Moderate()
    {
        const int AverageAmericanAnnualSalary = 60 * 1000;

        var game = new GameState();

        var payFrequency = TimeSpan.FromDays(1);
        var primaryIncome = new W2Income(AverageAmericanAnnualSalary / (int)(TimeSpan.FromDays(365) / payFrequency), payFrequency);
        game.Incomes.Add(primaryIncome);

        // 2 weeks of salary ready to spend
        game.CheckingAccountBalance = primaryIncome.GetGrossDuring(TimeSpan.FromDays(14));

        // 3 month emergency fund
        game.SavingsAccountBalance = primaryIncome.GetGrossDuring(3 * BoardEpoch);

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
