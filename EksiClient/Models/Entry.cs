using System;
using System.Collections.Generic;

namespace EksiClient
{
    /// <summary>
    /// Entry.
    /// </summary>
    public class Entry
    {
        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the content list.
        /// </summary>
        /// <value>The content list.</value>
        public List<EntryContent> ContentList { get; set; }

        /// <summary>
        /// Gets or sets the author.
        /// </summary>
        /// <value>The author.</value>
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets the author path.
        /// </summary>
        /// <value>The author path.</value>
        public string AuthorPath { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        public string Date { get; set; }
    }
}
