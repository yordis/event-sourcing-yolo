using System;

namespace Luffy.EventStore
{
  public interface IAppendToStreamResponse
  {
    UInt64 NextExpectedStreamPosition { get; }
  }
}
