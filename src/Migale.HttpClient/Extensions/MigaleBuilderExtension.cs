using Migale.Core;
using Migale.Core.Models;

namespace Migale.HttpClient.Extensions;

public static class MigaleBuilderExtensions
{
    public static MigaleBuilder WithHttpClientCrawler(this MigaleBuilder builder)
    {
        builder.WithCrawler<HttpClientCrawler>();
        
        return builder;
    }

    public static MigaleBuilder WithHttpClientCrawler(this MigaleBuilder builder, IEnumerable<Proxy> proxies)
    {
        return builder.WithCrawlers(proxies.Select(p => new HttpClientCrawler(new System.Net.Http.HttpClient(new HttpClientHandler
        {
            Proxy = p.ToWebProxy(),
            UseProxy = true
        }))));
    }
}