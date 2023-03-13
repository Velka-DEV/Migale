using System.Collections;

namespace Migale.Core.DataFlow.Selectors;

public interface ISelector
{
    public SelectorType Type { get; }

    public string? Select(DataContext context);

    public IEnumerable<string> SelectMany(DataContext context);
}