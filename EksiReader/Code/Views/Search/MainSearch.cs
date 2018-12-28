using Foundation;
using System;
using UIKit;

namespace EksiReader
{
    public partial class MainSearch : UISearchBar
    {
        public event EventHandler<string> OnSearch;

        public MainSearch (IntPtr handle) : base (handle)
        {
            SearchButtonClicked += MainSearch_SearchButtonClicked;
        }

        void MainSearch_SearchButtonClicked(object sender, EventArgs e)
        {
            Console.WriteLine("here");
            if (OnSearch != null)
            {
                OnSearch.Invoke(this, ((UISearchBar)sender).Text);
            }
        }

    }
}