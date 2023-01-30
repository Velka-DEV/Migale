using Migale.Core.Crawlers;
using Migale.Core.Models;

namespace Migale.Core;

public class MigaleBuilder
{
    private readonly List<ICrawler> _crawlers = new();
    private MigaleOptions? _options = new();

    /// <summary>
    /// Configure the Migale options.
    /// </summary>
    public MigaleBuilder WithOptions(MigaleOptions options)
    {
        _options = options;
        return this;
    }
    
    /// <summary>
    ///  Add a crawler to the list of crawlers to be used by Migale. The type of the crawler must be a class that implements the <see cref="ICrawler"/> interface.
    /// </summary>
    /// <typeparam name="T">Type implementing the ICrawler interface</typeparam>
    public MigaleBuilder WithCrawler<T>() where T : ICrawler, new()
    {
        _crawlers.Add(new T());
        return this;
    }
    
    /// <summary>
    /// Add a crawler to the list of crawlers to be used by Migale. The type of the crawler must be a class that implements the <see cref="ICrawler"/> interface.
    /// </summary>
    /// <param name="crawlerFactory">A factory that return an <see cref="ICrawler"/> instance</param>
    public MigaleBuilder WithCrawler(Func<ICrawler> crawlerFactory)
    {
        _crawlers.Add(crawlerFactory.Invoke());
        return this;
    }
    
    /// <summary>
    /// Add a crawler to the list of crawlers to be used by Migale. The type of the crawler must be a class that implements the <see cref="ICrawler"/> interface.
    /// </summary>
    public MigaleBuilder WithCrawler(ICrawler crawler)
    {
        _crawlers.Add(crawler);
        return this;
    }
    
    /// <summary>
    /// Add a list of crawlers to the list of crawlers to be used by Migale. The type of the crawler must be a class that implements the <see cref="ICrawler"/> interface.
    /// </summary>
    public MigaleBuilder WithCrawlers(IEnumerable<ICrawler> crawlers)
    {
        _crawlers.AddRange(crawlers);
        return this;
    }

    /// <summary>
    /// Build the MigaleSpider using the provided options and crawlers.
    /// </summary>
    /// <returns>A ready to start MigaleSpider instance</returns>
    /// <exception cref="InvalidOperationException">Occur when you try to build with incorrect configuration</exception>
    public MigaleSpider Build()
    {
        if (_crawlers.Count == 0)
            throw new InvalidOperationException("No crawlers were added to the builder.");

        if (_options is null)
            throw new InvalidOperationException("You can't start the crawler without options");
        
        if (_options.Threads <= 0)
            throw new InvalidOperationException("You can't start the crawler with 0 or less threads.");

        return new MigaleSpider(_options, _crawlers);
    }
}