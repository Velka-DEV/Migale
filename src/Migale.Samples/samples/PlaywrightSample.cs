using Migale.Core;
using Migale.Core.Models;
using Migale.Crawlers.Playwright.Extensions;
using Migale.Crawlers.Playwright.Models;

namespace Migale.Samples.samples;

public class PlaywrightSample
{
    public static async Task RunPlaywrightMigaleAsync()
    {
        var exitCode = Microsoft.Playwright.Program.Main(new[] {"install"});
        if (exitCode != 0)
        {
            throw new Exception($"Playwright exited with code {exitCode}");
        }

        var builder = new MigaleBuilder();

        builder.WithPlaywrightCrawler(new PlaywrightCrawlerOptions()
        {
            Headless = false,
            BrowserInstances = 2
        });
        builder.WithOptions(new MigaleOptions()
        {
            Threads = 2
        });
        
        var spider = builder.Build();
        
        spider.PageCrawlStarting += (sender, e) =>
        {
            Console.WriteLine($"Crawling {e.Target.Url}");
        };
        
        spider.PageCrawled += (sender, e) =>
        {
            Console.WriteLine($"Crawled {e.Target.Url}");
            Console.WriteLine(e.Result.Content);
        };
        
        spider.PageCrawlFailed += (sender, e) =>
        {
            Console.WriteLine($"Failed to crawl {e.Target.Url}");
        };
        
        for(var i = 0; i < 10; i++)
        {
            spider.AddTarget("http://example.com");
        }
        
        await spider.StartAsync();
        
        Console.WriteLine("Press any key to exit");
        
        Console.ReadKey();
    }
}