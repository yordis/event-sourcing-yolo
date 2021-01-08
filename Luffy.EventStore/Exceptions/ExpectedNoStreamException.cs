using System;

namespace Luffy.EventStore.Exceptions
{
  public class ExpectedNoStreamException : Exception
  {
    public string StreamId { get; }

    public ExpectedNoStreamException(
      string streamId,
      Exception? exception = null
    ) : base($"Expected {streamId} Stream to be absent", exception)
    {
      StreamId = streamId;
    }
  }
}
