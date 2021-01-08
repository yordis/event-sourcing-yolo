using System;
using System.Collections.Generic;
using System.Linq;
using Luffy.EventStore.Exceptions;

namespace Luffy.EventStore.InMemory
{
  public class InMemoryEventStore : IEventStore
  {
    private readonly Stream _allEvents;
    private readonly Dictionary<string, Stream> _eventsPerStream;

    public InMemoryEventStore()
    {
      _allEvents = new Stream();
      _eventsPerStream = new Dictionary<string, Stream>();
    }

    public bool IsEmpty()
    {
      return _eventsPerStream.Count == 0;
    }

    public IAppendToStreamResponse AppendToStream(UInt64 expectedStreamRevision, string streamId,
      IEnumerable<IEventData> events)
    {
      return DoAppendToStream(streamId, events, StreamState.Any, expectedStreamRevision);
    }

    public IAppendToStreamResponse AppendToStream(StreamState expectedStreamState, string streamId,
      IEnumerable<IEventData> events)
    {
      return DoAppendToStream(streamId, events, expectedStreamState, null);
    }

    public IEnumerable<IRecordedEvent> ReadStream(ReadDirection readDirection, string streamId,
      UInt64 fromStreamRevision,
      UInt64 howMany)
    {
      return _eventsPerStream[streamId];
    }

    public IEnumerable<IRecordedEvent> ReadAll(ReadDirection direction, UInt64 howMany)
    {
      return _allEvents;
    }

    private IAppendToStreamResponse DoAppendToStream(
      string streamId,
      IEnumerable<IEventData> events,
      StreamState expectedStreamState,
      UInt64? expectedStreamRevision)
    {
      EnsureStreamState(streamId, expectedStreamState);
      EnsureStreamRevision(streamId, expectedStreamState, expectedStreamRevision);

      foreach (var @event in events)
      {
        AppendRecordedEvent(streamId, new RecordedEvent
        {
          EventId = Guid.NewGuid(),
          Created = DateTime.Now,
          Data = @event.Data,
          Metadata = @event.Metadata,
          Type = @event.EventType,
          EventStreamId = streamId,
          GlobalEventRevision = NextGlobalEventPosition(),
          StreamEventRevision = NextStreamEventPosition(streamId),
        });
      }

      return new AppendToStreamResponse
      {
        NextExpectedStreamRevision = Convert.ToUInt64(_eventsPerStream[streamId].Count())
      };
    }

    private void EnsureStreamRevision(string streamId, StreamState expectedStreamState, UInt64? expectedStreamPosition)
    {
      if (StreamState.Any == expectedStreamState && expectedStreamPosition == null)
      {
        return;
      }

      var expected = expectedStreamPosition ?? NextStreamEventPosition(streamId);
      var actual = NextStreamEventPosition(streamId);

      if (expected == actual)
      {
        return;
      }

      throw new ExpectedStreamRevisionException(streamId, expected, actual);
    }

    private Stream GetStream(string streamId)
    {
      return _eventsPerStream[streamId];
    }

    private UInt64 NextGlobalEventPosition()
    {
      return Convert.ToUInt64(_allEvents.Count());
    }

    private UInt64 NextStreamEventPosition(string streamId)
    {
      return Convert.ToUInt64(_eventsPerStream[streamId].Count());
    }

    private void AppendRecordedEvent(string streamId, RecordedEvent recordedEvent)
    {
      _eventsPerStream[streamId].Add(recordedEvent);
      _allEvents.Add(recordedEvent);
    }

    private void EnsureStreamState(string streamId, StreamState expectedStreamState)
    {
      if (_eventsPerStream.ContainsKey(streamId))
      {
        if (StreamState.NoStream == expectedStreamState)
        {
          throw new ExpectedNoStreamException(streamId);
        }

        return;
      }

      if (StreamState.StreamExists == expectedStreamState)
      {
        throw new ExpectedStreamExistsException(streamId);
      }

      _eventsPerStream.Add(streamId, new Stream());
    }
  }
}
