using System.Text.RegularExpressions;

namespace PP3.Api.Utils
{
    public static class WordsHelper
    {
        private static readonly Regex MultiSpace = new(@"\s+", RegexOptions.Compiled);

        public static List<string> SplitWords(string text)
        {
            var normalized = MultiSpace.Replace(text.Trim(), " ");
            return normalized.Length == 0 ? new List<string>() : normalized.Split(' ').ToList();
        }

        public static (string prefix, string core, string suffix) AnalyzeToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                return ("", "", "");

            int start = 0;
            int end = token.Length - 1;

            while (start <= end && !char.IsLetter(token[start]))
                start++;

            while (end >= start && !char.IsLetter(token[end]))
                end--;

            if (start > end)
                return (token, "", ""); 

            var prefix = token[..start];
            var core = token.Substring(start, end - start + 1);
            var suffix = token[(end + 1)..];
            return (prefix, core, suffix);
        }
    }
}
