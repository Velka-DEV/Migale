using System.Net;

namespace Migale.Core.Models;

public class Proxy
{
    public required string Host { get; set; }
    
    public required int Port { get; set; }
    
    public required ProxyType Type { get; set; }
    
    public NetworkCredential? Credentials { get; set; } 

    public bool HasAuthentication => Credentials is not null;
    
    /// <summary>
    /// Return the proxy has url scheme (protocol://username:password). Actually don't support authentication
    /// </summary>
    /// <returns>Url proxy scheme</returns>
    public override string ToString() => $"{Type.ToString().ToLower()}://{Host}:{Port}";

    /// <summary>
    /// Return the proxy as WebProxy, commonly used by HttpClient
    /// </summary>
    /// <returns></returns>
    public WebProxy ToWebProxy() => new WebProxy(ToString())
    {
        Credentials = Credentials
    };

    public static Proxy? Parse(string proxy, ProxyType proxyType)
    {
        if (string.IsNullOrWhiteSpace(proxy) || !proxy.Contains(':'))
            return null;
        
        try
        {
            var proxyParts = proxy.Split(':');
            
            if (proxyParts.Length < 2)
                return null;
        
            var host = proxyParts[0];
            var port = int.Parse(proxyParts[1]);
            
            return new Proxy()
            {
                Host = host,
                Port = port,
                Type = proxyType,
                Credentials = proxyParts.Length == 4 ? new NetworkCredential(proxyParts[2], proxyParts[3]) : null
            };
        }
        catch
        {
            return null;
        }
    }
}