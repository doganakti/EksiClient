using EksiClient;
using Foundation;
using System;
using UIKit;

namespace EksiReader
{
    public partial class PagingVC : UIViewController
    {
        public event EventHandler<int> OnNext;
        public event EventHandler<int> OnPrevious;

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
            Container.Layer.ShadowRadius = 3;
            Container.Layer.ShadowOpacity = 0.5f;
            Container.Layer.ShadowOffset = new CoreGraphics.CGSize(0, 0);
            Container.Hidden = true;
            PageButton.SetTitle("", UIControlState.Normal);
            NextButton.TouchUpInside += NextButton_TouchUpInside;
            PreviousButton.TouchUpInside += PreviousButton_TouchUpInside;
        }

        void SetPage()
        {
            if (_pager != null)
            {
                var pageTitle = $"{_pager.CurrentPage} / {_pager.PageCount}";
                PageButton.SetTitle(pageTitle, UIControlState.Normal);
                PreviousButton.Alpha = _pager.CurrentPage == 1 ? 0.5f : 1;
                PreviousButton.Enabled = _pager.CurrentPage != 1;
                NextButton.Alpha = _pager.CurrentPage == _pager.PageCount ? 0.5f : 1;
                NextButton.Enabled = _pager.CurrentPage != _pager.PageCount;
            }
        }

        void NextButton_TouchUpInside(object sender, EventArgs e)
        {
            if (OnNext != null)
            {
                OnNext.Invoke(this, _pager.CurrentPage + 1);
            }
        }

        void PreviousButton_TouchUpInside(object sender, EventArgs e)
        {
            if (OnPrevious != null)
            {
                OnPrevious.Invoke(this, _pager.CurrentPage - 1);
            }
        }

    }
}