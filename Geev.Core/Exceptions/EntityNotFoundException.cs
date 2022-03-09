using Geev.Core.Extensions;

namespace Geev.Core.Exceptions;

public class EntityNotFoundException : GeevException
{
    public const int ExceptionCode = 2;

    /// <summary>
    /// Type of the entity.
    /// </summary>
    public Type EntityType { get; set; }

    /// <summary>
    /// Id of the Entity.
    /// </summary>
    public object Id { get; set; }

    public EntityNotFoundException(Type entityType, object id, IDictionary<string, object> extra)
        : this(entityType, id, null, extra)
    {
        ErrorCode = ExceptionCode;
    }

    /// <summary>
    /// Creates a new <see cref="EntityNotFoundException"/> object.
    /// </summary>
    public EntityNotFoundException(Type entityType, object id, Exception innerException, IDictionary<string, object> extra)
        : base("Entity not found", $"There is no such an entity. Entity type: {entityType.FullName}, id: {id}, extra: {extra.ToFormattedString()}", innerException)
    {
        EntityType = entityType;
        Id = id;
        ErrorCode = ExceptionCode;
    }
}
