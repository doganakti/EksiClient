using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EksiClient;
using Foundation;
using Newtonsoft.Json;
using UIKit;

namespace EksiReader
{
    /// <summary>
    /// Common.
    /// </summary>
    public class Common
    {
        /// <summary>
        /// Gets the system version.
        /// </summary>
        /// <returns>The system version.</returns>
        public static float SystemVersion
        {
            get
            {
                var versionString = UIDevice.CurrentDevice.SystemVersion;
                float version = 0;
                try
                {
                    var splitted = versionString.Split(".".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (splitted != null && splitted.ToList().Count > 0)
                    {
                        if (splitted.ToList().Count > 1)
                        {
                            var fused = splitted[0] + "." + splitted[1];
                            version = float.Parse(fused);
                        }
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return version;
            }
        }

        static Dictionary<string, Template> _templateDictionary;
        /// <summary>
        /// Gets the template dictionary.
        /// </summary>
        /// <value>The template dictionary.</value>
        public static Dictionary<string, Template> TemplateDictionary
        {
            get
            {
                if (_templateDictionary == null)
                {
                    var path = NSBundle.MainBundle.PathForResource("Template", "json");
                    var json = File.ReadAllText(path);
                    _templateDictionary = JsonConvert.DeserializeObject<Dictionary<string, Template>>(json);
                }
                return _templateDictionary;
            }
        }

        /// <summary>
        /// Gets the template.
        /// </summary>
        /// <value>The template.</value>
        public static Template Template
        {
            get
            {
                return TemplateDictionary["Sepia"];
            }
        }

        /// <summary>
        /// Gets the attributed string.
        /// </summary>
        /// <returns>The attributed string.</returns>
        /// <param name="contentList">Content list.</param>
        public static NSAttributedString GetAttributedString(List<EntryContent> contentList)
        {
            var attributedString = new NSMutableAttributedString();
            foreach(var content in contentList)
            {
                if (content.Text != null)
                {
                    attributedString.Append(
                        new NSAttributedString(content.Text + " ")
                    );
                }
                else if (content.Break != false)
                {
                    attributedString.Append(
                        new NSAttributedString("\n")
                    );
                }
                else if (content.LinkTitle != null)
                {
                    attributedString.Append(
                    new NSAttributedString(content.LinkTitle + "~", new UIStringAttributes
                    {
                        ForegroundColor = UIColor.Brown,
                        Link = NSUrl.FromString(content.LinkPath)
                    })
                    );
                }
                else if (content.InnerLinkTitle != null)
                {
                    attributedString.Append(
                    new NSAttributedString(content.InnerLinkTitle, new UIStringAttributes
                    {
                        ForegroundColor = UIColor.Brown,
                        Link = NSUrl.FromString(content.InnerLinkPath)
                    })
                    );
                }
            }
            return attributedString;
        }
    }
}
