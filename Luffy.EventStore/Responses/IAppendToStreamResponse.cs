namespace Luffy.EventStore.Responses
{
  public interface IAppendToStreamResponse
  {
    StreamRevision NextExpectedStreamRevision { get; }
  }
}
