using System.Collections.Concurrent;

namespace Luffy.EventStore.InMemory
{
  public class Stream : BlockingCollection<IRecordedEvent>, IStream
  {
    private string _streamId;

    public Stream(string streamId)
    {
      _streamId = streamId;
    }

    public bool IsEmpty()
    {
      return Count == 0;
    }

    public void AppendEvent(IRecordedEvent @event)
    {
      Add(@event);
    }
  }
}
