using System;
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
        /// <param name="path">Path.</param>
        public EksiHttpClient(string path, int page = 0)
        {
            DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Macintosh; Intel Mac OS X 10_10_1)");
            DefaultRequestHeaders.UserAgent.ParseAdd("AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.95 Safari/537.36");
            path = "https://eksisozluk.com" + path + (page == 0 ? "" : $"?p={page}");
            BaseAddress = new Uri(path);
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
