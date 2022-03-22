namespace CodePathHelper.Providers
{
    using System.Text.RegularExpressions;

    public static class TextProvider
    {
        public static bool ExtractUrl(in string text, out string url)
        {
            var urlParser = new Regex(@"(https?\://|www.)[A-Za-z0-9\.\-\/]+/?([A-Za-z0-9\?\&\=;\+!'\(\)\*\-\._~%\/]*)*", RegexOptions.IgnoreCase);

            var urlCollections = urlParser.Matches(text);
            if (urlCollections.Count == 0)
            {
                url = string.Empty;
                return false;
            }

            url = urlCollections[0].Value;
            return true;
        }
    }
}
