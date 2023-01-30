using Microsoft.AspNetCore.Http.Extensions;

namespace Migale.Core.Models;

public class HttpTarget
{
    public HttpMethod Method { get; set; }
    
    public string Url { get; set; }
    
    public string? ContentType { get; set; }
    
    public HttpContent? Content { get; set; }
    
    public Dictionary<string, string>? Headers { get; set; }
    
    public Dictionary<string, string>? QueryParameters { get; set; }
    
    public HttpTarget(HttpMethod method, string url)
    {
        Method = method;
        Url = url;
    }
    
    public HttpTarget(string url) : this(HttpMethod.Get, url) { }
    
    public HttpRequestMessage BuildRequest()
    {
        var request = new HttpRequestMessage(Method, Url)
        {
            Content = Content,
        };

        if (Headers is not null)
        {
            foreach (var header in Headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }
        }
        
        if (QueryParameters is not null)
        {
            var query = new QueryBuilder(QueryParameters);
            request.RequestUri = new Uri($"{Url}{query.ToQueryString()}");
        }
        
        return request;
    }
}