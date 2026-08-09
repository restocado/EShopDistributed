namespace Shared.Messaging.Events;

public abstract class IntegrationEvent
{
    // Unique identifier for this event instance
    public Guid EventId { get; }

    // When the event was created
    public DateTime OccurredOn { get; }

    // Logical type name for consumers
    public string EventType => GetType().AssemblyQualifiedName ?? string.Empty;

    // Optional correlation ID for tracing across services
    public Guid? CorrelationId { get; }

    // Optional source service name (e.g., "Catalog")
    public string? Source { get; }

    // Default constructor (no metadata provided)
    protected IntegrationEvent()
    {
        EventId = Guid.NewGuid();
        OccurredOn = DateTime.UtcNow;
    }

    // Constructor with correlation ID
    protected IntegrationEvent(Guid? correlationId)
        : this()
    {
        CorrelationId = correlationId;
    }

    // Constructor with correlation ID and source
    protected IntegrationEvent(Guid? correlationId, string? source)
        : this(correlationId)
    {
        Source = source;
    }
}
