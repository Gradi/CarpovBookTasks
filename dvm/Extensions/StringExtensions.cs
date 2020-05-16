using System;
using System.Collections.Generic;
using System.Text;

namespace dvm.Extensions
{
    public static class StringExtensions
    {
        public static IEnumerable<string> AsArgs(this string? str)
        {
            if (string.IsNullOrWhiteSpace(str))
                yield break;

            var builder = new StringBuilder();
            bool isQuotes = false;

            foreach (char ch in str)
            {
                if (isQuotes)
                {
                    if (ch == '"')
                    {
                        isQuotes = false;
                        yield return builder.ToString();
                        builder.Clear();
                    }
                    else
                        builder.Append(ch);
                }
                else
                {
                    if (ch == '"')
                    {
                        isQuotes = true;
                    }
                    else if (char.IsWhiteSpace(ch) && builder.Length != 0)
                    {
                        yield return builder.ToString();
                        builder.Clear();
                    }
                    else
                        builder.Append(ch);
                }
            }

            if (builder.Length != 0)
                yield return builder.ToString();
        }

    }
}
