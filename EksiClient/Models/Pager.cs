using System;
namespace EksiClient
{
    /// <summary>
    /// Pager.
    /// </summary>
    public class Pager
    {
        /// <summary>
        /// Gets or sets the current page.
        /// </summary>
        /// <value>The current page.</value>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Gets or sets the page count.
        /// </summary>
        /// <value>The page count.</value>
        public int PageCount { get; set; }
    }
}
