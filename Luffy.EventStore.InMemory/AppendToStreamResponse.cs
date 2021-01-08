using System;

namespace Luffy.EventStore.InMemory
{
  public record AppendToStreamResponse : IAppendToStreamResponse
  {
    public UInt64 NextExpectedStreamPosition { get; init; }
  }
}
