using EksiClient;
using Foundation;
using System;
using UIKit;

namespace EksiReader
{
    public partial class PagingVC : UIViewController
    {
        public event EventHandler<int> OnPage;

        public PagingVC (IntPtr handle) : base (handle)
        {
        }

        Pager _pager;
        public Pager Pager
        {
            get
            {
                return _pager;
            }
            set
            {
                _pager = value;
                Container.Hidden = _pager == null;
                SetPage();
            }
        }

        public UIView MainContainer => Container;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            Container.BackgroundColor = Common.Template.BackgroundColor.ColorFromHEX();
            Container.Layer.CornerRadius = Container.Frame.Size.Height/2;
            Container.Layer.ShadowRadius = 8;
            Container.Layer.ShadowOpacity = 0.2f;
            Container.Layer.ShadowOffset = new CoreGraphics.CGSize(0, 0);
            Container.Hidden = true;
            PageButton.SetTitle("", UIControlState.Normal);
            NextButton.TouchUpInside += NextButton_TouchUpInside;
            PreviousButton.TouchUpInside += PreviousButton_TouchUpInside;
            FirstButton.TouchUpInside += FirstButton_TouchUpInside;
            LastButton.TouchUpInside += LastButton_TouchUpInside;
        }

        void SetPage()
        {
            if (_pager != null)
            {
                var pageTitle = $"{_pager.CurrentPage} / {_pager.PageCount}";
                PageButton.SetTitle(pageTitle, UIControlState.Normal);
                PreviousButton.Alpha = _pager.CurrentPage == 1 ? 0.5f : 1;
                PreviousButton.Enabled = _pager.CurrentPage != 1;
                FirstButton.Alpha = PreviousButton.Alpha;
                FirstButton.Enabled = PreviousButton.Enabled;
                NextButton.Alpha = _pager.CurrentPage == _pager.PageCount ? 0.5f : 1;
                NextButton.Enabled = _pager.CurrentPage != _pager.PageCount;
                LastButton.Alpha = NextButton.Alpha;
                LastButton.Enabled = NextButton.Enabled;
            }
        }

        void NextButton_TouchUpInside(object sender, EventArgs e)
        {
            if (OnPage != null)
            {
                OnPage.Invoke(this, _pager.CurrentPage + 1);
            }
        }

        void PreviousButton_TouchUpInside(object sender, EventArgs e)
        {
            if (OnPage != null)
            {
                OnPage.Invoke(this, _pager.CurrentPage - 1);
            }
        }

        void FirstButton_TouchUpInside(object sender, EventArgs e)
        {
            if (OnPage != null)
            {
                OnPage.Invoke(this, 1);
            }
        }

        void LastButton_TouchUpInside(object sender, EventArgs e)
        {
            if (OnPage != null)
            {
                OnPage.Invoke(this, _pager.PageCount);
            }
        }

    }
}