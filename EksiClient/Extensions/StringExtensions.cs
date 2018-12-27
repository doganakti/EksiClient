using System;
namespace EksiClient
{
    /// <summary>
    /// String extensions.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Ises the null.
        /// </summary>
        /// <returns><c>true</c>, if null was ised, <c>false</c> otherwise.</returns>
        /// <param name="input">Input.</param>
        public static bool IsNull(this string input)
        {
            if (input == "null" || input == "undefined")
            {
                return true;
            }
            else
            {
                return string.IsNullOrEmpty(input);
            }
        }

        /// <summary>
        /// Splits the camel case.
        /// </summary>
        /// <returns>The camel case.</returns>
        /// <param name="input">Input.</param>
        public static string SplitCamelCase(this string input)
        {
            return System.Text.RegularExpressions.Regex.Replace(input, "([A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.CultureInvariant).Trim();
        }
    }
}
