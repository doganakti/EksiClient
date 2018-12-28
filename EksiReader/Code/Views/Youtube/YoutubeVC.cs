using Foundation;
using System;
using UIKit;
using WebKit;
using YouTube.Player;

namespace EksiReader
{
    public partial class YoutubeVC : UIViewController
    {
        public YoutubeVC(IntPtr handle) : base(handle)
        {
        }

        public event EventHandler<UIView> OnDismiss;

        UIView Container;
        UIPanGestureRecognizer _panGestureRecognizer;

        PlayerView YoutubeView;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            _panGestureRecognizer = new UIPanGestureRecognizer((UIPanGestureRecognizer pan) =>
            {
                var p = pan.TranslationInView(this.View);
                var v = pan.VelocityInView(this.View);
                Console.WriteLine("Pan. transration:{0}, velocity:{1}", p, v);
                var frame = Container.Frame;
                frame.Y = p.Y;
                frame.X = p.X;
                Container.Frame = frame;
                var alpha = 1f - frame.Y / 500f;
                YoutubeView.Alpha = alpha;

                if (pan.State == UIGestureRecognizerState.Ended)
                {
                    if (YoutubeView.Alpha < 0.5)
                    {
                        if (OnDismiss != null)
                        {
                            OnDismiss.Invoke(this, Container);
                        }
                        Container.RemoveFromSuperview();
                        YoutubeView.RemoveFromSuperview();

                    }
                    else
                    {
                        UIView.Animate(0.1, () =>
                        {
                            frame = Container.Frame;
                            frame.Y = 15;
                            frame.X = 15;
                            Container.Frame = frame;
                            YoutubeView.Alpha = 1;
                        });
                    }
                }
            });

        }

        public void LoadVideo(string videoId)
        {
            var iframe = $"https://www.youtube.com/embed/{videoId}?playsinline=1&autoplay=1&vq=hd720&origin=https://youtube.com";
            View.BackgroundColor = UIColor.Clear;
            LoadWebVideo(videoId);

        }

        public void LoadWebVideo(string url)
        {
            if (Container != null && YoutubeView != null)
            {
                YoutubeView.RemoveFromSuperview();
                Container.RemoveFromSuperview();
            }
            Container = new UIView(new CoreGraphics.CGRect(10, 10, View.Frame.Size.Width - 20, View.Frame.Size.Width * 9 / 16 - 20));
            Container.Layer.CornerRadius = 8;
            Container.Layer.ShadowRadius = 8;
            Container.Layer.ShadowOpacity = 0.2f;
            Container.Layer.ShadowOffset = new CoreGraphics.CGSize(0, 0);

            YoutubeView = new PlayerView(new CoreGraphics.CGRect(0, 0, Container.Frame.Size.Width, Container.Frame.Size.Height));
            YoutubeView.Layer.CornerRadius = 8;
            YoutubeView.Layer.ShadowOffset = new CoreGraphics.CGSize(0, 0);
            YoutubeView.Layer.MasksToBounds = true;
            Container.AddSubview(YoutubeView);

            View.AddGestureRecognizer(_panGestureRecognizer);
            View.AddSubview(Container);

            object[] keys = { "playsinline", "origin", "controls", "autoplay", "modestbranding", "rel", "autohide" };
            object[] values = { 1, "http://www.youtube.com", 1, 1, 1, 0, 1 };
            var playerVars = NSDictionary.FromObjectsAndKeys(values, keys, keys.Length);
            YoutubeView.LoadVideoById(url, playerVars);
            YoutubeView.BackgroundColor = UIColor.Orange;
            YoutubeView.Delegate = new VideoDelegate();
        }

        public class VideoDelegate : YouTube.Player.PlayerViewDelegate
        {
            public override void DidBecomeReady(PlayerView playerView)
            {
                playerView.PlayVideo();
            }
        }
    }
}