namespace CodePathHelper.Providers
{
    using EnvDTE80;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;
    using Microsoft.VisualStudio.TextManager.Interop;
    using System.IO;

    internal static  class FileNavigationProvider
    {
        internal static void GoToFileLine(in DTE2 dte, string filePath, string branchName, int line, int lineEnd, int lineStartColumn, int lineEndColumn)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (dte == null)
                return;

            string solutionDir = Path.GetDirectoryName(dte.Solution.FullName);

            string gitRootFolder = GitProvider.GetGitRootPath(solutionDir);

            if (Options.Instance.CheckoutBranchOption == CheckingoutBranchOption.DefaultBranch)
            {
                GitProvider.GitCheckout(Options.Instance.DefaultBranchName);
            }
            else if (Options.Instance.CheckoutBranchOption == CheckingoutBranchOption.UrlBranch)
            {
                GitProvider.GitCheckout(branchName);
            }

            Microsoft.VisualStudio.OLE.Interop.IServiceProvider sp = (Microsoft.VisualStudio.OLE.Interop.IServiceProvider)dte;
            ServiceProvider serviceProvider = new ServiceProvider(sp);
            VsShellUtilities.OpenDocument(serviceProvider, Path.Combine(gitRootFolder, filePath), VSConstants.LOGVIEWID_Primary, out IVsUIHierarchy hierarchy, out uint itemId, out IVsWindowFrame frame, out IVsTextView textView);
            textView.SetCaretPos(line, lineStartColumn);
            textView.EnsureSpanVisible(
                    new TextSpan
                    {
                        iStartIndex = lineStartColumn - 1,
                        iStartLine = line - 1,
                        iEndIndex = lineEndColumn - 1,
                        iEndLine = lineEnd - 1
                    });

            if (Options.Instance.SelectCodeSnippet)
            {
                textView.SetSelection(line - 1, lineStartColumn - 1, lineEnd - 1, lineEndColumn - 1);
            }
        }
    }
}
