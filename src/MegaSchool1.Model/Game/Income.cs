namespace MegaSchool1.Model.Game;

public record Income(decimal Gross, TimeSpan PayFrequency, string Description)
{
    public List<Theft> Thefts { get; } = new();

    public decimal Net(DayOfYear day)
    {
        return GetGross(day) - Thefts.Sum(theft => theft.StolenOn(this, day).Match(amount => amount, none => 0.0m));
    }

    public decimal GetGross(DayOfYear day) => GrossDuring(TimeSpan.FromDays(1));

    public decimal GrossDuring(TimeSpan duration) => (1.0m * Gross / PayFrequency.Days) * duration.Days;
}

public record W2Income : Income
{
    public W2Income(int gross, TimeSpan payFrequency) : base(gross, payFrequency, "Serf Duty")
    {
        Thefts.Add(new FederalIncomeTax());
        Thefts.Add(new IllinoisIncomeTax());
        Thefts.Add(new SocialSecurityTax());
        Thefts.Add(new MedicareTax());
    }
}
