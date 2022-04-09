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
        private readonly string _branchName;
        private readonly int _line;
        private readonly int _lineEnd;
        private readonly int _lineStartColumn;
        private readonly int _lineEndColumn;

        public DirectlyGotoDialogWindow(EnvDTE80.DTE2 dte, string url, string filePath, string branchName, int line, int lineEnd, int lineStartColumn, int lineEndColumn)
        {
            InitializeComponent();
            _dte = dte;
            _url = url;
            _filePath = filePath;
            _branchName = branchName;
            _line = line;
            _lineEnd = lineEnd;
            _lineStartColumn = lineStartColumn;
            _lineEndColumn = lineEndColumn;

            this.urlTextBlock.Text = _url;
        }

        private void OnCancel(object sender, RoutedEventArgs e)
        {
            this.Close();

            InputUrlDialogWindow inputUrlDialogWindow = new InputUrlDialogWindow(_dte)
            {
                HasMaximizeButton = false,
                HasMinimizeButton = false
            };
            inputUrlDialogWindow.ShowModal();
        }

        private void OnGoto(object sender, RoutedEventArgs e)
        {
            // Close and goto
            this.Close();
            FileNavigationProvider.GoToFileLine(_dte, _filePath, _branchName, _line, _lineEnd, _lineStartColumn, _lineEndColumn);
        }
    }
}
