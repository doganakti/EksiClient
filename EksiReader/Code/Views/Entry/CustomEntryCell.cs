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

        TextDelegate _textDelegate;
        public TextDelegate TextDelegate
        {
            get
            {
                if(_textDelegate == null)
                {
                    _textDelegate = new TextDelegate();
                }
                return _textDelegate;
            }
        }

        event EventHandler<string> _onPath;
        public event EventHandler<string> OnPath
        {
            add
            {
                _onPath = null;
                _onPath += value;
            }
            remove
            {
                _onPath = null;
            }
        }

        event EventHandler<string> _onYoutube;
        public event EventHandler<string> OnYoutube
        {
            add
            {
                _onYoutube = null;
                _onYoutube += value;
            }
            remove
            {
                _onYoutube = null;
            }
        }

        /// <summary>
        /// Setup this instance.
        /// </summary>
        public void Setup()
        {
            TextView.Font = UIFont.FromName(Common.Template.TextFontName, Common.Template.TextFontSize);
            BackgroundColor = UIColor.Clear;
            TextView.TextColor = Common.Template.TextColor.ColorFromHEX();
            TextView.UserInteractionEnabled = true;
            TextView.BackgroundColor = UIColor.Clear;
            TextView.TintColor = Common.Template.LinkColor.ColorFromHEX();
            Layer.ShouldRasterize = true;
            Layer.RasterizationScale = UIScreen.MainScreen.Scale;
            TextView.Delegate = TextDelegate;
            TextDelegate.OnPath+= TextDelegate_OnPath;
            TextDelegate.OnYoutube += TextDelegate_OnYoutube;

        }

        /// <summary>
        /// Sets the entry.
        /// </summary>
        /// <param name="entry">Entry.</param>
        public void SetEntry(Entry entry)
        {
            TextView.AttributedText = (NSAttributedString)entry.AttributedString;
            AuthorButton.SetTitle(entry.Author, UIControlState.Normal);
            DateLabel.Text = entry.Date;
            Setup();
        }

        void TextDelegate_OnPath(object sender, string e)
        {
            if (_onPath != null)
            {
                _onPath.Invoke(this, e);
            }
        }

        void TextDelegate_OnYoutube(object sender, string e)
        {
            if (_onYoutube != null)
            {
                _onYoutube.Invoke(this, e);
            }
        }
    }
}