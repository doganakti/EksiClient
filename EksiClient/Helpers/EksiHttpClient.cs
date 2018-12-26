using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace EksiClient
{
    /// <summary>
    /// Eksi http client.
    /// </summary>
    public class EksiHttpClient : HttpClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:EksiClient.EksiHttpClient"/> class.
        /// </summary>
        public EksiHttpClient(HttpMessageHandler handler):base(handler)
        {
            DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Macintosh; Intel Mac OS X 10_10_1)");
            DefaultRequestHeaders.UserAgent.ParseAdd("AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.95 Safari/537.36");
        }


        string _path;
        public string Path 
        { 
            get
            {
                return _path;
            }
            set
            {
                _path = "https://eksisozluk.com" + value;
                BaseAddress = new Uri(_path);
            }
        }

        /// <summary>
        /// Gets the document async.
        /// </summary>
        /// <returns>The document async.</returns>
        public async Task<HtmlDocument> GetDocumentAsync()
        {
            try
            {
                var content = await GetStringAsync(this.BaseAddress);
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(content);
                return htmlDocument;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the document.
        /// </summary>
        /// <returns>The document.</returns>
        public HtmlDocument GetDocument()
        {
            var content = AsyncHelper.RunSync(async () => await GetDocumentAsync());
            return content;
        }
    }
}
