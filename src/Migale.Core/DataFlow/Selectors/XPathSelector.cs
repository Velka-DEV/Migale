using AngleSharp.Html.Parser;
using AngleSharp.XPath;

namespace Migale.Core.DataFlow.Selectors;

public class XPathSelector : ISelector
{
    public SelectorType Type => SelectorType.XPath;

    private string Expression { get; }
    
    private readonly HtmlParser _htmlParser = new();

    public XPathSelector(string expression)
    {
        Expression = expression;
    }

    public string? Select(DataContext context)
    {
        if (context.DocumentType is not DocumentType.Html or DocumentType.Xml)
            return null;

        if (string.IsNullOrWhiteSpace(context.Source) || string.IsNullOrWhiteSpace(Expression))
            return null;
        
        var node = context.AsHtmlDocument()?.Body.SelectSingleNode(Expression);

        return node?.TextContent;
    }

    public IEnumerable<string> SelectMany(DataContext context)
    {
        if (context.DocumentType is not DocumentType.Html or DocumentType.Xml)
            return Enumerable.Empty<string>();

        if (string.IsNullOrWhiteSpace(context.Source) || string.IsNullOrWhiteSpace(Expression))
            return Enumerable.Empty<string>();

        var nodes = context.AsHtmlDocument()?.Body.SelectNodes(Expression);

        return nodes?.Select(x => x.TextContent) ?? Enumerable.Empty<string>();
    }
}