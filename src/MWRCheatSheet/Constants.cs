namespace MWRCheatSheet;

public class Constants
{
    public static string BusinessEnrollmentUrl(string username) => $"https://user.mwrfinancial.com/{username}/join";
    public static string MembershipEnrollmentUrl(string username) => $"https://user.mwrfinancial.com/{username}/signup-financialedge";
    public static string InstantPayRaiseUrlEnglish(string username) => $"https://www.mwrfinancial.com/iprr/?member={username}";
    public static string InstantPayRaiseUrlSpanish(string username) => $"https://www.mwrfinancial.com/es/iprr/?member={username}";
    public static string MarketingDirectorUrlEnglish(string username) => $"https://www.mwrfinancial.com/?member={username}";
    public static string MarketingDirectorUrlSpanish(string username) => $"https://www.mwrfinancial.com/es/?member={username}";
    public static string JoinMakeWealthRealEnglish(string username) => $"https://www.mwrfinancial.com/join/?member={username}";
    public static string JoinMakeWealthRealSpanish(string username) => $"https://www.mwrfinancial.com/es/join/?member={username}";
}