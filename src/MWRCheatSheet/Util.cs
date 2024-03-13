using MWRCheatSheet.Model;
using static System.Net.WebRequestMethods;

namespace MWRCheatSheet;

public class Util
{
    public static async Task<bool> IsUsernameValidAsync(string username, HttpClient http)
    {
        // check if username belongs to an active Qualified Marketing Director (QMD)
        var userBusinessEnrollmentPage = await GetAsync(Constants.BusinessEnrollmentUrl(username), http);

        return userBusinessEnrollmentPage?.IsSuccessStatusCode is true;
    }

    private static async Task<HttpResponseMessage?> GetAsync(string url, HttpClient http)
    {
        HttpResponseMessage? response = null;

        try
        {
            response = await http.GetAsync(url);
        }
        catch (Exception) { }

        return response;
    }

    public static int GetMonthlyIncome(int teamMembers)
    {
        return Constants.DailyGuarantee.Reverse().First(x => x.Value.NumMemberships <= teamMembers).Value.MonthlyPay;
    }

    public static TeamLevel[] GetTeamLevels(int monthlyIncomeGoal, TeamLevel[] distribution)
    {
        if (monthlyIncomeGoal == 0)
        {
            return Array.Empty<TeamLevel>();
        }
        else
        {
            var numMembershipToMeetMonthlyIncomeGoal = GetNumMembershipsToFundMonthlyBill(monthlyIncomeGoal);
            List<TeamLevel> teamLevels;

            var minimumRequiredLevel = distribution.FirstOrDefault(teamLevel => teamLevel.TeamMembersTotal >= numMembershipToMeetMonthlyIncomeGoal);
            if (minimumRequiredLevel == null)
            {
                teamLevels = [new("N/A", 0, null)];
            }
            else
            {
                teamLevels = distribution.Where(teamLevel => teamLevel.TeamMembersTotal <= minimumRequiredLevel.TeamMembersTotal).ToList();
            }

            return teamLevels.OrderBy(x => teamLevels.IndexOf(x)).ToArray();
        }
    }

    public static int GetNumMembershipsToFundMonthlyBill(int monthlyBillAmount)
    {
        var minimumRankToPayMonthlyBill = GetRankForMonthlyIncome(monthlyBillAmount);

        return minimumRankToPayMonthlyBill != Rank.None ? Constants.DailyGuarantee[minimumRankToPayMonthlyBill].NumMemberships : 0;
    }

    public static Rank GetRankForMonthlyIncome(int monthlyIncome)
        => Constants.DailyGuarantee.First(x => x.Value.MonthlyPay >= monthlyIncome).Key;
}