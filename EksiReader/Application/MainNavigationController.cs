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
                this.NavigationBar.PrefersLargeTitles = true;
                NavigationBar.LargeTitleTextAttributes = new UIStringAttributes()
                {
                    Font = UIFont.FromName("OpenSans-SemiBold", 30),
                    ForegroundColor = "#484035".ColorFromHEX()
                };
            }

            UINavigationBar.Appearance.ShadowImage = new UIImage();
            NavigationBar.BarTintColor = "#f6efdc".ColorFromHEX();
            NavigationBar.TitleTextAttributes = new UIStringAttributes()
            {
                Font = UIFont.FromName("OpenSans-SemiBold", 20),
                ForegroundColor = "#484035".ColorFromHEX()
            };

        }
    }
}