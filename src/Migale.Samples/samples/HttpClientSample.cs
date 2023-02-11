using Migale.Core;
using Migale.Crawlers.HttpClient.Extensions;

namespace Migale.Samples.samples;

public class HttpClientSample
{
    public static async Task RunHttpClientMigaleAsync()
    {
        var builder = new MigaleBuilder();

        builder.WithHttpClientCrawler();
        
        var spider = builder.Build();
        
        spider.SendingRequest += (sender, e) =>
        {
            Console.WriteLine($"Crawling {e.Target.Url}");
        };
        
        spider.ResponseReceived += (sender, e) =>
        {
            Console.WriteLine($"Crawled {e.Target.Url}");
        };
        
        spider.RequestFailed += (sender, e) =>
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