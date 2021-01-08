using System;

namespace Luffy.EventStore
{
  public static class StreamRevision
  {
    public static UInt64 Start = UInt64.MinValue;
    public static UInt64 End = UInt64.MaxValue;

    public static UInt64 ToStreamRevision(int value)
    {
      return Convert.ToUInt64(value);
    }
  }
}
