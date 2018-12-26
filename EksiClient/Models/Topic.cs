using System;
namespace EksiClient
{
    /// <summary>
    /// Topic.
    /// </summary>
    public class Topic
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>The path.</value>
        public string Path { get; set; }

        string _badge;
        public string Badge
        {
            get
            {
                if (_badge == null)
                {
                    var stringArray = Title.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    var lastItem = stringArray[stringArray.Length - 1];
                    int badge = 0;
                    Int32.TryParse(lastItem, out badge);
                    _badge = badge == 0 ? "" : badge.ToString();
                    if (_badge != "")
                    {
                        Title = Title.Remove(Title.Length - _badge.Length);
                    }
                }
                return _badge;
            }
        }
    }
}
