using System;
namespace EksiClient
{
    /// <summary>
    /// Entry content.
    /// </summary>
    public class EntryContent
    {
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:EksiClient.EntryContent"/> is break.
        /// </summary>
        /// <value><c>true</c> if break; otherwise, <c>false</c>.</value>
        public bool Break { get; set; }

        /// <summary>
        /// Gets or sets the link title.
        /// </summary>
        /// <value>The link title.</value>
        public string LinkTitle { get; set; }

        /// <summary>
        /// Gets or sets the link path.
        /// </summary>
        /// <value>The link path.</value>
        public string LinkPath { get; set; }

        /// <summary>
        /// Gets or sets the inner link title.
        /// </summary>
        /// <value>The inner link title.</value>
        public string InnerLinkTitle { get; set; }

        /// <summary>
        /// Gets or sets the inner link path.
        /// </summary>
        /// <value>The inner link path.</value>
        public string InnerLinkPath { get; set; }
    }
}
