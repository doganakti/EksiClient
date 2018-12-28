using CoreGraphics;
using EksiClient;
using Foundation;
using ObjCRuntime;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UIKit;
using YouTube;
using YouTube.Player;

namespace EksiReader
{
    public partial class EntryListVC : UITableViewController
    {
        public Topic Topic { get; set; }

        Pager _pager;

        /// <summary>
        /// Gets or sets the entry list.
        /// </summary>
        /// <value>The entry list.</value>
        List<Entry> _entryList { get; set; }

        MoreData _moreData { get; set; }

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

        YoutubeVC _youtubeVC;
        YoutubeVC YoutubeVC
        {
            get
            {
                if (_youtubeVC == null)
                {
                    _youtubeVC = (YoutubeVC)Storyboard.InstantiateViewController("YoutubeViewController");
                    _youtubeVC.OnDismiss += _youtubeVC_OnDismiss;
                }
                return _youtubeVC;
            }
        }

        bool _showHeader;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:EksiReader.EntryListVC"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
        public EntryListVC(IntPtr handle) : base(handle)
        {
        }

        /// <summary>
        /// Views the did load.
        /// </summary>
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            TableView.RowHeight = UITableView.AutomaticDimension;
            TableView.EstimatedRowHeight = 44;
            TableView.SeparatorColor = Common.Template.LinkColor.ColorFromHEX().ColorWithAlpha(0.2f);
            TableView.BackgroundColor = Common.Template.BackgroundColor.ColorFromHEX();
            TableView.AllowsSelection = true;
            if (Topic != null && _entryList == null)
            {
                SetEntryList();
            }
        }

        void SetEntryList(int page = 0)
        {
            InvokeInBackground(() =>
            {
                var result = EksiService.GetEntries(Topic.Path, page);
                _entryList = result.ResultList;
                _pager = result.Pager;
                _moreData = result.MoreData;
                Topic = result.Topic;
                foreach (var entry in _entryList)
                {
                    entry.AttributedString = Common.GetAttributedString(entry.ContentList);
                }
                BeginInvokeOnMainThread(() =>
                {
                    if (Topic != null)
                    {
                        PagingVC.Pager = _pager;
                        TableView.ReloadData();
                        TableView.ScrollToRow(NSIndexPath.FromRowSection(0, 0), UITableViewScrollPosition.Top, false);
                    }
                });
            });
        }

        /// <summary>
        /// Rowses the in section.
        /// </summary>
        /// <returns>The in section.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="section">Section.</param>
        public override nint RowsInSection(UITableView tableView, nint section)
        {
            return _entryList != null ? _entryList.Count + 1 : 0;
        }

        /// <summary>
        /// Gets the view for footer.
        /// </summary>
        /// <returns>The view for footer.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="section">Section.</param>
        public override UIView GetViewForFooter(UITableView tableView, nint section)
        {
            return PagingVC.View;
        }


        /// <summary>
        /// Gets the cell.
        /// </summary>
        /// <returns>The cell.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="indexPath">Index path.</param>
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            if (indexPath.Row == 0)
            {
                var cell = tableView.DequeueReusableCell("TitleCell");
                cell.TextLabel.Font = UIFont.FromName(Common.Template.LargeBarFontName, Common.Template.LargeBarFontSize);
                cell.TextLabel.TextColor = Common.Template.BarFontColor.ColorFromHEX();
                cell.TextLabel.Text = Topic.Title;
                cell.DetailTextLabel.Font = UIFont.FromName(Common.Template.TextFontName, Common.Template.TextFontSize);
                if (_moreData != null)
                {
                    cell.SelectionStyle = UITableViewCellSelectionStyle.Gray;
                    cell.DetailTextLabel.Text = _moreData.Title;
                }
                else
                {
                    cell.DetailTextLabel.Text = null;
                    cell.SelectionStyle = UITableViewCellSelectionStyle.None;
                }
                return cell;
            }
            else
            {
                var cell = (CustomEntryCell)tableView.DequeueReusableCell("EntryCell");
                if (cell == null)
                {
                    Console.WriteLine("null cell");
                }
                var entry = _entryList[indexPath.Row - 1];
                cell.SetEntry(entry);
                cell.OnPath += Cell_OnPath;
                cell.OnYoutube += Cell_OnYoutube;
                cell.SelectionStyle = UITableViewCellSelectionStyle.None;
                return cell;
            }
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (indexPath.Row == 0 && _moreData != null)
            {
                Topic = new Topic { Path = _moreData.Path, Title = Topic.Title };
                SetEntryList();
            }
        }

        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            if (_showHeader)
            {
                return YoutubeVC.View;
            }
            else
            {
                return null;
            }
        }

        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            if (_showHeader)
            {
                return new CGSize(View.Frame.Size.Width, View.Frame.Size.Width * 9 / 16).Height;
            }
            else
            {
                return 0;
            }
        }

        void _pagingVC_OnPage(object sender, int e)
        {
            Console.WriteLine(e);
            SetEntryList(e);
        }

        void Cell_OnPath(object sender, string e)
        {
            var indexpath = TableView.IndexPathForCell((UITableViewCell)sender);
            var entry = _entryList[indexpath.Row - 1];
            var content = entry.ContentList.Find(o => o.InnerLinkPath == e);
            if (content != null)
            {
                var topic = new Topic { Title = content.InnerLinkTitle, Path = content.InnerLinkPath };
                if (topic.Path != null)
                {
                    var enrtyListVC = (EntryListVC)Storyboard.InstantiateViewController("EntryListViewController");
                    enrtyListVC.Topic = topic;
                    NavigationController.PushViewController(enrtyListVC, true);
                }
            }
        }

        void Cell_OnYoutube(object sender, string e)
        {
            //PresentPopoverView(YoutubeVC, (UITableViewCell)sender, new CGSize(View.Frame.Size.Width,View.Frame.Size.Width * 9 / 16));
            TableView.SectionHeaderHeight = YoutubeVC.View.Frame.Size.Height;
            _showHeader = true;
            TableView.ReloadData();
            YoutubeVC.LoadVideo(e);
        }

        void _youtubeVC_OnDismiss(object sender, UIView e)
        {
            new Task(() =>
            {
                System.Threading.Thread.Sleep(00);
                _showHeader = false;
                InvokeOnMainThread(() =>
                {
                    TableView.ReloadData();

                });
            }).Start();
        }


        void PresentPopoverView(UIViewController viewController, UIView sourceView, CGSize popoverSize)
        {
            var presentationController = PopoverDelegate.ConfigurePresentation(viewController);
            presentationController.SourceView = NavigationController.NavigationBar;
            //presentationController.SourceRect = sourceView.Bounds;
            presentationController.PermittedArrowDirections = UIPopoverArrowDirection.Up;
            viewController.PreferredContentSize = popoverSize;
            PresentViewController(viewController, true, null);
        }
    }
}