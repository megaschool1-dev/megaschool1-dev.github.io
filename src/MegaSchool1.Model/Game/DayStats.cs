namespace MegaSchool1.Model.Game;

public class DayStats
{
    public DayStats(DayOfYear day)
    {
        this.Day = day;
    }

    public DayOfYear Day { get; }
    public decimal Income { get; set; }
}