using Microsoft.Playwright;
using Migale.Core.Crawlers;
using Migale.Core.Models;

namespace Migale.Playwright;

public class PlaywrightCrawler : ICrawler, IAsyncDisposable
{
    private readonly IBrowserContext _context;
    
    public PlaywrightCrawler(IBrowserContext context)
    {
        _context = context;
    }
    
    public async Task<string?> CrawlAsync(HttpTarget target, CancellationToken token = default)
    {
        var page = await _context.NewPageAsync();

        if (target.Method == HttpMethod.Get)
        {
            var response = await page.GotoAsync(target.Url);

            if (response is null) return string.Empty;

            var content = await response.TextAsync();

            await page.CloseAsync();

            return content;
        }

        throw new NotImplementedException("Only GET method is supported at the moment");
    }
    
    public async ValueTask DisposeAsync()
    {
        await _context.CloseAsync();
    }
}