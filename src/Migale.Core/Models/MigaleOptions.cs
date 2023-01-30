namespace Migale.Core.Models;

public sealed class MigaleOptions
{
    /// <summary>
    /// Represents the number of parallel threads to use when processing the HttpTargets.
    /// </summary>
    public int Threads { get; set; } = 10;

    /// <summary>
    /// Number of times to retry a failed crawl.
    /// </summary>
    public int MaxRetries { get; set; } = 3;
    
    /// <summary>
    /// Follow redirects.
    /// </summary>
    public bool AllowRedirects { get; set; } = true;

    /// <summary>
    /// Allowed domains to crawl. If empty, all domains are allowed.
    /// </summary>
    public IEnumerable<string>? AllowedDomains { get; set; }
}