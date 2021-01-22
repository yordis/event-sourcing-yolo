namespace Luffy.EventStore
{
  public interface IStream
  {
    public void AppendEvent(IRecordedEvent @event);
  }
}
