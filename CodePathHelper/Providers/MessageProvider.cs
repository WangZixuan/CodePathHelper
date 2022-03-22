namespace CodePathHelper.Providers
{
    using System.Threading.Tasks;
    using Community.VisualStudio.Toolkit;

    internal static class MessageProvider
    {
        internal static async Task ShowInStatusBarAsync(string message)
        {
            await VS.StatusBar.ShowMessageAsync(message);
            await Task.Delay(3000);
            await VS.StatusBar.ClearAsync();
        }

        internal static async Task ShowErrorInMessageBoxAsync(string message)
        {
            await VS.MessageBox.ShowAsync(Vsix.Name, message);
        }

        internal static async Task ShowInfoInMessageBoxAsync(string message)
        {
            await VS.MessageBox.ShowErrorAsync(Vsix.Name, message);
        }
    }
}
