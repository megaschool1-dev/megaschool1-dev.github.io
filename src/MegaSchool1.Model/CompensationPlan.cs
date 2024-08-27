namespace MegaSchool1.Model;

public static class CompensationPlan
{
    public static readonly double BusinessSetupFee = 100;
    public static readonly double MembershipFee = 149.97;
    public static readonly double MembershipEnrollmentBonus = 50;

    public static readonly Dictionary<Rank, (int NumMemberships, int MonthlyPay, string Title)> DailyGuarantee = new()
    {
        { Rank.None, (0, 0, "N/A") },
        { Rank.ExecutiveDirector1, (3, 150, "Executive Director") },
        { Rank.ExecutiveDirector2, (12, 600, "2* Executive Director") },
        { Rank.ExecutiveDirector3, (50, 1500, "3* Executive Director") },
        { Rank.ExecutiveDirector4, (90, 3000, "4* Executive Director") },
        { Rank.ExecutiveDirector5, (180, 4500, "5* Executive Director") },
        { Rank.Regional1, (300, 6000, "Regional Director") },
        { Rank.Regional2, (480, 9000, "2* Regional Director") },
        { Rank.Regional3, (750, 12000, "3* Regional Director") },
        { Rank.Regional4, (1200, 19500, "4* Regional Director") },
        { Rank.Regional5, (1800, 30000, "5* Regional Director") },
        { Rank.NationalDirector, (3000, 45000, "National Director") },
        { Rank.VicePresidentialDirector, (6000, 90000, "Vice Presidential Director") },
        { Rank.PresidentialDirector, (12000, 150000, "Presidential Director") },
        { Rank.ExecutiveChairman, (21000, 300000, "Executive Chairman") },
        { Rank.NationalAmbassador, (33000, 450000, "National Ambassador") },
    };

    public static int GetNumMembershipsToFundMonthlyBill(int monthlyBillAmount)
    {
        var minimumRankToPayMonthlyBill = GetRankForMonthlyIncome(monthlyBillAmount);

        return minimumRankToPayMonthlyBill != Rank.None ? DailyGuarantee[minimumRankToPayMonthlyBill].NumMemberships : 0;
    }

    public static Rank GetRankForMonthlyIncome(int monthlyIncome)
        => DailyGuarantee.First(x => x.Value.MonthlyPay >= monthlyIncome).Key;
}

public enum Rank
{
    None = 0,
    ExecutiveDirector1 = 1,
    ExecutiveDirector2 = 2,
    ExecutiveDirector3 = 3,
    ExecutiveDirector4 = 4,
    ExecutiveDirector5 = 5,
    Regional1 = 6,
    Regional2 = 7,
    Regional3 = 8,
    Regional4 = 9,
    Regional5 = 10,
    NationalDirector = 11,
    VicePresidentialDirector = 12,
    PresidentialDirector = 13,
    ExecutiveChairman = 14,
    NationalAmbassador = 15,
}