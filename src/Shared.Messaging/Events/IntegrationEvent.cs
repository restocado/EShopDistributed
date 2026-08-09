namespace Shared.Messaging.Events;

public abstract class IntegrationEvent
{
    // Unique identifier for this event instance
    public Guid EventId { get; init;  }

    // When the event was created
    public DateTimeOffset OccurredOn { get; init; }

    // Logical type name for consumers
    public string EventType => GetType().Name;

    // Optional correlation ID for tracing across services
    public Guid? CorrelationId { get; init; }

    // Optional source service name (e.g., "Catalog")
    public string? Source { get; init; }

    // Default constructor (no metadata provided)
    protected IntegrationEvent()
    {
        EventId = Guid.NewGuid();
        OccurredOn = DateTimeOffset.UtcNow;
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
