using System.Text.Json.Serialization;

namespace MWRCheatSheet.Model;

public class I18N
{
    [JsonPropertyName("language")]
    public string Language { get; set; } = default!;

    [JsonPropertyName("videos")]
    public VideoResource[] Videos { get; set; } = [];

    [JsonPropertyName("testimonials")]
    public Testimonial[] Testimonials { get; set; } = [];

    [JsonPropertyName("events")]
    public Event[] Events { get; set; } = [];

    public VideoResource? this[Content content]
    {
        get
        {
            return this.Videos.Content(content);
        }
    }
}