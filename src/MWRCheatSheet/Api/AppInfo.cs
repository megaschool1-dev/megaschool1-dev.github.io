using System.Text.Json.Serialization;

namespace MWRCheatSheet.Api;

public class AppInfo
{
    [JsonPropertyName("latest_version")]
    public string LatestVersion { get; set; } = default!;
}