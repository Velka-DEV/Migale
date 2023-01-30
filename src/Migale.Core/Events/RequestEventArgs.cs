using Migale.Core.Models;

namespace Migale.Core.Events;

public class RequestEventArgs : EventArgs
{
    public required HttpTarget Target { get; set; }
}