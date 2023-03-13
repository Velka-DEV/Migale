using Migale.Core.DataFlow.Selectors;

namespace Migale.Core.DataFlow.Parsers;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public abstract class ParserBase : Attribute, IParser
{
    protected ISelector Selector { get; }

    protected ParserBase(ISelector selector)
    {
        Selector = selector;
    }

    public abstract string Name { get; }
    
    public abstract object Parse(DataContext context);
}