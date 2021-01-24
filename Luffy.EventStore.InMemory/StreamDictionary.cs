using System.Collections.Concurrent;

namespace Luffy.EventStore.InMemory
{
  public class StreamDictionary : ConcurrentDictionary<string, Stream>
  {
    public Stream GetStream(string streamId)
    {
      return this[streamId];
    }

    public void CreateStream(string streamId)
    {
      TryAdd(streamId, new Stream(streamId));
    }

    public bool StreamExists(string streamId)
    {
      return ContainsKey(streamId);
    }
  }
}
