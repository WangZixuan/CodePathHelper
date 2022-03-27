namespace CodePathHelper.Providers
{
    public static class AzureDevOpsCodePathProvider
    {
        public static bool ExtractInfoFromUrl(in string url, out string repoUrl, out string filePath, out string branchName, out int lineNumber)
        {
            repoUrl = string.Empty;
            filePath = string.Empty;
            branchName = string.Empty;
            lineNumber = -1;

            if (string.IsNullOrEmpty(url))
                return false;

            var urlParts = url.Trim().Split('&');

            string repoUrlPart = urlParts[0];

            int questionMarkIndex = url.IndexOf('?');
            if (questionMarkIndex == -1)
                return false;
            repoUrl = repoUrlPart.Substring(0, questionMarkIndex);
            filePath = repoUrlPart.Substring(questionMarkIndex + 6);

            for (int i = 1; i < urlParts.Length; i++)
            {
                if (urlParts[i].StartsWith("version="))
                {
                    branchName = urlParts[i].Substring(10);
                }
                else if (urlParts[i].StartsWith("line="))
                {
                    lineNumber = int.Parse(urlParts[i].Substring(5));
                }
            }

            if (string.IsNullOrEmpty(repoUrl) || string.IsNullOrEmpty(filePath) || string.IsNullOrEmpty(branchName) || lineNumber < 0)
                return false;

            return true;
        }

        public static string GenerateUrlFromInfo(in string repoUrl, in string filePath, in string branchName, in int line, in int lineEnd, in int lineStartColumn, in int lineEndColumn)
        {
            return $"{FormatRepoUrl(repoUrl)}?path={filePath}&version=GB{branchName}&line={line}&lineEnd={lineEnd}&lineStartColumn={lineStartColumn}&lineEndColumn={lineEndColumn}&lineStyle=plain&_a=contents";
        }

        /// <summary>
        /// Repo url extracted from git can be: 
        ///  - https://user@dev.azure.com/user/Repository/_git/Repository
        ///  - https://tenant.visualstudio.com/Organization/_git/Repository
        /// </summary>
        /// <param name="url">Original url</param>
        /// <returns>Formated url</returns>
        public static string FormatRepoUrl(in string url)
        {
            if (!url.StartsWith(@"https://"))
                return url;

            string formatedUrl = url.Substring(8); // Remove "https://"

            var urlParts = formatedUrl.Split('/');
            if (urlParts.Length < 4)
                return url;

            if (urlParts[0].Contains("dev.azure.com"))
            {
                return $"https://{urlParts[1]}.visualstudio.com/_git/{urlParts[2]}";
            }

            return url;
        }
    }
}
