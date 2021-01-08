using System;

namespace Luffy.EventStore.Exceptions
{
  public class ExpectedStreamExistsException : Exception
  {
    public string StreamId { get; }

    public ExpectedStreamExistsException(
      string streamId,
      Exception? exception = null
    ) : base($"Expected {streamId} Stream to exists", exception)
    {
      StreamId = streamId;
    }
  }
}
