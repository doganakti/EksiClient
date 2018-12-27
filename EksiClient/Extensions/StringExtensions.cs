using System;
using System.Linq;

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

        /// <summary>
        /// Gets the video identifier.
        /// </summary>
        /// <returns>The video identifier.</returns>
        /// <param name="url">URL.</param>
        public static string GetVideoId(this string url)
        {
            try
            {
                var uri = new Uri(url);
                var query = HttpUtility.ParseQueryString(uri.Query);
                var videoId = string.Empty;
                if (query.ContainsKey("v"))
                {
                    videoId = query["v"];
                }
                else
                {
                    videoId = uri.Segments.Last();
                }
                return videoId;
            }
            catch
            {
                return null;
            }
        }
    }
}
