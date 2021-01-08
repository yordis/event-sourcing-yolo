using System;

namespace Luffy.EventStore.Exceptions
{
  public class ExpectedStreamRevisionException : Exception
  {
    public string StreamId { get; }
    public UInt64 Expected { get; }
    public UInt64 Actual { get; }

    public ExpectedStreamRevisionException(
      string streamId,
      UInt64 expected,
      UInt64 actual,
      Exception? exception = null
    ) : base($"Expected {expected.ToString()} stream revision but got {actual.ToString()} revision instead in {streamId} stream",
      exception)
    {
      StreamId = streamId;
      Expected = expected;
      Actual = actual;
    }
  }
}
