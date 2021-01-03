using System;

namespace Luffy.EventStore
{
  public readonly struct StreamRevision : IEquatable<StreamRevision>
  {
    private static class Constants
    {
      public static readonly ulong NoStream = Convert.ToUInt64(-1);
      public static readonly ulong Any = Convert.ToUInt64(-2);
      public static readonly ulong StreamExists = Convert.ToUInt64(-4);
    }

    /// <summary>
    /// Represents no <see cref="T:Luffy.EventStore.StreamRevision" />, i.e., when a stream does not exist.
    /// </summary>
    public static readonly StreamRevision None = new StreamRevision(ulong.MaxValue);

    private readonly ulong _value;

    public StreamRevision(ulong value)
    {
      _value = value <= (ulong) long.MaxValue || value == ulong.MaxValue
        ? value
        : throw new ArgumentOutOfRangeException(nameof(value));
    }

    public bool Equals(StreamRevision other)
    {
      return _value.Equals(other._value);
    }

    public override bool Equals(object obj)
    {
      return obj is StreamRevision other && Equals(other);
    }

    public override int GetHashCode()
    {
      return _value.GetHashCode();
    }
  }
}
