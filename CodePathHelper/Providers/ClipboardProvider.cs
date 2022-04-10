using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CodePathHelper.Providers
{
    internal static class ClipboardProvider
    {
        internal static void SetClipboardData(string plainText, string html) 
        {
            DataObject dataObject = new DataObject();
            dataObject.SetText(plainText, TextDataFormat.Text);
            dataObject.SetText(plainText, TextDataFormat.UnicodeText);
            dataObject.SetText(GenerateHtmlForClipboard(html), TextDataFormat.Html);
            Clipboard.SetDataObject(dataObject, true);
        }

        internal static string GenerateAElement(string url) 
        {
            return $"<a href=\"{url}\" target=\"_blank\">{url}</a>";
        }

        private static string GenerateHtmlForClipboard(string htmlFragment)
        {
            StringBuilder sb = new StringBuilder();

            // Builds the CF_HTML header. See format specification here:
            // http://msdn.microsoft.com/library/default.asp?url=/workshop/networking/clipboard/htmlclipboard.asp

            // The string contains index references to other spots in the string, so we need placeholders so we can compute the offsets.
            // The <<<<<<<_ strings are just placeholders. We'll backpatch them actual values afterwards.
            // The string layout (<<<) also ensures that it can't appear in the body of the html because the <
            // character must be escaped.
            string header =
    @"Version:1.0
StartHTML:<<<<<<<1
EndHTML:<<<<<<<2
StartFragment:<<<<<<<3
EndFragment:<<<<<<<4
StartSelection:<<<<<<<3
EndSelection:<<<<<<<4
";

            string pre =
                @"<!DOCTYPE html><HTML><HEAD><meta charset=""UTF-8""><TITLE>Snippet</TITLE></HEAD><BODY><!--StartFragment-->";

            string post = @"<!--EndFragment--></BODY></HTML>";

            sb.Append(header);
            int startHTML = sb.Length;

            sb.Append(pre);
            int fragmentStart = startHTML + pre.Length;

            sb.Append(htmlFragment);
            int fragmentEnd = fragmentStart + GetByteCount(htmlFragment);

            sb.Append(post);
            int endHTML = fragmentEnd + post.Length;

            // Backpatch offsets
            sb.Replace("<<<<<<<1", To8DigitString(startHTML));
            sb.Replace("<<<<<<<2", To8DigitString(endHTML));
            sb.Replace("<<<<<<<3", To8DigitString(fragmentStart));
            sb.Replace("<<<<<<<4", To8DigitString(fragmentEnd));

            // Finally copy to clipboard.
            string data = sb.ToString();
            return data;
        }

        private static int GetByteCount(string fragment)
        {
            int result = Encoding.UTF8.GetByteCount(fragment);
            return result;
        }

        private static string To8DigitString(int x)
        {
            return string.Format("{0,8}", x.ToString("D8"));
        }
    }
}
