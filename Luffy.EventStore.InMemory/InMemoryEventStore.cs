using System;
using System.Collections.Generic;

namespace Luffy.EventStore.InMemory
{
  public class InMemoryEventStore : Luffy.EventStore.IEventStore
  {
    public IAppendToStreamResponse AppendToStream(string streamName, ulong streamRevision, IEnumerable<IEventData> events)
    {
      throw new NotImplementedException();
    }

    public IRecordedEvent[] ReadStream(int readDirection, string streamName, ulong fromStreamRevision, ulong howMany)
    {
      throw new NotImplementedException();
    }
  }
}
