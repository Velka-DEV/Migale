using Migale.Core.Models;

namespace Migale.Core.Crawlers;

public interface ICrawler
{
    /// <summary>
    /// This method is executed for each target and should return the response body as a string
    /// </summary>
    /// <param name="target"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<string?> CrawlAsync(HttpTarget target, CancellationToken token = default);
}