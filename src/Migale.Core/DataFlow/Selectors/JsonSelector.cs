namespace Migale.Core.DataFlow.Selectors;

public class JsonSelector : ISelector
{
    public SelectorType Type => SelectorType.JsonPath;
    
    public string Path { get; }

    public JsonSelector(string path)
    {
        Path = path;
    }
    
    public string? Select(DataContext context)
    {
        if (context.DocumentType is not DocumentType.Json)
            return null;
        
        if (string.IsNullOrWhiteSpace(context.Source) || string.IsNullOrWhiteSpace(Path))
            return null;

        return context.AsJsonDocument()?.GetProperty(Path).GetString();
    }

    public IEnumerable<string> SelectMany(DataContext context)
    {
        if (string.IsNullOrWhiteSpace(context.Source) || string.IsNullOrWhiteSpace(Path))
            return Enumerable.Empty<string>();

        //TODO: Check how to cast json array property to Enumerable<string> properly
        
        return Enumerable.Empty<string>();
    }
}