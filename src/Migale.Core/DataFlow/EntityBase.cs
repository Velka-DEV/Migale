namespace Migale.Core.DataFlow;

public abstract class EntityBase<TEntity> where TEntity : class
{
    public virtual Guid Guid { get; } = Guid.NewGuid();

    public DateTime CreatedAt { get; } = DateTime.Now;
}