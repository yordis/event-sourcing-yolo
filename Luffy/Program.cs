using System;
using System.Collections.Generic;
using EventStore.Client;
using Luffy.Events;
using System.Text.Json;
using EventData = EventStore.Client.EventData;

var settings = EventStoreClientSettings.Create("esdb://localhost:2113?Tls=true");
var client = new EventStoreClient(settings);
var evt = new TestEvent
{
  EntityId = Guid.NewGuid().ToString("N"),
  ImportantData = "I wrote my first event!",
};

var eventData = new EventData(
  Uuid.NewUuid(),
  "TestEvent",
  JsonSerializer.SerializeToUtf8Bytes(evt)
);
var r = await client.AppendToStreamAsync(
  "some-stream",
  StreamState.Any,
  new List<EventData> {eventData}
);
return 0;
