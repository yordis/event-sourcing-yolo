using System;
using System.Collections;
using System.Text.Json;
using Luffy.EventStore;
using Npgsql;

namespace Luffy
{
  public class PostgresEventStore : IDisposable, IEventStore
  {
    private readonly NpgsqlConnection connection;

    public PostgresEventStore(NpgsqlConnection connection)
    {
      this.connection = connection;
    }

    public bool AppendEvent<TStream>(Guid streamId, object @event, long? expectedVersion = null)
    {
      var command = new NpgsqlCommand(@"
        SELECT append_event(
          @Id,
          @Data::jsonb,
          @Type,
          @StreamId,
          @StreamType,
          @ExpectedVersion
        )
      ", connection);

      command.Parameters.AddWithValue("Id", Guid.NewGuid());
      command.Parameters.AddWithValue("Data", JsonSerializer.Serialize(@event));
      command.Parameters.AddWithValue("Type", @event.GetType().AssemblyQualifiedName);
      command.Parameters.AddWithValue("StreamId", streamId);
      command.Parameters.AddWithValue("StreamType", typeof(TStream).AssemblyQualifiedName);
      command.Parameters.AddWithValue("ExpectedVersion", expectedVersion ?? 0);

      return command.ExecuteNonQuery() == 1;
    }

    public void Dispose()
    {
      connection.Dispose();
    }

    public IEnumerable GetEvents(Guid streamId)
    {
      const string query = @"
        SELECT id, data, stream_id, type, version, created
        FROM events
        WHERE stream_id = @streamId
        ORDER BY version
      ";

      using var command = new NpgsqlCommand(query, connection);
      command.Parameters.AddWithValue("streamId", streamId);

      var events = new ArrayList();

      using (var reader = command.ExecuteReader(System.Data.CommandBehavior.SingleResult)) {
        while(reader.Read())
        {
          //  I dont know what to do anymore
        }
      }

      return events;
    }

    public StreamState GetStreamState(Guid streamId)
    {
      throw new NotImplementedException();
    }

    public void Init()
    {
      CreateStreamsTable();
      CreateEventsTable();
      CreateAppendEventFunction();
    }

    private void CreateAppendEventFunction()
    {
      const string query = @"
        CREATE OR REPLACE FUNCTION append_event(
          id                        uuid,
          data                      jsonb,
          type                      text,
          stream_id                 uuid,
          stream_type               text,
          expected_stream_version   bigint    default null
        ) RETURNS boolean

        LANGUAGE plpgsql
        AS $$
        DECLARE
          stream_version int;
        BEGIN
            -- 1. Get stream version
            SELECT
                version INTO stream_version
            FROM streams as s
            WHERE
                s.id = stream_id FOR UPDATE;

            -- 2. If stream doesn't exist - create new one with version 0
            IF stream_version IS NULL
            THEN
                stream_version := 0;

                INSERT INTO streams (id, type, version)
                VALUES (stream_id, stream_type, stream_version);
            END IF;

            -- 3. Check optimistic concurrency - return false if expected stream version is different than stream version
            IF expected_stream_version IS NOT NULL
               AND stream_version != expected_stream_version
            THEN
                RETURN FALSE;
            END IF;

            -- 4. Increment stream_version
            stream_version := stream_version + 1;

            -- 5. Append event to events table
            INSERT INTO events
                (id, data, stream_id, type, version)
            VALUES
                (id, data, stream_id, type, stream_version);

            -- 6. Update stream version in stream table
            UPDATE streams as s
                SET version = stream_version
            WHERE
                s.id = stream_id;

            RETURN TRUE;
        END;
        $$;
      ";

      using var command = new NpgsqlCommand(query, connection);
      command.ExecuteNonQuery();
    }

    private void CreateStreamsTable()
    {
      const string query = @"
        CREATE TABLE IF NOT EXISTS streams(
          id        UUID      NOT NULL    PRIMARY KEY,
          type      TEXT      NOT NULL,
          version   BIGINT    NOT NULL
        );
      ";

      using var command = new NpgsqlCommand(query, connection);
      command.ExecuteNonQuery();
    }

    private void CreateEventsTable()
    {
      const string query = @"
        CREATE TABLE IF NOT EXISTS events(
          id          UUID                        NOT NULL    PRIMARY KEY,
          data        JSONB                       NOT NULL,
          stream_id   UUID                        NOT NULL,
          type        TEXT                        NOT NULL,
          version     BIGINT                      NOT NULL,
          created     timestamp with time zone    NOT NULL    default (now()),

          FOREIGN KEY(stream_id) REFERENCES streams(id),

          CONSTRAINT events_stream_and_version UNIQUE(stream_id, version)
        );
      ";

      using var command = new NpgsqlCommand(query, connection);
      command.ExecuteNonQuery();
    }

    public T AggregateStream<T>(Guid streamId)
    {
      throw new NotImplementedException();
    }
  }
}
