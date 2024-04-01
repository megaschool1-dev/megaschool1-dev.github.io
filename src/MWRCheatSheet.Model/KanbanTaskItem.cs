using System.Text.Json.Serialization;

namespace MWRCheatSheet.Model;

public class KanbanTaskItem
{
    [JsonPropertyName("name")]
    public string Name { get; init; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("order")]
    public int Order { get; set; }

    public KanbanTaskItem(string name, string status)
    {
        Name = name;
        Status = status;
    }
}
