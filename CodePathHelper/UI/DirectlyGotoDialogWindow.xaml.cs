namespace CodePathHelper
{
    using CodePathHelper.Providers;
    using Microsoft.VisualStudio.PlatformUI;
    using System.Windows;

    /// <summary>
    /// Interaction logic for InputUrlDialogWindow.xaml
    /// </summary>
    public partial class DirectlyGotoDialogWindow : DialogWindow
    {
        private readonly EnvDTE80.DTE2 _dte;
        private readonly string _url;
        private readonly string _filePath;
        private readonly int _lineNumber;

        public DirectlyGotoDialogWindow(EnvDTE80.DTE2 dte, string url, string filePath, int lineNumber)
        {
            InitializeComponent();
            _dte = dte;
            _url = url;
            _filePath = filePath;
            _lineNumber = lineNumber;

            this.urlTextBlock.Text = _url;
        }

        private void OnCancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OnGoto(object sender, RoutedEventArgs e)
        {
            // Close and goto
            this.Close();
            FileNavigationProvider.GoToFileLine(_dte, _filePath, _lineNumber, GitProvider.GitGetBranch());
        }
    }
}
