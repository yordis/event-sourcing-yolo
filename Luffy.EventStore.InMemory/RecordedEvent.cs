using System;

namespace Luffy.EventStore.InMemory
{
  public record RecordedEvent : IRecordedEvent
  {
    public Guid EventId { get; init; }
    public string EventStreamId { get; init; }
    public UInt64 StreamEventPosition { get; init; }
    public UInt64 GlobalEventPosition { get; init; }
    public string Type { get; init; }
    public DateTime Created { get; init; }
    public ReadOnlyMemory<byte> Data { get; init; }
    public ReadOnlyMemory<byte> Metadata { get; init; }
  }
}
