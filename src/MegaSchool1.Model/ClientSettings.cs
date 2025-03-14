using System.Text.Json.Serialization;

namespace MegaSchool1.Model;

public class ClientSettings
{
    [JsonPropertyName("latestVersion")]
    public string LatestVersion { get; set; } = default!;

    public UISettings UI => Settings;

    public UISettings Settings { get; set; } = default!;
}