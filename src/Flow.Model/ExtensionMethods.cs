namespace Flow.Model;

public static class ExtensionMethods
{
    public static DayOfWeek DayOfWeek(this (YearalMonth Month, int DayOfMonth) day) => ((day.DayOfMonth - 1) % 7) switch
    {
        0 => System.DayOfWeek.Sunday,
        1 => System.DayOfWeek.Monday,
        2 => System.DayOfWeek.Tuesday,
        3 => System.DayOfWeek.Wednesday,
        4 => System.DayOfWeek.Thursday,
        5 => System.DayOfWeek.Friday,
        6 => System.DayOfWeek.Saturday
    };
}
