using System.Text.Json.Serialization;

namespace MWRCheatSheet.Repository.Model;

public class Settings
{
    [JsonPropertyName("username")]
    public string Username { get; set; } = default!;

    [JsonPropertyName("fast_start_checklist")]
    public FastStartChecklist FastStartChecklist { get; set; } = new();
}