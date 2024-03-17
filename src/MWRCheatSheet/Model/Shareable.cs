namespace MWRCheatSheet.Model;

public record Content(string Message, string Tooltip, string Url);

public record Shareable(string Description, Content English, Content? Spanish, string ImageUrl, TimeSpan? Duration);

