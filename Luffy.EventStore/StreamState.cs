using System;

namespace Luffy.EventStore
{
  /// <summary>
  /// A structure that represents the state the stream should be in when writing.
  /// </summary>
  public readonly struct StreamState : IEquatable<StreamState>
  {
    private static class Constants
    {
      public const int NoStream = 1;
      public const int Any = 2;
      public const int StreamExists = 4;
    }

    /// <summary>The stream should not exist.</summary>
    public static readonly StreamState NoStream = new(Constants.NoStream);

    /// <summary>The stream may or may not exist.</summary>
    public static readonly StreamState Any = new(Constants.Any);

    /// <summary>The stream must exist.</summary>
    public static readonly StreamState StreamExists = new(Constants.StreamExists);

    private readonly int _value;

    private StreamState(int value)
    {
      switch (value)
      {
        case Constants.NoStream:
        case Constants.Any:
        case Constants.StreamExists:
          _value = value;
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(value));
      }
    }

    public bool Equals(StreamState other)
    {
      return _value.Equals(other._value);
    }

    public override bool Equals(object? obj)
    {
      return obj is StreamState other && Equals(other);
    }

    public override int GetHashCode()
    {
      return _value.GetHashCode();
    }

    public static bool operator ==(StreamState left, StreamState right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(StreamState left, StreamState right)
    {
      return !left.Equals(right);
    }

    public long ToInt64()
    {
      return -Convert.ToInt64(_value);
    }

    public static implicit operator int(StreamState streamState)
    {
      return streamState._value;
    }

    public override string ToString()
    {
      switch (_value)
      {
        case Constants.NoStream:
          return "NoStream";
        case Constants.Any:
          return "Any";
        case Constants.StreamExists:
          return "StreamExists";
        default:
          return _value.ToString();
      }
    }
  }
}
