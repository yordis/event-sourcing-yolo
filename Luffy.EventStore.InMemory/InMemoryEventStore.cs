﻿using System.Linq;
using System;
using System.Collections.Generic;
using Luffy.EventStore.Exceptions;

namespace Luffy.EventStore.InMemory
{
  public class InMemoryEventStore : IEventStore
  {
    private readonly StreamDictionary _streams;

    public InMemoryEventStore()
    {
      _streams = new StreamDictionary();
      _streams.CreateStream(Constants.AllStreamId);
    }

    private Stream GetAllStream()
    {
      return _streams.GetStream(Constants.AllStreamId);
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
      IStreamRevision fromStreamRevision,
      UInt64 howMany)
    {
      return _streams.GetStream(streamId).FromStreamRevision(RecordedEvent.RevisionType.Stream, readDirection, fromStreamRevision, howMany);
    }

    public IEnumerable<IRecordedEvent> ReadAll(ReadDirection direction, IStreamRevision fromStreamRevision, UInt64 howMany)
    {
      return GetAllStream().GetEvents();
    }

    private AppendToStreamResponse DoAppendToStream(
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
          GlobalEventRevision = NextGlobalEventRevision(),
          StreamEventRevision = NextStreamEventRevision(streamId),
        });
      }

      return new AppendToStreamResponse
      {
        NextExpectedStreamRevision = StreamRevision.ToStreamRevision(GetStream(streamId).Count())
      };
    }

    private void EnsureStreamRevision(string streamId, StreamState expectedStreamState, UInt64? expectedStreamRevision)
    {
      if (StreamState.Any == expectedStreamState && expectedStreamRevision == null)
      {
        return;
      }

      var expected = expectedStreamRevision ?? NextStreamEventRevision(streamId);
      var actual = NextStreamEventRevision(streamId);

      if (expected != actual)
      {
        throw new ExpectedStreamRevisionException(streamId, expected, actual);
      }
    }

    private Stream GetStream(string streamId)
    {
      return _streams.GetStream(streamId);
    }

    private UInt64 NextGlobalEventRevision()
    {
      return GetAllStream().NextStreamEventRevision();
    }

    private UInt64 NextStreamEventRevision(string streamId)
    {
      return GetStream(streamId).NextStreamEventRevision();
    }

    private void AppendRecordedEvent(string streamId, RecordedEvent recordedEvent)
    {
      _streams.GetStream(streamId).AppendEvent(recordedEvent);
      GetAllStream().AppendEvent(recordedEvent);
    }

    private void EnsureStreamState(string streamId, StreamState expectedStreamState)
    {
      if (_streams.StreamExists(streamId))
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

      _streams.CreateStream(streamId);
    }
  }
}
