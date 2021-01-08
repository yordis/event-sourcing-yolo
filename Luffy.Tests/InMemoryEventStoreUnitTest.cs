using System;
using System.Collections.Generic;
using System.Linq;
using Luffy.EventStore;
using Luffy.EventStore.Exceptions;
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
    public void appending_events_when_the_stream_must_be_present()
    {
      var eventStore = new InMemoryEventStore();

      Assert.Throws<ExpectedStreamExistsException>(() => eventStore.AppendToStream(StreamState.StreamExists, "stream-1",
        new List<IEventData>
        {
          new EventData
          {
            Data = new ReadOnlyMemory<byte>(null),
            Metadata = new ReadOnlyMemory<byte>(null),
            EventId = Guid.NewGuid(),
            EventType = "UserRegistered"
          },
        }));
    }

    [Fact]
    public void appending_events_when_the_stream_must_be_absent()
    {
      var eventStore = new InMemoryEventStore();

      eventStore.AppendToStream(StreamState.Any, "stream-1",
        new List<IEventData>
        {
          new EventData
          {
            Data = new ReadOnlyMemory<byte>(null),
            Metadata = new ReadOnlyMemory<byte>(null),
            EventId = Guid.NewGuid(),
            EventType = "UserRegistered"
          },
        });

      Assert.Throws<ExpectedNoStreamException>(() => eventStore.AppendToStream(StreamState.NoStream, "stream-1",
        new List<IEventData>
        {
          new EventData
          {
            Data = new ReadOnlyMemory<byte>(null),
            Metadata = new ReadOnlyMemory<byte>(null),
            EventId = Guid.NewGuid(),
            EventType = "UserRegistered"
          },
        }));
    }

    [Fact]
    public void append_events_to_a_stream()
    {
      var eventStore = new InMemoryEventStore();

      var response= eventStore.AppendToStream(StreamState.Any, "stream-1", new List<IEventData>
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

      var expected = Convert.ToUInt64(2);

      Assert.Equal(expected, response.NextExpectedStreamRevision);
    }

    [Fact]
    public void read_events_from_a_stream()
    {
      var eventStore = new InMemoryEventStore();

      eventStore.AppendToStream(StreamState.Any, "cart-2", new List<IEventData>
      {
        new EventData
        {
          Data = new ReadOnlyMemory<byte>(null),
          Metadata = new ReadOnlyMemory<byte>(null),
          EventId = Guid.NewGuid(),
          EventType = "CartCreated"
        },
        new EventData
        {
          Data = new ReadOnlyMemory<byte>(null),
          Metadata = new ReadOnlyMemory<byte>(null),
          EventId = Guid.NewGuid(),
          EventType = "CartItemAdded"
        },
        new EventData
        {
          Data = new ReadOnlyMemory<byte>(null),
          Metadata = new ReadOnlyMemory<byte>(null),
          EventId = Guid.NewGuid(),
          EventType = "CartItemAdded"
        },
      });

      var events = eventStore.ReadStream(ReadDirection.Forwards, "cart-2", 1L, 1L);

      Assert.Equal("CartCreated", events.ElementAt(0).Type);
      Assert.Equal(Convert.ToUInt64(0), events.ElementAt(0).StreamEventRevision);

      Assert.Equal("CartItemAdded", events.ElementAt(1).Type);
      Assert.Equal(Convert.ToUInt64(1), events.ElementAt(1).StreamEventRevision);

      Assert.Equal("CartItemAdded", events.ElementAt(2).Type);
      Assert.Equal(Convert.ToUInt64(2), events.ElementAt(2).StreamEventRevision);
    }
  }
}
