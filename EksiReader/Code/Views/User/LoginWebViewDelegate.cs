using System;
using EksiClient;
using Foundation;
using UIKit;

namespace EksiReader
{
    public class LoginWebViewDelegate : UIWebViewDelegate
    {
        string _userName;
        string _password;
        bool _loggedIn;
        bool _loginFailed;

        public event EventHandler<bool> LoggedIn;

        public override void LoadStarted(UIWebView webView)
        {
            try
            {
                if (string.IsNullOrEmpty(_userName))
                {
                    _userName = webView.EvaluateJavascript(@"document.getElementById('username').value");
                }
                if (string.IsNullOrEmpty(_password))
                {
                    _password = webView.EvaluateJavascript(@"document.getElementById('password').value");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public override void LoadingFinished(UIWebView webView)
        {
            if (!string.IsNullOrEmpty(_userName) && !string.IsNullOrEmpty(_password) && !_loggedIn)
            {
                if (!_loginFailed)
                {
                    Console.WriteLine(_userName);
                    Console.WriteLine(_password);
                    _loggedIn = EksiService.Login(_userName, _password);
                    _loginFailed = !_loggedIn;
                    if (_loggedIn && LoggedIn != null)
                    {
                        var helper = new KeyChain.Net.XamarinIOS.KeyChainHelper();
                        bool userSet = helper.SetKey("pass", _userName);
                        bool passSet = helper.SetKey("password", _password);
                        LoggedIn.Invoke(this, true);
                    }
                }
            }
        }
    }
}
