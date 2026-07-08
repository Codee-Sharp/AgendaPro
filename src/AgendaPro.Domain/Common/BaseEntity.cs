namespace AgendaPro.Domain.Common;

public abstract class BaseEntity
{
    protected BaseEntity() : this(Guid.CreateVersion7())
    {
    }

    protected BaseEntity(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; protected set; }
}
