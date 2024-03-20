using FinderBot;

const int MAX_CONCURRENCY = 10;
const string SRART_URL = "https://www.onliner.by";
const int MAX_PAGE = 2000000;

var crawler = new WebCrawler(MAX_CONCURRENCY);

await crawler.CrawlAsync(SRART_URL, MAX_PAGE);