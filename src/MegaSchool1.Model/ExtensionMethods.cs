using MegaSchool1.Model.Dto;

namespace MegaSchool1.Model;

public static class ExtensionMethods
{
    public static DayOfWeek DayOfWeek(this (YearalMonth Month, int DayOfMonth) day) => ((day.DayOfMonth - 1) % 7) switch
    {
        0 => System.DayOfWeek.Sunday,
        1 => System.DayOfWeek.Monday,
        2 => System.DayOfWeek.Tuesday,
        3 => System.DayOfWeek.Wednesday,
        4 => System.DayOfWeek.Thursday,
        5 => System.DayOfWeek.Friday,
        6 => System.DayOfWeek.Saturday
    };

    public static int CeilingInt(this double value) => (int)Math.Round(value, MidpointRounding.ToEven);

    public static ShareableDto? Content(this ShareableDto[] videos, Content content)
        => videos.FirstOrDefault(v => v.ContentId == content);

    public static string? MinimalistUrl(this ShareableDto video)
        => video.Platform switch
        {
            VideoPlatform.YouTube => Constants.MinimalistYouTubeLink(video.Id),
            VideoPlatform.Vimeo => $"{Constants.MinimalistVimeoLink(video.Id, video.Hash)}",
            VideoPlatform.TikTok => Constants.MinimalistTikTokLink(video.UserHandle!, video.Id),
            _ => null
        };

    public static string? ShareableUrl(this ShareableDto video) => video.MinimalistUrl() ?? video.Url;

    public static string EmbeddableUrl(this ShareableDto video)
        => video.Platform switch
        {
            VideoPlatform.YouTube => Constants.EmbeddableYouTubeLink(video.Id),
            VideoPlatform.Vimeo => $"{Constants.EmbeddableVimeoLink(video.Id)}{(string.IsNullOrWhiteSpace(video.Hash) ? string.Empty : $"&h={video.Hash}")}",
            _ => throw new Exception("No video set!")
        };
}
