using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using System.Linq;

namespace EksiClient
{
    /// <summary>
    /// Eksi service.
    /// </summary>
    public class EksiService
    {
        /// <summary>
        /// Gets the channels.
        /// </summary>
        /// <returns>The channels.</returns>
        public static List<Channel> GetChannels()
        {
            var channelList = new List<Channel>();
            var client = new EksiHttpClient("/kanallar");
            var document = client.GetDocument();
            var eksiContent = document.GetElementbyId("content-body");
            var ul = eksiContent.ChildNodes.ToList().Find(o => o.Name == "ul");
            var list = ul.ChildNodes.ToList().FindAll(o => o.Name == "li");
            foreach (var item in list)
            {
                var link = item.Descendants("a").First().Attributes["href"];
                var title = item.Descendants("a").First().InnerText.Trim();
                var channel = new Channel { Title = title, Path = link != null ? link.Value : null };
                channelList.Add(channel);
            }
            return channelList;
        }

        /// <summary>
        /// Gets the topics.
        /// </summary>
        /// <returns>The topics.</returns>
        /// <param name="path">Path.</param>
        /// <param name="page">Page.</param>
        public static List<Topic> GetTopics(string path = "/basliklar/gundem", int page = 0)
        {
            var topicList = new List<Topic>();
            var client = new EksiHttpClient(path, page);
            var document = client.GetDocument();
            var eksiContent = document.GetElementbyId("content-body");
            var ul = eksiContent.ChildNodes.ToList().Find(o => o.Name == "ul");
            var list = ul.ChildNodes.ToList().FindAll(o => o.Name == "li");
            foreach (var item in list)
            {
                var link = item.Descendants("a").First().Attributes["href"];
                var title = item.InnerText.Trim();
                var topic = new Topic { Title = title, Path = link != null ? link.Value : null };
                topicList.Add(topic);
            }
            return topicList;
        }

        /// <summary>
        /// Gets the entries.
        /// </summary>
        /// <returns>The entries.</returns>
        /// <param name="path">Path.</param>
        /// <param name="page">Page.</param>
        public static List<Entry> GetEntries(string path, int page = 0)
        {
            var entryList = new List<Entry>();
            var client = new EksiHttpClient(path, page);
            var document = client.GetDocument();
            var eksiContent = document.GetElementbyId("entry-item-list");
            var list = eksiContent.ChildNodes.ToList().FindAll(o => o.Name == "li");
            foreach (var item in list)
            {
                var link = item.Descendants("a").First().Attributes["href"];
                var content = item.ChildNodes.First(o => o.Name == "div");
                var text = "";
                var entryContentList = new List<EntryContent>();
                foreach(var childNode in content.ChildNodes)
                {
                    if (childNode.Name == "#text")
                    {
                        if (childNode.InnerText.Contains("bkz:"))
                        {
                            System.Diagnostics.Debug.WriteLine("here");
                        }
                        text = text + childNode.InnerText.Trim();
                        entryContentList.Add(new EntryContent { Text = childNode.InnerText.Trim() });
                    }
                    else if (childNode.Name == "br")
                    {
                        text = text + "\n";
                        entryContentList.Add(new EntryContent { Break = true });
                    }
                    else if (childNode.Name == "a")
                    {
                        text = text + childNode.InnerText.Trim();
                        var linkPath = childNode.Attributes["href"].Value;
                        if (linkPath.Substring(0, 1).Contains("/"))
                        {
                            entryContentList.Add(new EntryContent { InnerLinkPath = linkPath, InnerLinkTitle = childNode.InnerText.Trim() });
                        }
                        else
                        {
                            entryContentList.Add(new EntryContent { LinkPath = linkPath, LinkTitle = childNode.InnerText.Trim() });
                        }
                    }
                }
                var footer = item.ChildNodes.First(o => o.Name == "footer");
                var infoDiv = footer.Descendants("div").First(o => o.HasClass("info"));
                var authorDiv = infoDiv.Descendants("a").First(o => o.HasClass("entry-author"));
                var dateDiv = infoDiv.Descendants("a").First(o => o.HasClass("entry-date"));

                var entry = new Entry
                {
                    Content = text,
                    Author = authorDiv.InnerText.Trim(),
                    AuthorPath = authorDiv.Attributes["href"].Value,
                    Date = dateDiv.InnerText.Trim(),
                    ContentList = entryContentList
                };
                entryList.Add(entry);
            }
            return entryList;
        }
    }
}
