using System.Collections.Concurrent;

namespace Luffy.EventStore.InMemory
{
  public class Stream : BlockingCollection<RecordedEvent>
  {
    public bool IsEmpty()
    {
      return this.Count == 0;
    }
  }
}
