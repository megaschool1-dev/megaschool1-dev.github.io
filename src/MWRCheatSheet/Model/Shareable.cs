namespace MWRCheatSheet.Model;

public record Content(string Message, string Tooltip, string Url);

public record Shareable(string Description, Content English, Content? Spanish, string ImageUrl, TimeSpan? Duration)
{
    public static string VideoShareable(string heading, string videoUrl, TimeSpan videoLength)
        => $"{heading}{Environment.NewLine}{Constants.PointingDownEmoji}{Environment.NewLine}({Util.MinuteEstimate(videoLength)}min){Environment.NewLine}{videoUrl}";
}

