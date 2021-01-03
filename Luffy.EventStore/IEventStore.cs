using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Luffy.EventStore.Responses;

namespace Luffy.EventStore
{
  public interface IEventStore
  {
    public Task<IAppendToStreamResponse> AppendToStream(
      string streamName,
      StreamState expectedState,
      IEnumerable<EventData> events,
      CancellationToken cancellationToken
    );

    Task<IReadStreamEventsResponse> ReadStream(
      ReadDirection readDirection,
      string streamName,
      StreamPosition revision,
      long maxCount,
      CancellationToken cancellationToken
    );
  }
}
