using System;

namespace Luffy.EventStore
{
  public interface IRecordedEvent
  {
    Guid EventId { get; }
    string EventStreamId { get; }
    UInt64 StreamEventPosition { get;  }
    UInt64 GlobalEventPosition { get;  }
    string Type { get; }
    DateTime Created { get; }
    ReadOnlyMemory<byte> Data { get; }
    ReadOnlyMemory<byte> Metadata { get; }
  }
}
