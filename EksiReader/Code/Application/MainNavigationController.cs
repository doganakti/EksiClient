using Foundation;
using System;
using UIKit;

namespace EksiReader
{
    public partial class MainNavigationController : UINavigationController
    {
        public MainNavigationController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            if (Common.SystemVersion >= 11.0)
            {
                this.NavigationBar.PrefersLargeTitles = false;
                NavigationBar.LargeTitleTextAttributes = new UIStringAttributes()
                {
                    Font = UIFont.FromName(Common.Template.LargeBarFontName, Common.Template.LargeBarFontSize),
                    ForegroundColor = Common.Template.BarFontColor.ColorFromHEX()
                };
            }

            UINavigationBar.Appearance.ShadowImage = new UIImage();
            NavigationBar.BarTintColor = Common.Template.BarBackgroundColor.ColorFromHEX();
            NavigationBar.TintColor = Common.Template.LinkColor.ColorFromHEX();
            NavigationBar.Translucent = false;
            NavigationBar.TitleTextAttributes = new UIStringAttributes()
            {
                Font = UIFont.FromName(Common.Template.BarFontName, Common.Template.BarFontSize),
                ForegroundColor = Common.Template.BarFontColor.ColorFromHEX()
            };
        }
    }
}