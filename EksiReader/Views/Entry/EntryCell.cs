using CoreGraphics;
using EksiClient;
using Foundation;
using System;
using UIKit;

namespace EksiReader
{
    /// <summary>
    /// Entry cell.
    /// </summary>
    public partial class EntryCell : UITableViewCell
    {
        public EntryCell() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:EksiReader.EntryCell"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
        public EntryCell (IntPtr handle) : base (handle)
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
                TextView.AttributedText = Common.GetAttributedString(entry.ContentList);
                TextView.Font = UIFont.FromName(Common.Template.TextFontName, Common.Template.TextFontSize);
                BackgroundColor = UIColor.Clear;
                TextView.TextColor = Common.Template.TextColor.ColorFromHEX();
                TextView.UserInteractionEnabled = true;
            }
        }

        public override void Draw(CGRect rect)
        {

        }
    }
}