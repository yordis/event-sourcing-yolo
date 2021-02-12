using System;

namespace Luffy.EventStore
{
  public class StreamRevision : IStreamRevision
  {
    public static StreamRevision Start = new StreamRevision(UInt64.MinValue);
    public static StreamRevision End = new StreamRevision(UInt64.MaxValue);

    public UInt64 Value { get; }

    public static UInt64 ToStreamRevision(int value)
    {
      return Convert.ToUInt64(value);
    }

    public StreamRevision(int value)
    {
      Value = ToStreamRevision(value);
    }

    public StreamRevision(UInt64 value)
    {
      Value = value;
    }
  }
}
