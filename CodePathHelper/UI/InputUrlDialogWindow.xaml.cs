namespace CodePathHelper
{
    using CodePathHelper.Providers;
    using Microsoft.VisualStudio.PlatformUI;
    using System.Windows;

    /// <summary>
    /// Interaction logic for InputUrlDialogWindow.xaml
    /// </summary>
    public partial class InputUrlDialogWindow : DialogWindow
    {
        private readonly EnvDTE80.DTE2 _dte;

        public InputUrlDialogWindow(EnvDTE80.DTE2 dte)
        {
            InitializeComponent();
            _dte = dte;
        }

        private void OnCancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OnGoto(object sender, RoutedEventArgs e)
        {
            string url = this.UrlTextBox.Text.Trim();
            if (string.IsNullOrEmpty(url))
            {
                _ = MessageProvider.ShowErrorInMessageBoxAsync("Url cannot be empty.");
                return;
            }

            // Extract
            bool isExtracted = AzureDevOpsCodePathProvider.ExtractInfoFromUrl(url, out string repoUrl, out string filePath, out string branchName, out int line, out int lineEnd, out int lineStartColumn, out int lineEndColumn);
            if (!isExtracted)
            {
                _ = MessageProvider.ShowErrorInMessageBoxAsync("Url is incorrect, please double check and retry.");
                this.UrlTextBox.Clear();
                return;
            }
            
            // Close and goto
            this.Close();
            FileNavigationProvider.GoToFileLine(_dte, filePath, branchName, line, lineEnd, lineStartColumn, lineEndColumn);
        }
    }
}
