using CoreGraphics;
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

        Pager _pager;
        MorePage _more;

        string _path;

        PagingVC _pagingVC;
        PagingVC PagingVC
        {
            get
            {
                if (_pagingVC == null)
                {
                    _pagingVC = (PagingVC)Storyboard.InstantiateViewController("PagingViewController");
                    _pagingVC.OnPage += _pagingVC_OnPage;
                }
                return _pagingVC;
            }
        }

        TopicsHeaderVC _topicsHeaderVC { get; set; }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            TableView.RowHeight = UITableView.AutomaticDimension;
            TableView.EstimatedRowHeight = 44;
            TableView.SeparatorColor = Common.Template.LinkColor.ColorFromHEX().ColorWithAlpha(0.2f);
            TableView.BackgroundColor = Common.Template.BackgroundColor.ColorFromHEX();
            View.BackgroundColor = Common.Template.BackgroundColor.ColorFromHEX();
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

            AccountButton.Clicked += AccountButton_Clicked;
        }

        public void UpdateData(string path = "/basliklar/gundem", int page = 0)
        {
            InvokeOnMainThread(() =>
            {
                var y = TableView.ContentOffset.Y;
                y = y <= 0 ? 0 : 44;
                TableView.ContentOffset = new CGPoint(0, y);
                NSOperationQueue.CurrentQueue.AddOperation(() =>
                {
                    InvokeInBackground(() =>
                    {
                        if (_channelMode)
                        {
                            _channelList = EksiService.GetChannels();
                            _topicList = new List<Topic>();
                            _pager = null;
                            _more = null;
                        }
                        else
                        {
                            var result = EksiService.GetTopics(path, page);
                            _path = result.Path;
                            _topicList = result.ResultList;
                            _pager = result.Pager;
                            _more = result.MorePage;
                            _channelList = new List<Channel>();
                        }
                        InvokeOnMainThread(() =>
                        {
                            TableView.LayoutIfNeeded();
                            TableView.ContentOffset = new CGPoint(0, y);
                            NSOperationQueue.CurrentQueue.AddOperation(() =>
                            {
                                TableView.ReloadSections(NSIndexSet.FromIndex(0), UITableViewRowAnimation.Automatic);
                                RefreshControl.EndRefreshing();
                                PagingVC.Pager = _pager;
                            });
                        });
                    });
                });
            });

        }

        public override nint RowsInSection(UITableView tableView, nint section)
        {
            var count = _channelMode ? _channelList.Count : _topicList.Count;
            if(!_channelMode)
            {
                count = _more != null ? _topicList.Count + 1 : _topicList.Count;
            }
            return count;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {

            var cell = tableView.DequeueReusableCell("TopicCell", indexPath);
            if (cell.AccessoryView == null)
            {
                cell.AccessoryView = new UILabel
                {
                    Frame = new CoreGraphics.CGRect(0, 0, 64, 44),
                    TextAlignment = UITextAlignment.Right,
                    Font = UIFont.FromName(Common.Template.BadgeFontName, Common.Template.BadgeFontSize),
                    TextColor = Common.Template.BadgeFontColor.ColorFromHEX()
                };
            }
            cell.TextLabel.Font = UIFont.FromName(Common.Template.TextFontName, Common.Template.TextFontSize);
            cell.BackgroundColor = UIColor.Clear;
            cell.TextLabel.TextColor = Common.Template.TextColor.ColorFromHEX();

            if (_channelMode)
            {
                if (_channelList.HasItem())
                {
                    var channel = _channelList[indexPath.Row];
                    ((UILabel)cell.AccessoryView).Text = "";
                    cell.TextLabel.Text = channel.Title;
                }
            }
            else
            {
                if (indexPath.Row < _topicList.Count)
                {
                    var topic = _topicList[indexPath.Row];
                    ((UILabel)cell.AccessoryView).Text = topic.Badge;
                    cell.TextLabel.Text = topic.Title;
                }
                else if(indexPath.Row == _topicList.Count && _topicList.Count > 0)
                {
                    cell.TextLabel.Text = "";
                    ((UILabel)cell.AccessoryView).Text = "Daha da";
                }
            }

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
                if (indexPath.Row < _topicList.Count)
                {
                    PerformSegue("EntryListSegue", indexPath);
                }
                else
                {
                    UpdateData(_more.Path);
                }
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

        public override nfloat GetHeightForFooter(UITableView tableView, nint section)
        {
            return 60;
        }

        public override UIView GetViewForFooter(UITableView tableView, nint section)
        {
            return PagingVC.View;
        }

        /// <summary>
        /// Prepares for segue.
        /// </summary>
        /// <param name="segue">Segue.</param>
        /// <param name="sender">Sender.</param>
        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            if (segue.Identifier == "EntryListSegue")
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
                _path = "/basliklar/gundem";
                UpdateData(_path);
            }
            else if (e == 1)
            {
                _path = "/basliklar/bugun/1";
                UpdateData(_path);
            }
            else if (e == 2)
            {
                _channelMode = true;
                UpdateData();
            }
        }

        void AccountButton_Clicked(object sender, EventArgs e)
        {
            PerformSegue("Account", null);
        }

        void _pagingVC_OnPage(object sender, int e)
        {
            Console.WriteLine(e);
            if (_path.Contains("/basliklar/bugun"))
            {
                _path = "/basliklar/bugun/" + e.ToString();
                UpdateData(_path, 0);
            }
            else
            {
                UpdateData(_path, e);
            }

        }

    }
}