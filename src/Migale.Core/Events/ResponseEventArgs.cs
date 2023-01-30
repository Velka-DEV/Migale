using Migale.Core.Models;

namespace Migale.Core.Events;

public class ResponseEventArgs : EventArgs
{
    public required HttpTarget Target { get; set; }
    
    public required CrawResult? Result { get; set; }
}