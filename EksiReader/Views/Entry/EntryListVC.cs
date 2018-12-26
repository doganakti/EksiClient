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

        List<Entry> _entryList;
        /// <summary>
        /// Gets or sets the entry list.
        /// </summary>
        /// <value>The entry list.</value>
        List<Entry> EntryList
        {
            get
            {
                if (Topic != null && _entryList == null)
                {
                    Task.Run(() =>
                    {
                        _entryList = EksiService.GetEntries(Topic.Path);
                        foreach(var entry in _entryList)
                        {
                            entry.AttributedString = Common.GetAttributedString(entry.ContentList);
                        }
                        BeginInvokeOnMainThread(TableView.ReloadData);
                    });
                }
                return _entryList;
            }
            set
            {
                _entryList = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:EksiReader.EntryListVC"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
        public EntryListVC (IntPtr handle) : base (handle)
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
            TableView.SectionHeaderHeight = UITableView.AutomaticDimension;
            TableView.EstimatedSectionHeaderHeight = 44;
            TableView.SeparatorColor = Common.Template.LinkColor.ColorFromHEX().ColorWithAlpha(0.2f);
            TableView.BackgroundColor = Common.Template.BackgroundColor.ColorFromHEX();
            TableView.AllowsSelection = false;
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
        /// Gets the view for header.
        /// </summary>
        /// <returns>The view for header.</returns>
        /// <param name="tableView">Tabl    e view.</param>
        /// <param name="section">Section.</param>
        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
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
            return textView;
        }

        public override nfloat GetHeightForFooter(UITableView tableView, nint section)
        {
            return 0.1f;
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
    }
}