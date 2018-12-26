using Foundation;
using System;
using UIKit;

namespace EksiReader
{
    public partial class PagingVC : UIViewController
    {
        public PagingVC (IntPtr handle) : base (handle)
        {
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
        }
    }
}