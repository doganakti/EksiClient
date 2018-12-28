using System;
using Foundation;
using UIKit;
using EksiClient;

namespace EksiReader
{
    public class TextDelegate : UITextViewDelegate
    {
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

        event EventHandler<NSUrl> _onUrl;
        public event EventHandler<NSUrl> OnUrl
        {
            add
            {
                _onUrl = null;
                _onUrl += value;
            }
            remove
            {
                _onUrl = null;
            }
        }


        public override bool ShouldInteractWithUrl(UITextView textView, NSUrl URL, NSRange characterRange)
        {
            bool isPath = URL != null && URL.ToString().Substring(0, 1).Contains("/");
            if (isPath)
            {
                Console.WriteLine($"path detected: {URL.ToString()}");
                if (_onPath != null)
                {
                    _onPath.Invoke(this, URL.ToString());
                }
                return false;
            }
            else if (IsYoutube(URL))
            {
                Console.WriteLine($"video detected: {URL.ToString()}");
                if (_onYoutube != null)
                {
                    var videoId = URL.ToString().GetVideoId();
                    if (videoId != null)
                    {
                        _onYoutube.Invoke(this, videoId);
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            else if(URL != null)
            {
                Console.WriteLine($"url detected: {URL}");
                if (_onUrl != null)
                {
                    _onUrl.Invoke(this, URL);
                    return false;
                }
            }
            return true;
        }

        bool IsYoutube(NSUrl url)
        {
            try
            {
                bool isYoutube = url.ToString().Contains("youtube.com/watch") || url.ToString().Contains("youtu.be");
                return isYoutube;
            }
            catch
            {
                return false;
            }
        }
    }
}
