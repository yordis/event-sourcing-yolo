using System;

namespace Luffy.EventStore.InMemory
{
  public record EventData : IEventData
  {
    public Guid EventId { get; init; }
    public string EventType { get; init; }
    public ReadOnlyMemory<byte> Data { get; init; }
    public ReadOnlyMemory<byte> Metadata { get; init; }
  }
}
