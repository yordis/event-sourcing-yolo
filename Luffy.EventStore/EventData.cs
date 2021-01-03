using System;

namespace Luffy.EventStore
{
  public sealed class EventData
  {
    public readonly Guid EventId;
    public readonly string Type;
    public readonly ReadOnlyMemory<byte> Data;
    public readonly ReadOnlyMemory<byte> Metadata;

    public EventData(Guid eventId, string type, ReadOnlyMemory<byte> data, ReadOnlyMemory<byte>? metadata = null)
    {
      EventId = eventId;
      Type = type;
      Data = data;
      Metadata = metadata ?? (ReadOnlyMemory<byte>) Array.Empty<byte>();
    }
  }
}
