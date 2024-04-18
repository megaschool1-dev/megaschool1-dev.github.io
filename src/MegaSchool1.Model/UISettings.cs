using System.Text.Json.Serialization;

namespace MWRCheatSheet.Model;

public class UISettings
{
    [JsonPropertyName("localizations")]
    public I18N[] Localizations { get; set; } = default!;

    public I18N EnglishLocale => this.Localizations.First(l => l.Language == "en-US");
    public I18N SpanishLocale => this.Localizations.First(l => l.Language == "es-MX");
}