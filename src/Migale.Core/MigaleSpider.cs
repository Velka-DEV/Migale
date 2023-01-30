using Migale.Core.Collections;
using Migale.Core.Crawlers;
using Migale.Core.Events;
using Migale.Core.Models;

namespace Migale.Core;

public class MigaleSpider
{
    /// <summary>
    /// Configuration of the spider
    /// </summary>
    public MigaleOptions Options { get; }
    
    /// <summary>
    /// ShouldCrawl delegate is executed and the result is used to determine if the page should be crawled.
    /// Always bypass if not set.
    /// </summary>
    public ShouldCrawlDelegate? ShouldCrawl { get; set; }
    public delegate bool ShouldCrawlDelegate(HttpTarget target);

    /// <summary>
    /// Triggered on the start of the crawl
    /// </summary>
    public event EventHandler<RequestEventArgs>? PageCrawlStarting;
    
    /// <summary>
    /// Triggered on the completion of the crawl
    /// </summary>
    public event EventHandler<ResponseEventArgs>? PageCrawled;
    
    /// <summary>
    /// Fired when the crawl of a page failed
    /// </summary>
    public event EventHandler<RequestFailedEventArgs>? PageCrawlFailed; 

    private CancellationTokenSource? _cancellationTokenSource;
    private readonly ObjectPool<HttpTarget> _targets;
    private readonly ObjectPool<ICrawler> _crawlers;
    private readonly List<Task> _workers;
    
    internal MigaleSpider(MigaleOptions options, IEnumerable<ICrawler> crawlers)
    {
        Options = options;

        _targets = new ObjectPool<HttpTarget>();
        _crawlers = new ObjectPool<ICrawler>(crawlers);
        _workers = new List<Task>(options.Threads);
    }
    
    public async Task StartAsync()
    {
        if (_targets.Count == 0)
            throw new ArgumentException("Empty targets list");

        _cancellationTokenSource = new CancellationTokenSource();
        
        for (var i = 0; i < Options.Threads; i++)
        {
            var thread = Task.Run(() => WorkerLoopAsync(_cancellationTokenSource.Token));
            _workers.Add(thread);
        }
    }

    private async Task<CrawResult> InternalCrawlAsync(ICrawler crawler, HttpTarget target, CancellationToken token = default, int retries = 0)
    {
        try
        {
            var page = await crawler.CrawlAsync(target, token);
            
            return new CrawResult(page);
        }
        catch (Exception e)
        {
            if (e is OperationCanceledException)
                return new CrawResult(e, true);

            if (Options.MaxRetries > retries)
            {
                return await InternalCrawlAsync(crawler, target, token, retries + 1);
            }

            return new CrawResult(e);
        }
    }
    
    private async Task WorkerLoopAsync(CancellationToken token = default)
    {
        while(!token.IsCancellationRequested)
        {
            var crawler = _crawlers.NextItem();
            var target = _targets.NextItem(true);

            if (crawler is null || target is null)
                continue;
            
            if (!(ShouldCrawl?.Invoke(target) ?? true))
                continue;
            
            PageCrawlStarting?.Invoke(this, new RequestEventArgs()
            {
                Target = target
            });
            
            var result = await InternalCrawlAsync(crawler, target, token);

            if (!result.Success)
            {
                PageCrawlFailed?.Invoke(this, new RequestFailedEventArgs()
                {
                    Target = target,
                    Result = result
                });
                continue;
            }
            
            PageCrawled?.Invoke(this, new ResponseEventArgs()
            {
                Target = target,
                Result = result
            });
        }
    }
    
    
    public void AddTarget(string url) => AddTarget(new HttpTarget(HttpMethod.Get, url));

    public void AddTarget(HttpTarget target) => _targets.Add(target);
}