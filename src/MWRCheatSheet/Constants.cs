using MWRCheatSheet.Model;

namespace MWRCheatSheet;

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

public enum Highlight
{
    MoneyChallenge,
    ReduceMyTaxes,
    EliminateMyDebt,
    LowerMyBills,
    RestoreMyCredit,
    DCA,
    PRA,
    LandBanking,
    PreciousMetals,
    TrustMyAssets,
    KeysToHomeOwnership,
    Bonuses,
    Residuals,
    Couple,
    Military,
    GenYGenZ,
}

public enum Image
{
    None = 0,
    MoneyChallengeLogo = 1,
    MWRBanner = 2,
    HealthShare = 3,
    MembershipLogo = 4,
    CheatSheetPreview = 5,
    Overview1On1English = 6,
    RevenueShare1On1English = 7,
    Overview1On1Spanish = 8,
    RevenueShare1On1Spanish = 9,
    PreciousMetals = 10,
}

public enum Strategy
{
    Corporate = 0,
    ExtraDigitMovement = 1,
    MegaSchool = 2,
}

public class Constants
{
    public const string PointingDownEmoji = $"\ud83d\udc47";

    public static string BusinessEnrollmentUrl(string username) => $"https://user.mwrfinancial.com/{username}/join";
    public static string MembershipEnrollmentUrl(string username) => $"https://user.mwrfinancial.com/{username}/signup-financialedge";
    public static string InstantPayRaiseUrlEnglish(string username) => $"https://www.mwrfinancial.com/iprr/?member={username}";
    public static string InstantPayRaiseUrlSpanish(string username) => $"https://www.mwrfinancial.com/es/iprr/?member={username}";
    public static string MarketingDirectorUrlEnglish(string username) => $"https://www.mwrfinancial.com/?member={username}";
    public static string MarketingDirectorUrlSpanish(string username) => $"https://www.mwrfinancial.com/es/?member={username}";
    public static string JoinMakeWealthRealEnglish(string username) => $"https://www.mwrfinancial.com/join/?member={username}";
    public static string JoinMakeWealthRealSpanish(string username) => $"https://www.mwrfinancial.com/es/join/?member={username}";

    public static string GetImageUrl(Image image) => image switch
    {
        Image.MWRBanner => "/images/mwr-banner.png",
        Image.HealthShare => "/images/mwr-healthshare.png",
        Image.MembershipLogo => "/images/mwr-membership-logo.jpg",
        Image.MoneyChallengeLogo => "/images/72hour-money-challenge-logo.png",
        Image.CheatSheetPreview => "images/mwr-cheatsheet-preview.png",
        Image.Overview1On1English => "images/72-HourMoneyChallengeOverview_1on1_ENG.png",
        Image.RevenueShare1On1English => "images/72-HourMoneyChallengeRevenueSharing-1on1-ENG.png",
        Image.Overview1On1Spanish => "images/72-HourMoneyChallengeOverview_1on1_SPANISH.png",
        Image.RevenueShare1On1Spanish => "images/72-HourMoneyChallengeRevenueSharing-1on1-SPANISH.png",
        Image.PreciousMetals => "images/mwr-precious-metals.jpg",
        _ => throw new Exception($"Image not found: {image}"),
    };

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

    public static readonly Dictionary<Strategy, Shareable> MoneyChallenge = new()
    {
        {
            Strategy.Corporate,
            new ($"72-Hour Money Challenge",
                    new(Shareable.VideoShareable("72-Hour Money Challenge!", "https://www.ms1.megaschool.me/72hr-money-challenge", new(0, 0, 1, 30)), "English shareable copied!", "https://www.ms1.megaschool.me/72hr-money-challenge"),
                    new(Shareable.VideoShareable("72-Hour Money Challenge!", "https://www.ms1.megaschool.me/72hr-money-challenge", new(0, 0, 1, 30)), "Español shareable copied!", "https://www.ms1.megaschool.me/72hr-money-challenge"),
                    GetImageUrl(Image.MoneyChallengeLogo),
                    new(0, 1, 30))
        },
        {
            Strategy.ExtraDigitMovement,
            new ($"72-Hour Money Challenge Overview",
                    new(Shareable.VideoShareable("72-Hour Money Challenge!", "https://www.ms1.megaschool.me/72hr-overview", new(0, 0, 15, 0)), "Shareable copied!", "https://www.ms1.megaschool.me/72hr-overview"),
                    null,
                    GetImageUrl(Image.MoneyChallengeLogo),
                    new(0, 15, 0))
        },
        {
            Strategy.MegaSchool,
            new ($"72-Hour Money Challenge",
                    new($"{Shareable.VideoShareable("72-Hour Money Challenge!", "https://www.ms1.megaschool.me/72hr-money-challenge", new(0, 0, 1, 30))}{Environment.NewLine}{Environment.NewLine}{Shareable.VideoShareable("FAQ", "https://www.ms1.megaschool.me/72hr-faq", new(0, 0, 5, 0))}", "Shareable copied!", "https://www.ms1.megaschool.me/72hr-money-challenge"),
                    new($"{Shareable.VideoShareable("72-Hour Money Challenge!", "https://www.ms1.megaschool.me/72hr-money-challenge", new(0, 0, 1, 30))}{Environment.NewLine}{Environment.NewLine}{Shareable.VideoShareable("FAQ", "https://www.ms1.megaschool.me/72hr-faq", new(0, 0, 5, 0))}", "Shareable copied!", "https://www.ms1.megaschool.me/72hr-money-challenge"),
                    GetImageUrl(Image.MoneyChallengeLogo),
                    new(0, 7, 0))
        },
    };

    public static readonly Shareable CheatSheet = new(
        $"MWR Cheat Sheet App",
        new($"MWR Cheat Sheet App{Environment.NewLine}{PointingDownEmoji}{Environment.NewLine}https://makewealthreal.github.io/", "Shareable copied!", "https://www.ms1.megaschool.me/72hr-money-challenge"),
        null,
        "images/mwr-cheatsheet-preview.jpeg",
        null);

    public static readonly Shareable MoneyChallengeFAQ = new(
        "72-Hour Money Challenge FAQ",
        new(Shareable.VideoShareable("72-Hour Money Challenge - FAQ", "https://www.ms1.megaschool.me/72hr-faq", new(0, 0, 5, 0)), "Shareable copied!", "https://www.ms1.megaschool.me/72hr-faq"),
        null,
        GetImageUrl(Image.MoneyChallengeLogo),
        new(0, 0, 5, 0));
}