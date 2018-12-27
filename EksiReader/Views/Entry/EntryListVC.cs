using EksiClient;
using Foundation;
using ObjCRuntime;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UIKit;

namespace EksiReader
{
    public partial class EntryListVC : UITableViewController
    {
        public Topic Topic { get; set; }

        Pager _pager;

        List<Entry> _entryList;
        /// <summary>
        /// Gets or sets the entry list.
        /// </summary>
        /// <value>The entry list.</value>
        List<Entry> EntryList
        {
            get
            {
                return _entryList;
            }
            set
            {
                _entryList = value;
            }
        }

        PagingVC _pagingVC;
        PagingVC PagingVC
        {
            get
            {
                if (_pagingVC == null)
                {
                    _pagingVC = (PagingVC)Storyboard.InstantiateViewController("PagingViewController");
                    _pagingVC.OnNext += _pagingVC_OnNext;
                    _pagingVC.OnPrevious += _pagingVC_OnPrevious;
                }
                return _pagingVC;
            }
        }

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
            TableView.AllowsSelection = false;
            //Title = Topic.Title;
            UITextView textView = new UITextView
            {
                ScrollEnabled = false,
                Text = Topic.Title,
                Font = UIFont.FromName(Common.Template.LargeBarFontName, Common.Template.LargeBarFontSize),
                TextColor = Common.Template.BarFontColor.ColorFromHEX(),
                BackgroundColor = Common.Template.BackgroundColor.ColorFromHEX(),
                TextContainerInset = new UIEdgeInsets(9, 9, 9, 9),
                Editable = false,
                Selectable = false
            };
            textView.SizeToFit();
            var header = new UIView(new CoreGraphics.CGRect(0, 0, 300, 200));
            TableView.TableHeaderView = textView;
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
                foreach (var entry in _entryList)
                {
                    entry.AttributedString = Common.GetAttributedString(entry.ContentList);
                }
                BeginInvokeOnMainThread(() =>
                {
                    TableView.ReloadData();
                    PagingVC.Pager = _pager;
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
            return EntryList != null ? EntryList.Count : 0;
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
            var cell = (CustomEntryCell)tableView.DequeueReusableCell("CustomEntryCell", indexPath);
            var entry = EntryList[indexPath.Row];
            cell.SetEntry(entry);
            return cell;
        }

        void _pagingVC_OnNext(object sender, int e)
        {
            Console.WriteLine(e);
            SetEntryList(e);
        }

        void _pagingVC_OnPrevious(object sender, int e)
        {
            Console.WriteLine(e);
            SetEntryList(e);
        }

    }
}