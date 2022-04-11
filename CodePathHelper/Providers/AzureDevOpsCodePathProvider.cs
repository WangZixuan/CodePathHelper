namespace CodePathHelper.Providers
{
    public static class AzureDevOpsCodePathProvider
    {
        public static bool ExtractInfoFromUrl(in string url, out string repoUrl, out string filePath, out string branchName, out int line, out int lineEnd, out int lineStartColumn, out int lineEndColumn)
        {
            repoUrl = string.Empty;
            filePath = string.Empty;
            branchName = string.Empty;
            line = -1;
            lineEnd = -1;
            lineStartColumn = -1;
            lineEndColumn = -1;

            if (string.IsNullOrEmpty(url))
                return false;

            var urlParts = url.Trim().Split('&');

            string repoUrlPart = urlParts[0];

            int questionMarkIndex = url.IndexOf('?');
            if (questionMarkIndex == -1)
                return false;
            repoUrl = repoUrlPart.Substring(0, questionMarkIndex);
            filePath = repoUrlPart.Substring(questionMarkIndex + 6).Trim('/');

            for (int i = 1; i < urlParts.Length; i++)
            {
                if (urlParts[i].StartsWith("version="))
                {
                    branchName = urlParts[i].Substring(10);
                }
                else if (urlParts[i].StartsWith("line="))
                {
                    line = int.Parse(urlParts[i].Substring(5));
                }
                else if (urlParts[i].StartsWith("lineEnd="))
                {
                    lineEnd = int.Parse(urlParts[i].Substring(8));
                }
                else if (urlParts[i].StartsWith("lineStartColumn="))
                {
                    lineStartColumn = int.Parse(urlParts[i].Substring(16));
                }
                else if (urlParts[i].StartsWith("lineEndColumn="))
                {
                    lineEndColumn = int.Parse(urlParts[i].Substring(14));
                }
            }

            if (string.IsNullOrEmpty(repoUrl) || string.IsNullOrEmpty(filePath) || string.IsNullOrEmpty(branchName) || 
                line < 0 || lineEnd < 0 || lineStartColumn < 0 || lineEndColumn < 0)
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
