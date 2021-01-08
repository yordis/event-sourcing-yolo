using System;
using System.Collections.Generic;

namespace Luffy.EventStore
{
  public interface IEventStore
  {
    public IAppendToStreamResponse AppendToStream(
      string streamId,
      UInt64 expectedStreamPosition,
      IEnumerable<IEventData> events
    );

    IEnumerable<IRecordedEvent> ReadStream(
      int readDirection,
      string streamId,
      UInt64 fromStreamPosition,
      UInt64 howMany
    );
  }
}
