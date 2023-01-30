using Migale.Core.Crawlers;
using Migale.Core.Models;

namespace Migale.Crawlers.HttpClient;

public class HttpClientCrawler : ICrawler
{
    private readonly System.Net.Http.HttpClient _httpClient;

    public HttpClientCrawler() : this(new System.Net.Http.HttpClient()) { }
    
    public HttpClientCrawler(System.Net.Http.HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<string?> CrawlAsync(HttpTarget target, CancellationToken token = default)
    {
        using var request = target.BuildRequest();
        using var response = await _httpClient.SendAsync(request, token);
        return await response.Content.ReadAsStringAsync(token);
    }
}