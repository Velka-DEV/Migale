using System.Text.Json;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;

namespace Migale.Core.DataFlow;

public class DataContext
{
    public string Source { get; set; }

    public DocumentType DocumentType { get; }

    private IHtmlDocument? _htmlDocument;
    private JsonElement? _jsonDocument;

    public DataContext(string source)
    {
        Source = source;
        DocumentType = GetDocumentType(source);
    }

    public DataContext(string source, DocumentType documentType)
    {
        Source = source;
        DocumentType = documentType;
    }

    public IHtmlDocument? AsHtmlDocument()
    {
        try
        {
            return _htmlDocument ??= new HtmlParser().ParseDocument(Source);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public JsonElement? AsJsonDocument()
    {
        try
        {
            return _jsonDocument ??= JsonDocument.Parse(Source).RootElement;
        }
        catch (Exception)
        {
            return null;
        }
    }

    private static DocumentType GetDocumentType(string source)
    {
        if (string.IsNullOrWhiteSpace(source))
            return DocumentType.Empty;

        if (source.StartsWith("<!DOCTYPE html>", StringComparison.InvariantCultureIgnoreCase) ||
            source.StartsWith("<html", StringComparison.InvariantCultureIgnoreCase))
            return DocumentType.Html;

        if (source.StartsWith("<?xml", StringComparison.InvariantCultureIgnoreCase))
            return DocumentType.Xml;

        if ((source.StartsWith("{") && source.EndsWith("}")) ||
            (source.StartsWith("[") && source.EndsWith("]")))
            return DocumentType.Json;

        return DocumentType.Text;
    }
}