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
}