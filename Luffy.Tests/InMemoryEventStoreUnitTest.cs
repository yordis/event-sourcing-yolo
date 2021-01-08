using System;
using System.Linq;
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
          var events = eventStore.ReadStream(1, "hello", 1L, 1L);
          Assert.False(events.Any(), "event store should be empty");
        }
    }
}
