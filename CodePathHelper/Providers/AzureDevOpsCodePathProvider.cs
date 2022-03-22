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
            return $"{repoUrl}?path={filePath}&version=GB{branchName}&line={line}&lineEnd={lineEnd}&lineStartColumn={lineStartColumn}&lineEndColumn={lineEndColumn}&lineStyle=plain&_a=contents";
        }
    }
}
