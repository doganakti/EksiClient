using EksiClient;
using Foundation;
using System;
using UIKit;

namespace EksiReader
{
    /// <summary>
    /// Custom entry cell.
    /// </summary>
    public partial class CustomEntryCell : UITableViewCell
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:EksiReader.CustomEntryCell"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
        public CustomEntryCell (IntPtr handle) : base (handle)
        {
        }

        /// <summary>
        /// Sets the entry.
        /// </summary>
        /// <param name="entry">Entry.</param>
        public void SetEntry(Entry entry)
        {
            if (TextView != null)
            {
                TextView.AttributedText = ((NSAttributedString)entry.AttributedString);
                TextView.Font = UIFont.FromName(Common.Template.TextFontName, Common.Template.TextFontSize);
                BackgroundColor = UIColor.Clear;
                TextView.TextColor = Common.Template.TextColor.ColorFromHEX();
                TextView.UserInteractionEnabled = true;
                TextView.BackgroundColor = UIColor.Clear;
                TextView.TintColor = Common.Template.LinkColor.ColorFromHEX();
                AuthorButton.SetTitle(entry.Author, UIControlState.Normal);
                DateLabel.Text = entry.Date;
                Layer.ShouldRasterize = true;
                Layer.RasterizationScale = UIScreen.MainScreen.Scale;
            }
        }
    }
}