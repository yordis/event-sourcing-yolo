using System;

namespace Luffy.EventStore
{
  public interface IEventData
  {
    Guid EventId { get; }
    string EventType { get; }
    ReadOnlyMemory<byte> Data { get; }
    ReadOnlyMemory<byte> Metadata { get; }
  }
}
