using Migale.Core.DataFlow.Selectors;

namespace Migale.Core.DataFlow;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class EntityValidator : Attribute
{
    public ISelector Selector { get; }
    
    public EntityValidator(ISelector selector)
    {
        Selector = selector;
    }

    public bool Validate(DataContext context)
        => Selector.Select(context) is not null;
}