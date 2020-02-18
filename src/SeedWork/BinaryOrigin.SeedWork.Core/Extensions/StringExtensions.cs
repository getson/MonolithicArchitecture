using System;
using System.Linq;

namespace BinaryOrigin.SeedWork.Core
{
    public static class StringExtensions
    {
        public static bool EqualsIgnoreCase(this string str1, params string[] strings)
        {
            return strings != null &&
                   strings.All(str => string.Compare(str1, str, StringComparison.InvariantCultureIgnoreCase) == 0);
        }

        public static bool EqualsAnyIgnoreCase(this string str1, params string[] strings)
        {
            return strings != null &&
                   strings.Any(str => string.Compare(str1, str, StringComparison.InvariantCultureIgnoreCase) == 0);
        }

        public static bool IsNullOrEmpty(this string @string)
        {
            return string.IsNullOrWhiteSpace(@string);
        }

        /// <summary>
        /// Ensure that a string doesn't exceed maximum allowed length
        /// </summary>
        /// <param name="str">Input string</param>
        /// <param name="maxLength">Maximum length</param>
        /// <param name="postfix">A string to add to the end if the original string was shorten</param>
        /// <returns>Input string if its length is OK; otherwise, truncated input string</returns>
        public static string EnsureMaximumLength(this string str, int maxLength, string postfix = null)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            if (str.Length <= maxLength)
                return str;

            var pLen = postfix?.Length ?? 0;

            var result = str.Substring(0, maxLength - pLen);
            if (!string.IsNullOrEmpty(postfix))
            {
                result += postfix;
            }

            return result;
        }

        /// <summary>
        /// Ensures that a string only contains numeric values
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns>Input string with only numeric values, empty string if input is null/empty</returns>
        public static string EnsureNumericOnly(this string str)
        {
            return string.IsNullOrEmpty(str) ? string.Empty : new string(str.Where(char.IsDigit).ToArray());
        }

        /// <summary>
        /// Ensure that a string is not null
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns>Result</returns>
        public static string EnsureNotNull(this string str)
        {
            return str ?? string.Empty;
        }
    }
}