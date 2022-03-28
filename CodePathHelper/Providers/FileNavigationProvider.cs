namespace CodePathHelper.Providers
{
    using EnvDTE80;
    using System.IO;

    internal static  class FileNavigationProvider
    {
        internal static void GoToFileLine(in DTE2 dte, string filePath, int lineNumber, string branchName)
        {
            if (dte == null)
                return;

            string gitRootFolder = GitProvider.GetGitRootPath();

            if (Options.Instance.CheckoutBranchOption == CheckingoutBranchOption.DefaultBranch)
            {
                GitProvider.GitCheckout(Options.Instance.DefaultBranchName);
            }
            else if (Options.Instance.CheckoutBranchOption == CheckingoutBranchOption.UrlBranch)
            {
                GitProvider.GitCheckout(branchName);
            }

            dte.ExecuteCommand("File.OpenFile", Path.Combine(gitRootFolder, filePath));
            dte.ExecuteCommand("Edit.Goto", $"{lineNumber}");
        }
    }
}
