using Foundation;
using System;
using UIKit;
using YouTube.Player;

namespace EksiReader
{
    public partial class YoutubeVC : UIViewController
    {
        public YoutubeVC (IntPtr handle) : base (handle)
        {
        }


        PlayerView _playerView;
        PlayerView PlayerView
        {
            get
            {
                if (_playerView == null)
                {
                    _playerView = new PlayerView();
                }
                return _playerView;
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            PlayerView.Frame = new CoreGraphics.CGRect(0, 0, View.Frame.Size.Width - 20, View.Frame.Size.Width * 9 / 16 - 20);
            PlayerView.BackgroundColor = UIColor.Orange;
            View.AddSubview(PlayerView);
        }

        public void LoadVideo(string videoId)
        {
            PlayerView.LoadVideoById(videoId);
            PlayerView.PlayVideo();
        }
    }
}