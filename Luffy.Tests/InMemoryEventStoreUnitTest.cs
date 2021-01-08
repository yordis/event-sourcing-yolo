using System;
using System.Collections.Generic;
using System.Linq;
using Luffy.EventStore;
using Luffy.EventStore.InMemory;
using Xunit;

namespace Luffy.Tests
{
  public class InMemoryStoreEventStoreUnitTest
  {
    [Fact]
    public void creates_an_empty_event_store()
    {
      var eventStore = new InMemoryEventStore();
      Assert.True(eventStore.IsEmpty(), "event store should be empty");
    }

    [Fact]
    public void append_an_event_to_a_stream()
    {
      var eventStore = new InMemoryEventStore();

      eventStore.AppendToStream("stream-1", 0, new List<IEventData>
      {
        new EventData
        {
          Data = new ReadOnlyMemory<byte>(null),
          Metadata = new ReadOnlyMemory<byte>(null),
          EventId = Guid.NewGuid(),
          EventType = "UserRegistered"
        },
        new EventData
        {
          Data = new ReadOnlyMemory<byte>(null),
          Metadata = new ReadOnlyMemory<byte>(null),
          EventId = Guid.NewGuid(),
          EventType = "UserConfirmed"
        },
      });

      var events = eventStore.ReadStream(1, "stream-1", 1L, 1L);

      Assert.Equal(2, events.Count());
    }
  }
}
