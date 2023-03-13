using System.Reflection;
using Migale.Core.DataFlow.Parsers;

namespace Migale.Core.DataFlow;

public class ParsingModel<TEntity> where TEntity : EntityBase<TEntity>
{
    public EntityValidator? Validator { get; }
    
    public IEnumerable<FollowRequestParser> FollowRequestParsers { get; }

    public ParsingModel()
    {
        Validator = typeof(TEntity).GetCustomAttributes(typeof(EntityValidator), true).FirstOrDefault() as EntityValidator;
        FollowRequestParsers = typeof(TEntity).GetCustomAttributes(typeof(TEntity))
            .Where(x => x.GetType() == typeof(FollowRequestParser))
            .Cast<FollowRequestParser>();
    }
    
    public bool Validate(DataContext context)
        => Validator?.Validate(context) ?? false;
    
    public TEntity? ParseEntity(DataContext context)
    {
        var entity = Activator.CreateInstance(typeof(TEntity)) as TEntity;
        
        foreach (var prop in typeof(TEntity).GetProperties())
        {
            var attributes = prop.GetCustomAttributes();

            foreach (var parserAttr in attributes.Where(x => x.GetType().IsSubclassOf(typeof(IParser))))
            {
                if (Activator.CreateInstance(parserAttr.GetType()) is not IParser parser)
                    continue;

                prop.SetValue(entity, parser.Parse(context));
            }
        }

        return entity;
    }
}