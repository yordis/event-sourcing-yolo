using System;
using System.Collections.Generic;

namespace Luffy.EventStore.InMemory
{
  public class InMemoryEventStore : IEventStore
  {
    private List<IRecordedEvent> allEvents;
    private Dictionary<string, IRecordedEvent> eventsPerStream;

    public InMemoryEventStore()
    {
      allEvents = new List<IRecordedEvent>();
      eventsPerStream = new Dictionary<string, IRecordedEvent>();
    }

    public IAppendToStreamResponse AppendToStream(string streamName, ulong streamRevision, IEnumerable<IEventData> events)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<IRecordedEvent> ReadStream(int readDirection, string streamName, ulong fromStreamRevision, ulong howMany)
    {
      return allEvents;
    }
  }
}
