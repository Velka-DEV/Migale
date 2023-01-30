namespace Migale.Core.Models;

public class CrawResult
{
    public bool Success { get; }
    
    public string? Content { get; }
    
    public string? Error { get; }
    
    public CrawResult(bool success)
    {
        Success = success;
    }
    
    public CrawResult(string? content)
    {
        Success = true;
        Content = content;
    }
    
    public CrawResult(Exception e, bool success = false)
    {
        Success = success;
        Error = e.Message;
    }
}