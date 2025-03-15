using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;

namespace MegaSchool1.Model;

public class ClientSettings
{
    [JsonPropertyName("latestVersion")]
    [ConfigurationKeyName("latestVersion")]
    public string LatestVersion { get; set; } = default!;

    [JsonPropertyName("Settings")]
    [ConfigurationKeyName("Settings")]
    public UISettings UI { get; set; } = default!;
}