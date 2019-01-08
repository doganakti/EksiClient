using Foundation;
using System;
using System.Threading.Tasks;
using UIKit;

namespace EksiReader
{
    public partial class LoginViewController : UIViewController
    {
        public LoginViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            var webViewDelegate =new LoginWebViewDelegate();
            webViewDelegate.LoggedIn += WebViewDelegate_LoggedIn;
            WebView.Delegate = webViewDelegate;
            WebView.LoadRequest(NSUrlRequest.FromUrl(NSUrl.FromString("https://eksisozluk.com/giris")));
            CancelButton.Clicked += CancelButton_Clicked;
            WebView.Opaque = false;
            WebView.BackgroundColor = UIColor.White;
        }

        void WebViewDelegate_LoggedIn(object sender, bool e)
        {
            var helper = new KeyChain.Net.XamarinIOS.KeyChainHelper();
            var isSaved = helper.SetKey("userName", "myKeyValue");
            var keyValue = helper.GetKey("myKey");
            var isDeleted = helper.DeleteKey("myKey");
            NavigationController.DismissViewController(true, null);
        }

        void CancelButton_Clicked(object sender, EventArgs e)
        {
            NavigationController.DismissViewController(true, null);
        }

    }
}