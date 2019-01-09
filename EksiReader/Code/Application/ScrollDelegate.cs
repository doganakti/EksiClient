using System;
using UIKit;

namespace EksiReader
{
    public class ScrollDelegate : UIScrollViewDelegate
    {
        public override void Scrolled(UIScrollView scrollView)
        {
            Console.WriteLine(scrollView.ContentOffset.Y);
        }
    }
}
