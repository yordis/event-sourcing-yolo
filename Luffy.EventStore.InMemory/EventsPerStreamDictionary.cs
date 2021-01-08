using System.Collections.Concurrent;

namespace Luffy.EventStore.InMemory
{
  public class EventsPerStreamDictionary : ConcurrentDictionary<string, Stream>
  {
    public Stream GetStream(string streamId)
    {
      return this[streamId];
    }
  }
}
