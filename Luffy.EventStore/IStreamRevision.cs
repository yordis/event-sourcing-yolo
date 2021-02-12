using System;

namespace Luffy.EventStore
{
  public interface IStreamRevision
  {
    UInt64 Value { get; }
  }
}
