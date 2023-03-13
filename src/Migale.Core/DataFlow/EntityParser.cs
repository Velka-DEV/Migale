using System.Reflection;

namespace Migale.Core.DataFlow;

public class EntityParser<TEntity> where TEntity : EntityBase<TEntity>
{
    private readonly ParsingModel<TEntity> _parsingModel = new();

    public bool ShouldParse(DataContext context)
    {
        return _parsingModel.Validator?.Validate(context) ?? false;
    }
    
    public TEntity? ParseEntity(string source)
    {
        return default(TEntity);
    }
}