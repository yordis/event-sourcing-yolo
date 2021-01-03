using System;
using System.Text;
using System.Threading.Tasks;
using EventStore.Client;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using EventData = EventStore.Client.EventData;

namespace Luffy
{
  class Program
  {
    //public class UserCreated
    //{
    //  public Guid UserId { get; }
    //  public string UserName { get; }

    //  public UserCreated(Guid userId, string userName)
    //  {
    //    UserId = userId;
    //    UserName = userName;
    //  }
    //}

    class TestEvent
    {
      public string EntityId { get; }
      public string ImportantData { get; }

      public TestEvent(string entityId, string importantData)
      {
        EntityId = entityId;
        ImportantData = importantData;
      }
    }

    static void Main(string[] args)
    {
      MainAsync().GetAwaiter().GetResult();
    }

    private static async Task MainAsync()
    {
      var settings =
        EventStoreClientSettings.Create("esdb://localhost:2113?Tls=true");
      var client = new EventStoreClient(settings);

      var connection = EventStoreConnection.Create(
        new Uri("tcp://admin:changeit@localhost:1113")
      );
      await connection.ConnectAsync();

      var evt = new TestEvent(Guid.NewGuid().ToString("N"), "I wrote my first event!");

      const string streamName = "newstream";
      const string eventType  = "event-type";
      const string data       = "{ \"a\":\"2\"}";
      const string metadata   = "{}";

      var eventPayload = new EventData(
        eventId: EventStore.Client.Uuid.NewUuid(),
        type: eventType,
        true,
        Encoding.UTF8.GetBytes(data),
        Encoding.UTF8.GetBytes(metadata)
      );

      connection.ReadStreamEventsForwardAsync()

      // var result = await connection.AppendToStreamAsync(streamName, ExpectedVersion.Any, eventPayload);


      var eventData = new EventData(
        Uuid.NewUuid(),
        "TestEvent",
        JsonSerializer.SerializeToUtf8Bytes(evt)
      );

      var r = await client.AppendToStreamAsync(
        "some-stream",
      StreamState.Any,
        new[] {eventData}
      );

      return;
    }

    //private static async void Pepeg()
    //{
    //  NpgsqlConnection conn = await createDatabaseConnection();
    //  var eventStore = createEventStoreDatabase(conn);

    //  var streamId = Guid.NewGuid();
    //  var @event = new UserCreated(streamId, "John Doe");

    //  eventStore.AppendEvent<UserCreated>(streamId, @event);

    //  var streamState = eventStore.GetStreamState(streamId);

    //  Console.WriteLine("Hello World!");
    //}

    //private static PostgresEventStore createEventStoreDatabase(NpgsqlConnection conn)
    //{
    //  var eventStore = new PostgresEventStore(conn);
    //  eventStore.Init();
    //  return eventStore;
    //}

    //private static async Task<NpgsqlConnection> createDatabaseConnection()
    //{
    //  var builder = new NpgsqlConnectionStringBuilder();
    //  builder.Host = "localhost";
    //  builder.Username = "postgres";
    //  builder.Password = "";
    //  builder.Database = "luffy_eventstore_dev";
    //  builder.Port = 5432;
    //  var conn = new NpgsqlConnection(builder.ConnectionString);
    //  conn.Open();
    //  return conn;
    //}
  }
}
