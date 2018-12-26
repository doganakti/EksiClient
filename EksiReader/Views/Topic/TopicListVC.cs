using EksiClient;
using Foundation;
using System;
using System.Collections.Generic;
using UIKit;

namespace EksiReader
{
    public partial class TopicListVC : UITableViewController
    {
        public TopicListVC (IntPtr handle) : base (handle)
        {
        }

        List<Topic> _topicList = EksiService.GetTopics();

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            TableView.RowHeight = UITableView.AutomaticDimension;
            TableView.EstimatedRowHeight = 44;
            //TableView.SeparatorColor = UIColor.Clear;
            TableView.BackgroundColor = Common.Template.BackgroundColor.ColorFromHEX();
            //TableView.ReloadData();
            Title = "Gündem";

            NavigationController.NavigationBar.PrefersLargeTitles = true;
        }

        public override nint RowsInSection(UITableView tableView, nint section)
        {
            return _topicList.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell("TopicCell", indexPath);
            var topic = _topicList[indexPath.Row];
            if (cell.AccessoryView == null)
            {
                cell.AccessoryView = new UILabel
                {
                    Frame = new CoreGraphics.CGRect(0, 0, 44, 44),
                    TextAlignment = UITextAlignment.Right,
                    Font = UIFont.FromName(Common.Template.BadgeFontName, Common.Template.BadgeFontSize),
                    TextColor = Common.Template.BadgeFontColor.ColorFromHEX()
                };
            }
            ((UILabel)cell.AccessoryView).Text = topic.Badge;
            cell.TextLabel.Text = topic.Title;
            cell.TextLabel.Font = UIFont.FromName(Common.Template.TextFontName, Common.Template.TextFontSize);
            cell.BackgroundColor = UIColor.Clear;
            cell.TextLabel.TextColor = Common.Template.TextColor.ColorFromHEX();
            return cell;
        }
    }
}