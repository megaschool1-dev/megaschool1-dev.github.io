using System.Text.Json.Serialization;

namespace Shareable.Api;

public class AppInfo
{
    [JsonPropertyName("latest_version")]
    public string LatestVersion { get; set; } = default!;
}