using Migale.Core.DataFlow.Selectors;

namespace Migale.Core.DataFlow.Parsers;

public class StringParser : ParserBase
{
    public override string Name => nameof(StringParser);

    public StringParser(ISelector selector) : base(selector) { }

    public override string Parse(DataContext context)
        => Selector.Select(context) ?? string.Empty;
}