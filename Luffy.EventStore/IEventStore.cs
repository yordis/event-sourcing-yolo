using System;
using System.Collections.Generic;

namespace Luffy.EventStore
{
  public interface IEventStore
  {
    /// <exception cref="Luffy.EventStore.Exceptions.ExpectedStreamRevisionException">
    /// Thrown when the expected stream revision does not match
    /// </exception>
    public IAppendToStreamResponse AppendToStream(
      UInt64 expectedStreamRevision,
      string streamId,
      IEnumerable<IEventData> events
    );

    /// <exception cref="Luffy.EventStore.Exceptions.ExpectedNoStreamException">
    /// Thrown when the expected stream state is NoStream and the stream exists
    /// </exception>
    /// <exception cref="Luffy.EventStore.Exceptions.ExpectedStreamExistsException">
    /// Thrown when the expected stream state is StreamExists and the stream does not exists
    /// </exception>
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
      UInt64 fromStreamRevision,
      UInt64 howMany
    );
  }
}
