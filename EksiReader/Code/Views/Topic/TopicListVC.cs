using EksiClient;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using UIKit;

namespace EksiReader
{
    public partial class TopicListVC : UITableViewController
    {
        public TopicListVC (IntPtr handle) : base (handle)
        {
        }

        List<Topic> _topicList { get; set; } = new List<Topic>();
        List<Channel> _channelList { get; set; } = new List<Channel>();
        Topic _searchTopic;
        bool _channelMode = false;

        TopicsHeaderVC _topicsHeaderVC { get; set; }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            TableView.RowHeight = UITableView.AutomaticDimension;
            TableView.EstimatedRowHeight = 44;
            TableView.SeparatorColor = Common.Template.LinkColor.ColorFromHEX().ColorWithAlpha(0.2f);
            TableView.BackgroundColor = Common.Template.BackgroundColor.ColorFromHEX();
            Title = "Gündem";
            SearchBar.OnSearch += SearchBar_OnSearch;
            SearchBar.KeyboardAppearance = Common.Template.Light ? UIKeyboardAppearance.Light : UIKeyboardAppearance.Dark;
            AddRefreshControl();
            UpdateData();
            UIApplication.SharedApplication.SetStatusBarStyle(Common.Template.Light ? UIStatusBarStyle.Default : UIStatusBarStyle.LightContent, false);
            SetNeedsStatusBarAppearanceUpdate();

            _topicsHeaderVC = (TopicsHeaderVC)Storyboard.InstantiateViewController("TopicsHeaderViewController");
            _topicsHeaderVC.TopicSelected += _topicsHeaderVC_TopicSelected;
            var frame = _topicsHeaderVC.View.Frame;
            frame.Height = 39;
            _topicsHeaderVC.View.Frame = frame;
        }

        public void UpdateData(string path = "/basliklar/gundem")
        {
            InvokeInBackground(() =>
            {
                if (_channelMode)
                {
                    _channelList = EksiService.GetChannels();
                }
                else
                {
                    _topicList = EksiService.GetTopics(path);
                }
                BeginInvokeOnMainThread(() =>
                {
                    TableView.ReloadSections(NSIndexSet.FromIndex(0), UITableViewRowAnimation.Automatic);
                    TableView.ScrollRectToVisible(CoreGraphics.CGRect.Empty, true);
                    NSOperationQueue.CurrentQueue.AddOperation(() =>
                    {
                        Console.WriteLine(RefreshControl.State);
                        RefreshControl.EndRefreshing();
                    });
                });
            });
        }

        public override nint RowsInSection(UITableView tableView, nint section)
        {
            return _channelMode ? _channelList.Count : _topicList.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell("TopicCell", indexPath);

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

            if (_channelMode)
            {
                var channel = _channelList[indexPath.Row];
                ((UILabel)cell.AccessoryView).Text = "";
                cell.TextLabel.Text = channel.Title;
            }
            else
            {
                var topic = _topicList[indexPath.Row];
                ((UILabel)cell.AccessoryView).Text = topic.Badge;
                cell.TextLabel.Text = topic.Title;
            }

            cell.TextLabel.Font = UIFont.FromName(Common.Template.TextFontName, Common.Template.TextFontSize);
            cell.BackgroundColor = UIColor.Clear;
            cell.TextLabel.TextColor = Common.Template.TextColor.ColorFromHEX();


            return cell;
        }

        /// <summary>
        /// Rows the selected.
        /// </summary>
        /// <param name="tableView">Table view.</param>
        /// <param name="indexPath">Index path.</param>
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (_channelMode)
            {
                _channelMode = false;
                var channel = _channelList[indexPath.Row];
                Title = channel.Title;
                UpdateData(channel.Path);
            }
            else
            {
                PerformSegue("EntryListSegue", indexPath);
            }
            tableView.DeselectRow(indexPath, true);
        }

        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            return 40;
        }

        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            return _topicsHeaderVC.View;
        }

        /// <summary>
        /// Prepares for segue.
        /// </summary>
        /// <param name="segue">Segue.</param>
        /// <param name="sender">Sender.</param>
        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            if (sender.GetType() == typeof(MainSearch))
            {
                var entryListVC = (EntryListVC)segue.DestinationViewController;
                entryListVC.Topic = _searchTopic;
            }
            else if (segue.Identifier == "EntryListSegue")
            {
                var entryListVC = (EntryListVC)segue.DestinationViewController;
                var indexPath = (NSIndexPath)sender;
                var topic = _topicList[indexPath.Row];
                entryListVC.Topic = topic;
            }
        }

        void SearchBar_OnSearch(object sender, string e)
        {
            e = e.ToLower(new System.Globalization.CultureInfo("tr"));
            _searchTopic = EksiService.GetSearchTopic(e);
            SearchBar.Text = null;
            View.EndEditing(true);
            PerformSegue("EntryListSegue", SearchBar);
        }

        void AddRefreshControl()
        {
            RefreshControl = new UIRefreshControl();
            RefreshControl.ValueChanged += (sender, e) =>
            {
                Console.WriteLine(((UIRefreshControl)sender).State);
                UpdateData();
            };
            TableView.Add(RefreshControl);
        }

        void _topicsHeaderVC_TopicSelected(object sender, int e)
        {
            Title = ((UISegmentedControl)sender).TitleAt(e);
            _channelMode = false;
            if(e == 0)
            {
                string path = "/basliklar/gundem";
                UpdateData(path);
            }
            else if (e == 1)
            {
                string path = "/basliklar/bugun/1";
                UpdateData(path);
            }
            else if (e == 2)
            {
                _channelMode = true;
                UpdateData();
            }
        }

    }
}