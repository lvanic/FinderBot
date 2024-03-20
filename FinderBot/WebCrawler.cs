using HtmlAgilityPack;
using System.Collections.Concurrent;
using System.Linq;

namespace FinderBot
{
    public class WebCrawler
    {
        private readonly HttpClient _httpClient;
        private readonly HashSet<string> _visitedUrls;
        private readonly ConcurrentQueue<string> _urlsToCrawl;
        private int _maxConcurrency;
        private int _currentConcurrency;

        public WebCrawler(int maxConcurrency)
        {
            _httpClient = new HttpClient();
            _visitedUrls = new HashSet<string>();
            _urlsToCrawl = new ConcurrentQueue<string>();
            _maxConcurrency = maxConcurrency;
        }

        public async Task CrawlAsync(string startUrl, int maxPages)
        {
            _urlsToCrawl.Enqueue(startUrl);
            _visitedUrls.Add(startUrl);

            var crawlTasks = new List<Task>();

            while (_urlsToCrawl.Count > 0 && _visitedUrls.Count < maxPages)
            {
                if (_currentConcurrency < _maxConcurrency)
                {
                    if (_urlsToCrawl.TryDequeue(out string url))
                    {
                        Interlocked.Increment(ref _currentConcurrency);

                        crawlTasks.Add(Task.Run(async () =>
                        {
                            await ProcessPageAsync(url);

                            Interlocked.Decrement(ref _currentConcurrency);
                        }));
                    }
                }

                await Task.WhenAny(crawlTasks);
                crawlTasks.RemoveAll(t => t.IsCompleted);
            }
        }

        private async Task ProcessPageAsync(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();

                // Здесь можно обрабатывать содержимое страницы
                // Например, извлекать ссылки и добавлять их в очередь для дальнейшего сканирования

                var links = ExtractLinks(content);
                if (links != Enumerable.Empty<string>())
                {
                    foreach (var link in links)
                    {
                        if (!_visitedUrls.Contains(link))
                        {
                            _urlsToCrawl.Enqueue(link);
                            _visitedUrls.Add(link);
                        }
                    }
                    Console.WriteLine($"Processed page: {url}");
                }
                else
                {
                    Console.WriteLine($"No links at URL: {url}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing page {url}: {ex.Message}");
            }
        }

        private IEnumerable<string> ExtractLinks(string htmlContent)
        {
            var document = new HtmlDocument();
            document.LoadHtml(htmlContent);

            var links = document.DocumentNode.Descendants("a")
                .Select(a => a.GetAttributeValue("href", string.Empty))
                .Where(href => !string.IsNullOrEmpty(href))
                
                .Distinct();

            return links;
        }
    }
}
