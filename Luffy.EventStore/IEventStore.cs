using System;
using System.Collections.Generic;

namespace Luffy.EventStore
{
  public interface IAppendToStreamResponse
  {
    ulong NextExpectedStreamPosition { get; }
  }

  public interface IRecordedEvent
  {
    Guid EventId { get; }
    string EventStreamId { get; }
    ulong StreamEventPosition { get;  }
    ulong GlobalEventPosition { get;  }
    string Type { get; }
    DateTime Created { get; }
    ReadOnlyMemory<byte> Data { get; }
    ReadOnlyMemory<byte> Metadata { get; }
  }

  public interface IEventData
  {
    Guid EventId { get; }
    string EventType { get; }
    ReadOnlyMemory<byte> Data { get; }
    ReadOnlyMemory<byte> Metadata { get; }
  }

  public interface IEventStore
  {
    public IAppendToStreamResponse AppendToStream(
      string streamName,
      ulong streamRevision,
      IEnumerable<IEventData> events
    );

    IRecordedEvent[] ReadStream(
      int readDirection,
      string streamName,
      ulong fromStreamRevision,
      ulong howMany
    );
  }
}
