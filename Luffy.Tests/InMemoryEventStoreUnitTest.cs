using System;
using System.Collections.Generic;
using System.Linq;
using Luffy.EventStore;
using Luffy.EventStore.Exceptions;
using Luffy.EventStore.InMemory;
using Xunit;

namespace Luffy.Tests
{
  [Collection("InMemory EventStore")]
  public class InMemoryStoreEventStoreUnitTest
  {
    [Fact(DisplayName = "Creating an empty store")]
    public void creates_an_empty_event_store()
    {
      var eventStore = new InMemoryEventStore();
      Assert.True(eventStore.IsEmpty(), "event store should be empty");
    }

    [Fact(DisplayName = "Appending events when the stream must be present")]
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

    [Fact(DisplayName = "Appending events when the stream must be absent")]
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

    [Fact(DisplayName = "Appending events with a wrong expected revision")]
    public void appending_events_with_a_wrong_expected_revision()
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

      Assert.Throws<ExpectedStreamRevisionException>(() => eventStore.AppendToStream(StreamRevision.ToStreamRevision(10), "stream-1",
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

    [Fact(DisplayName = "Appending events")]
    public void appending_events()
    {
      var eventStore = new InMemoryEventStore();

      var response = eventStore.AppendToStream(StreamState.Any, "stream-1", new List<IEventData>
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

      var expected = StreamRevision.ToStreamRevision(2);

      Assert.Equal(expected, response.NextExpectedStreamRevision);
    }

    [Fact(DisplayName = "Reading events from a stream")]
    public void reading_events_from_a_stream()
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
      Assert.Equal(StreamRevision.ToStreamRevision(0), events.ElementAt(0).StreamEventRevision);

      Assert.Equal("CartItemAdded", events.ElementAt(1).Type);
      Assert.Equal(StreamRevision.ToStreamRevision(1), events.ElementAt(1).StreamEventRevision);

      Assert.Equal("CartItemAdded", events.ElementAt(2).Type);
      Assert.Equal(StreamRevision.ToStreamRevision(2), events.ElementAt(2).StreamEventRevision);
    }
  }
}
