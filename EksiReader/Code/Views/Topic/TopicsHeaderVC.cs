using Foundation;
using System;
using UIKit;

namespace EksiReader
{
    public partial class TopicsHeaderVC : UIViewController
    {
        public event EventHandler<int> TopicSelected;

        public TopicsHeaderVC (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            SegmentedControl.TintColor = Common.Template.LinkColor.ColorFromHEX();
            View.BackgroundColor = Common.Template.BackgroundColor.ColorFromHEX();
            SegmentedControl.ValueChanged += SegmentedControl_ValueChanged;
        }

        void SegmentedControl_ValueChanged(object sender, EventArgs e)
        {
            if (TopicSelected != null)
            {
                TopicSelected.Invoke(sender, (int)((UISegmentedControl)sender).SelectedSegment);
            }
        }

    }
}