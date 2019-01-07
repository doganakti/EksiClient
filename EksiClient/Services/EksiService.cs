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
        static bool LoggedIn;

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
        public static Result<Entry> GetEntries(string path, int page = 0, HtmlDocument document = null)
        {
            var result = new Result<Entry>();
            try
            {
                var sub = path.Substring(0, 4);
                if (sub == "/?q=" && page != 0)
                {
                    path = path.Replace("/?q=", "/");
                }
                if (path.Contains("?") && page != 0)
                {
                    path = path + $"&p={page}";
                }
                else if (!path.Contains("?") && page != 0)
                {
                    path = path + $"?p={page}";
                }

                var entryList = new List<Entry>();
                if (document == null)
                {
                    _client.Path = path;
                    document = _client.GetDocument();
                }
                var eksiContent = document.GetElementbyId("entry-item-list");
                var list = eksiContent.ChildNodes.ToList().FindAll(o => o.Name == "li");

                var pageResult = GetPager(document);
                if (pageResult.ResultList.HasItem())
                {
                    var pager = pageResult.ResultList.First();
                    result.Pager = pager;
                }

                var moreResult = GetMore(document);
                if (moreResult.ResultList.HasItem())
                {
                    var moreData = moreResult.ResultList.First();
                    result.MoreData = moreData;
                }

                var topicResult = GetTopic(document);
                if (topicResult.ResultList.HasItem())
                {
                    var query = HttpUtility.ParseQueryString(_client.BaseAddress.Query);
                    var topic = topicResult.ResultList.First();
                    foreach(var q in query)
                    {
                        var index = query.IndexOf(q);
                        if (q.Key != "p" && q.Key != "focusto")
                        {
                            if (index == 0)
                            {
                                topic.Path = topic.Path + $"?{q.Key}={q.Value}";
                            }
                            else
                            {
                                topic.Path = topic.Path + $"&{q.Key}={q.Value}";
                            }
                        }
                    }
                    result.Topic = topic;
                }

                foreach (var item in list)
                {
                    var link = item.Descendants("a").First().Attributes["href"];
                    var content = item.ChildNodes.First(o => o.Name == "div");
                    var text = "";
                    var entryContentList = new List<EntryContent>();
                    foreach (var childNode in content.ChildNodes)
                    {
                        if (childNode.Name == "#text")
                        {
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
                result.ResultList = entryList;
            }
            catch(Exception ex)
            {
                ExceptionHelper.Write(typeof(EksiService), ex);
                result.Status = Status.Fail;
            }

            return result;
        }

        /// <summary>
        /// Gets the pager.
        /// </summary>
        /// <returns>The pager.</returns>
        /// <param name="document">Document.</param>
        public static Result<Pager> GetPager(HtmlDocument document)
        {
            var result = new Result<Pager>();
            try
            {
                var eksiTopic = document.GetElementbyId("topic");
                var eksiContainer = eksiTopic.Descendants("div").First(o => o.HasClass("sub-title-container"));
                var eksiPagerList = eksiContainer.Descendants("div").ToList();
                var eksiPager = eksiPagerList.Find(o => o.HasClass("pager"));
                if (eksiPager != null)
                {
                    var currentPage = eksiPager.Attributes["data-currentpage"].Value;
                    var pageCount = eksiPager.Attributes["data-pagecount"].Value;
                    var pager = new Pager { PageCount = Int32.Parse(pageCount), CurrentPage = Int32.Parse(currentPage) };
                    result.ResultList = pager.AsList();
                }
            }
            catch(Exception ex)
            {
                ExceptionHelper.Write(typeof(EksiService), ex);
                result.Status = Status.Fail;
            }
            return result;
        }

        /// <summary>
        /// Gets the topic.
        /// </summary>
        /// <returns>The topic.</returns>
        /// <param name="document">Document.</param>
        public static Result<Topic>GetTopic(HtmlDocument document)
        {
            var result = new Result<Topic>();
            try
            {
                var eksiTopic = document.GetElementbyId("topic");
                var eksiContainer = eksiTopic.Descendants("h1").First(o => o.Id == "title");
                var eksiTitle = eksiContainer.Descendants("a").First();

                if (eksiTitle != null)
                {
                    var path = eksiTitle.Attributes["href"].Value;
                    var title = eksiTitle.InnerText.Trim();
                    var topic = new Topic
                    {
                        Path = path,
                        Title = title
                    };
                    result.ResultList = topic.AsList();
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.Write(typeof(EksiService), ex);
                result.Status = Status.Fail;
            }
            return result;
        }

        /// <summary>
        /// Gets the more.
        /// </summary>
        /// <returns>The more.</returns>
        public static Result<MoreData> GetMore(HtmlDocument document)
        {
            var result = new Result<MoreData>();
            try
            {
                var eksiTopic = document.GetElementbyId("topic");
                var eksiMoreData = eksiTopic.Descendants("a").First(o => o.HasClass("more-data"));
                if (eksiMoreData != null)
                {
                    var path = eksiMoreData.Attributes["href"].Value;
                    var title = eksiMoreData.InnerText.Trim();
                    var moreData = new MoreData { Title = title, Path = path };
                    result.ResultList = moreData.AsList();
                }
            }
            catch
            {
                result.Status = Status.Fail;
            }
            return result;
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

            if (loggedIn)
            {
                LoggedIn = true;
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

        /// <summary>
        /// Gets the search topic.
        /// </summary>
        /// <returns>The search topic.</returns>
        /// <param name="input">Input.</param>
        public static Topic GetSearchTopic(string input)
        {
            var searchArray = input.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
            var searchString = "";
            if (searchArray.Count() > 0)
            {
                foreach (var searchItem in searchArray)
                {
                    if (searchArray.IndexOf(searchItem) == 0)
                    {
                        searchString = searchItem;
                    }
                    else
                    {
                        searchString = searchString + "+" + searchItem;
                    }
                }
            }
            Topic searchTopic = new Topic { Path = $"/?q={searchString}", Title = input };
            return searchTopic;
        }
    }
}
