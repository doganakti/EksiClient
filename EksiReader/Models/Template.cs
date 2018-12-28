using System;
using System.Collections.Generic;
using System.IO;

namespace EksiReader
{
    public class Template
    {
        /// <summary>
        /// Gets or sets the large bar font.
        /// </summary>
        /// <value>The large bar font.</value>
        public string LargeBarFontName { get; set; }

        /// <summary>
        /// Gets or sets the size of the large bar font.
        /// </summary>
        /// <value>The size of the large bar font.</value>
        public int LargeBarFontSize { get; set; }

        /// <summary>
        /// Gets or sets the name of the bar font.
        /// </summary>
        /// <value>The name of the bar font.</value>
        public string BarFontName { get; set; }

        /// <summary>
        /// Gets or sets the size of the bar font.
        /// </summary>
        /// <value>The size of the bar font.</value>
        public int BarFontSize { get; set; }

        /// <summary>
        /// Gets or sets the color of the bar background.
        /// </summary>
        /// <value>The color of the bar background.</value>
        public string BarBackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the bar font.
        /// </summary>
        /// <value>The color of the bar font.</value>
        public string BarFontColor { get; set; }

        /// <summary>
        /// Gets or sets the name of the text font.
        /// </summary>
        /// <value>The name of the text font.</value>
        public string TextFontName { get; set; }

        /// <summary>
        /// Gets or sets the size of the text font.
        /// </summary>
        /// <value>The size of the text font.</value>
        public int TextFontSize { get; set; }

        /// <summary>
        /// Gets or sets the color of the text.
        /// </summary>
        /// <value>The color of the text.</value>
        public string TextColor { get; set; }

        /// <summary>
        /// Gets or sets the name of the badge font.
        /// </summary>
        /// <value>The name of the badge font.</value>
        public string BadgeFontName { get; set; }

        /// <summary>
        /// Gets or sets the size of the badge font.
        /// </summary>
        /// <value>The size of the badge font.</value>
        public int BadgeFontSize { get; set; }

        /// <summary>
        /// Gets or sets the color of the badge font.
        /// </summary>
        /// <value>The color of the badge font.</value>
        public string BadgeFontColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the background.
        /// </summary>
        /// <value>The color of the background.</value>
        public string BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the link.
        /// </summary>
        /// <value>The color of the link.</value>
        public string LinkColor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:EksiReader.Template"/> is light.
        /// </summary>
        /// <value><c>true</c> if light; otherwise, <c>false</c>.</value>
        public bool Light { get; set; }
    }
}
