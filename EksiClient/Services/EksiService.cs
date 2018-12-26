using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using System.Linq;
using System.Net.Http;
using System.Net;

namespace EksiClient
{
    /// <summary>
    /// Eksi service.
    /// </summary>
    public class EksiService
    {
        static CookieContainer _cookieContainer { get; set; } = new CookieContainer();
        static HttpClientHandler _handler { get; set; } = new HttpClientHandler() { CookieContainer = _cookieContainer };
        static EksiHttpClient _client { get; set; } = new EksiHttpClient(_handler);

        /// <summary>
        /// Gets the channels.
        /// </summary>
        /// <returns>The channels.</returns>
        public static List<Channel> GetChannels()
        {
            var channelList = new List<Channel>();
            _client.Path = "/kanallar";
            var document = _client.GetDocument();
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
            _client.Path = path;
            var document = _client.GetDocument();
            var eksiContent = document.GetElementbyId("content-body");
            var topBar = document.GetElementbyId("top-bar");
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
            _client.Path = path;
            var document = _client.GetDocument();
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

        /// <summary>
        /// Login the specified userName and password.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <param name="password">Password.</param>
        public static bool Login(string userName, string password)
        {
            var token = GetToken();
            var formVariables = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("__RequestVerificationToken", token),
                new KeyValuePair<string, string>("ReturnUrl", "https://eksisozluk.com/"),
                new KeyValuePair<string, string>("UserName", userName),
                new KeyValuePair<string, string>("Password", password),
                new KeyValuePair<string, string>("RememberMe", "true")
            };
            var formContent = new FormUrlEncodedContent(formVariables);
            var response = AsyncHelper.RunSync(async () => await _client.PostAsync(_client.BaseAddress, formContent));
            var content = AsyncHelper.RunSync(async () => await response.Content.ReadAsStringAsync());
            bool loggedIn = GetLoginStatus();


            IEnumerable<Cookie> responseCookies = _cookieContainer.GetCookies(_client.BaseAddress).Cast<Cookie>();
            foreach (Cookie cookie in responseCookies)
            {
                System.Diagnostics.Debug.WriteLine(cookie.Name + ": " + cookie.Value);

            }

            return loggedIn;
        }

        /// <summary>
        /// Gets the login status.
        /// </summary>
        /// <returns><c>true</c>, if login status was gotten, <c>false</c> otherwise.</returns>
        public static bool GetLoginStatus()
        {
            _client.Path = "/giris";
            var testDocument = _client.GetDocument();
            var testContent = testDocument.GetElementbyId("top-bar");
            var status = testContent.Attributes["class"].Value;
            if (status == "loggedin")
            {
                System.Diagnostics.Debug.WriteLine("loggedin");
                return true;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("loggedoff");
                return false;
            }
        }

        /// <summary>
        /// Gets the token.
        /// </summary>
        /// <returns>The token.</returns>
        static string GetToken()
        {
            _client.Path = "/giris";
            var document = _client.GetDocument();
            var eksiContent = document.GetElementbyId("content-body");
            var eksiFormContainer = eksiContent.Descendants("div").First(o => o.HasClass("form-container"));
            var form = eksiFormContainer.Descendants("form").First();
            var tokenInput = form.Descendants("input").First();
            var token = tokenInput.Attributes["value"].Value;
            return token;
        }
    }
}
