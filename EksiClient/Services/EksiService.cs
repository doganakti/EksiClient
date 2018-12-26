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
            foreach(var item in list)
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
            foreach(var item in list)
            {
                var link = item.Descendants("a").First().Attributes["href"];
                var title = item.InnerText.Trim();
                var topic = new Topic { Title = title, Path = link != null ? link.Value : null};
                topicList.Add(topic);
            }
            return topicList;
        }
    }
}
