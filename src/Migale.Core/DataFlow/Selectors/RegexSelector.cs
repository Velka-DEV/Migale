using System.Text.RegularExpressions;

namespace Migale.Core.DataFlow.Selectors;

public class RegexSelector : ISelector
{
    public SelectorType Type => SelectorType.Regex;
    
    private Regex Regex { get; }

    public RegexSelector(string pattern, RegexOptions options = RegexOptions.None) : this(new Regex(pattern, options)) { }
    
    public RegexSelector(Regex regex)
    {
        Regex = regex;
    }
    
    public string? Select(DataContext context)
    {
        if (string.IsNullOrWhiteSpace(context.Source))
            return null;
        
        var result = Regex.Match(context.Source);

        return result.Value;
    }

    public IEnumerable<string> SelectMany(DataContext context)
    {
        if (string.IsNullOrWhiteSpace(context.Source))
            return Enumerable.Empty<string>();
        
        var results = Regex.Matches(context.Source);

        return results.Select(x => x.Value);
    }
}