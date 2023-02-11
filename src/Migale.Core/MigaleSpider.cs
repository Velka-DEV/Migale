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
    public event EventHandler<RequestEventArgs>? SendingRequest;
    
    /// <summary>
    /// Triggered on the completion of the crawl
    /// </summary>
    public event EventHandler<ResponseEventArgs>? ResponseReceived;
    
    /// <summary>
    /// Fired when the crawl of a page failed
    /// </summary>
    public event EventHandler<RequestFailedEventArgs>? RequestFailed; 

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
    
    /// <summary>
    /// Start the spider, if no targets are added, an exception will be thrown.
    /// This might change as the spider don't actually need targets to start and don't stop when all target has been processed.
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
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

    /// <summary>
    /// Stop the spider and await tasks to finish, if the spider is not running, an exception will be thrown.
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task StopAsync()
    {
        if (_cancellationTokenSource is null || _workers.Count == 0 || _cancellationTokenSource.IsCancellationRequested)
            throw new InvalidOperationException("Spider is not running");
        
        _cancellationTokenSource.Cancel();
        
        await Task.WhenAll(_workers);
    }

    private async Task<CrawResult> InternalCrawlAsync(ICrawler crawler, HttpTarget target, int retries = 0, CancellationToken token = default)
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
                return await InternalCrawlAsync(crawler, target, retries + 1, token);
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
            
            SendingRequest?.Invoke(this, new RequestEventArgs()
            {
                Target = target
            });
            
            var result = await InternalCrawlAsync(crawler, target, token: token);

            if (!result.Success)
            {
                RequestFailed?.Invoke(this, new RequestFailedEventArgs()
                {
                    Target = target,
                    Result = result
                });
                continue;
            }
            
            ResponseReceived?.Invoke(this, new ResponseEventArgs()
            {
                Target = target,
                Result = result
            });
        }
    }
    
    /// <summary>
    /// Proxy for <see cref="AddTarget(HttpTarget)"/>. The method will create a new <see cref="HttpTarget"/> with the given url and the default <see cref="HttpMethod.Get"/>
    /// </summary>
    /// <param name="url"></param>
    public void AddTarget(string url) => AddTarget(new HttpTarget(HttpMethod.Get, url));

    /// <summary>
    /// Add a target to the spider
    /// </summary>
    /// <param name="target"></param>
    public void AddTarget(HttpTarget target) => _targets.Add(target);
}