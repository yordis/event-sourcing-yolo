using System;

namespace Luffy.EventStore
{
  public interface IAppendToStreamResponse
  {
    UInt64 NextExpectedStreamRevision { get; }
  }
}
