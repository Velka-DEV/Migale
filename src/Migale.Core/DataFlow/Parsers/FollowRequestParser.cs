using System.Text.RegularExpressions;
using Migale.Core.DataFlow.Selectors;

namespace Migale.Core.DataFlow.Parsers;

public partial class FollowRequestParser : EnumerableParser<string>
{
    public override string Name => nameof(FollowRequestParser);

    public FollowRequestParser() : base(new RegexSelector(HttpUrlRegex()))
    {
    }
    
    public FollowRequestParser(ISelector selector) : base(selector)
    {
    }

    public override IEnumerable<string> Parse(DataContext context)
        => Selector.SelectMany(context).Where(x => HttpUrlRegex().IsMatch(context.Source));

    [GeneratedRegex("^https?:\\\\/\\\\/(?:www\\\\.)?[-a-zA-Z0-9@:%._\\\\+~#=]{1,256}\\\\.[a-zA-Z0-9()]{1,6}\\\\b(?:[-a-zA-Z0-9()@:%_\\\\+.~#?&\\\\/=]*)$")]
    private static partial Regex HttpUrlRegex();
}