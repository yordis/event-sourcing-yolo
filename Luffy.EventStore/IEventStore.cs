using System;
using System.Collections.Generic;

namespace Luffy.EventStore
{
  public interface IEventStore
  {
    public IAppendToStreamResponse AppendToStream(
      UInt64 expectedStreamRevision,
      string streamId,
      IEnumerable<IEventData> events
    );

    public IAppendToStreamResponse AppendToStream(
      StreamState expectedStreamState,
      string streamId,
      IEnumerable<IEventData> events
    );

    public IEnumerable<IRecordedEvent> ReadStream(
      ReadDirection readDirection,
      string streamId,
      UInt64 fromStreamRevision,
      UInt64 howMany
    );

    public IEnumerable<IRecordedEvent> ReadAll(
      ReadDirection direction,
      UInt64 howMany
    );
  }
}
