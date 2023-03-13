using System.Globalization;
using System.Numerics;
using Migale.Core.DataFlow.Selectors;

namespace Migale.Core.DataFlow.Parsers;

public class NumberParser<T> : ParserBase where T : INumber<T>
{
    public override string Name => nameof(NumberParser<T>);

    public NumberParser(ISelector selector) : base(selector)
    {
    }

    public override INumber<T> Parse(DataContext context)
        => T.Parse(Selector.Select(context) ?? "0", CultureInfo.InvariantCulture);
}