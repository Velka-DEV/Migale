using AngleSharp.Html.Parser;

namespace Migale.Core.DataFlow.Selectors;

public class CssSelector : ISelector
{
    public SelectorType Type => SelectorType.Css;

    private string Expression { get; }

    private readonly HtmlParser _htmlParser = new();
    
    public CssSelector(string expression)
    {
        Expression = expression;
    }
    
    public string? Select(DataContext context)
    {
        if (context.DocumentType is not DocumentType.Html)
            return null;

        if (string.IsNullOrWhiteSpace(context.Source) || string.IsNullOrWhiteSpace(Expression))
            return null;
        
        var node = context.AsHtmlDocument()?.QuerySelector(Expression);

        return node?.TextContent;
    }

    public IEnumerable<string> SelectMany(DataContext context)
    {
        if (context.DocumentType is not DocumentType.Html)
            return Enumerable.Empty<string>();

        if (string.IsNullOrWhiteSpace(context.Source) || string.IsNullOrWhiteSpace(Expression))
            return Enumerable.Empty<string>();
        
        var nodes = context.AsHtmlDocument()?.QuerySelectorAll(Expression);

        return nodes?.Select(x => x.InnerHtml) ?? Enumerable.Empty<string>();
    }
}