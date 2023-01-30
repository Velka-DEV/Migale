using Migale.Core.Models;

namespace Migale.Core.Events;

public class RequestFailedEventArgs
{
    public required HttpTarget Target { get; set; }
    
    public required CrawResult? Result { get; set; }
}