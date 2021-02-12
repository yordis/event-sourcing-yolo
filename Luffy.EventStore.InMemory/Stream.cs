using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Luffy.EventStore.InMemory
{
  public class Stream
  {

    private readonly List<RecordedEvent> _data;
    private readonly string _streamId;

    public Stream(string streamId)
    {
      _data = new List<RecordedEvent>();
      _streamId = streamId;
    }

    public int Count()
    {
      return _data.Count();
    }

    public void AppendEvent(RecordedEvent @event)
    {
      _data.Add(@event);
    }

    public List<RecordedEvent> FromStreamRevision(RecordedEvent.RevisionType revisionType, ReadDirection readDirection, IStreamRevision streamRevision, UInt64 howMany)
    {
      var stream = new List<RecordedEvent>();
      var foundStreamRevisionIndex = FindStreamRevisionIndex(revisionType, streamRevision);

      if (foundStreamRevisionIndex == -1)
      {
        return stream;
      }

      var currentIndex = foundStreamRevisionIndex;
      var cod = readDirection == ReadDirection.Forwards ? currentIndex < _data.Count : currentIndex >= 0;

      while (cod && Convert.ToUInt64(stream.Count) <= howMany)
      {
        stream.Add(GetEventByIndex(currentIndex));

        if (ReadDirection.Forwards == readDirection)
        {
          currentIndex++;
        }
        else
        {
          currentIndex--;
        }
      }

      return stream;
    }

    public List<RecordedEvent> GetEvents()
    {
      return _data;
    }

    public UInt64 NextStreamEventRevision()
    {
      return StreamRevision.ToStreamRevision(_data.Count());
    }

    private int FindStreamRevisionIndex(RecordedEvent.RevisionType revisionType, IStreamRevision streamRevision)
    {
      return _data.FindIndex(@event => @event.GetEventRevisionFor(revisionType) == streamRevision.Value);
    }

    private RecordedEvent GetEventByIndex(int index)
    {
      return _data.ElementAt(index);
    }
  }
}
