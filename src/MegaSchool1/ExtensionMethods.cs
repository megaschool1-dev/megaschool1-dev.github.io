using MudBlazor;

namespace MegaSchool1;

public static class ExtensionMethods
{
    private static readonly Task InfiniteWait = Task.Delay(-1);

    public static async Task ShowModalAsync(this MudDialog dialog, TimeSpan? autoClose = null)
    {
        var displayedDialog = await dialog.ShowAsync();

        var autoCloseDelay = autoClose != null ? Task.Delay(autoClose.Value) : InfiniteWait;

        await Task.WhenAny(displayedDialog.Result, autoCloseDelay);

        displayedDialog.Close();
    }
}