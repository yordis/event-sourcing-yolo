using System;

namespace Luffy.EventStore.InMemory
{
  public record RecordedEvent : IRecordedEvent
  {
    public Guid EventId { get; init; }
    public string EventStreamId { get; init; }
    public UInt64 StreamEventRevision { get; init; }
    public UInt64 GlobalEventRevision { get; init; }
    public string Type { get; init; }
    public DateTime Created { get; init; }
    public ReadOnlyMemory<byte> Data { get; init; }
    public ReadOnlyMemory<byte> Metadata { get; init; }
  }
}
