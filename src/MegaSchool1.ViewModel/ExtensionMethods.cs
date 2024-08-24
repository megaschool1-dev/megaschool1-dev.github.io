using MegaSchool1.Model;

namespace MegaSchool1.ViewModel;

public static class ExtensionMethods
{
    public static string MinimalistUrl(this Video video) => Constants.MinimalistVideoLink(video);
}
