namespace MegaSchool1.Model.Game;

public record Income(decimal Gross, TimeSpan PayFrequency)
{
    public List<Theft> Thefts { get; } = new();

    public decimal Net(DayOfYear day)
    {
        return Gross - Thefts.Sum(theft => theft.StolenOn(this, day));
    }

    public decimal GetGrossDuring(TimeSpan duration) => (1.0m * Gross / PayFrequency.Days) * duration.Days;
}

public record W2Income : Income
{
    public W2Income(int gross, TimeSpan payFrequency) : base(gross, payFrequency)
    {
        Thefts.Add(new FederalIncomeTax());
        Thefts.Add(new IllinoisIncomeTax());
        Thefts.Add(new SocialSecurityTax());
        Thefts.Add(new MedicareTax());
    }
}
