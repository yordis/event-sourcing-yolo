using System;
using System.Collections.Generic;
using System.Linq;

namespace Luffy.EventStore.InMemory
{
  public class InMemoryEventStore : IEventStore
  {
    private readonly Dictionary<string, Stream> _eventsPerStream;

    public InMemoryEventStore()
    {
      _eventsPerStream = new Dictionary<string, Stream>();
    }

    public bool IsEmpty()
    {
      return _eventsPerStream.Count == 0;
    }

    public IAppendToStreamResponse AppendToStream(string streamId, UInt64 expectedStreamPosition, IEnumerable<IEventData> events)
    {
      EnsureStreamExists(streamId);

      foreach (var @event in events)
      {
        var recordedEvent = new RecordedEvent
        {
          EventId = Guid.NewGuid(),
          Created = DateTime.Now,
          Data = @event.Data,
          Metadata = @event.Metadata,
          Type = @event.EventType,
          EventStreamId = streamId,
          GlobalEventPosition = 1L,
          StreamEventPosition = 1L
        };

        _eventsPerStream[streamId].Add(recordedEvent);
      }

      return new AppendToStreamResponse
      {
        NextExpectedStreamPosition = Convert.ToUInt64(_eventsPerStream[streamId].Count())
      };
    }

    private void EnsureStreamExists(string streamId)
    {
      if (!_eventsPerStream.ContainsKey(streamId))
      {
        _eventsPerStream.Add(streamId, new Stream());
      }
    }

    public IEnumerable<IRecordedEvent> ReadStream(int readDirection, string streamId, UInt64 fromStreamPosition,
      UInt64 howMany)
    {
      return _eventsPerStream[streamId];
    }
  }
}
