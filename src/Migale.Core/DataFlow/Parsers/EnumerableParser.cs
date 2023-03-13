using Migale.Core.DataFlow.Selectors;

namespace Migale.Core.DataFlow.Parsers;

public abstract class EnumerableParser<T> : ParserBase
{
    public override string Name => nameof(EnumerableParser<T>);
    
    protected EnumerableParser(ISelector selector) : base(selector)
    {
    }

    public abstract override IEnumerable<T> Parse(DataContext context);
}