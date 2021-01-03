namespace Luffy.EventStore
{
    public class Event
    {
        private readonly object _data;
        private readonly object _metaData;

        public Event(object data, object metaData)
        {
            this._data = data;
            this._metaData = metaData;
        }

        public virtual string GetEventType()
        {
            return GetType().AssemblyQualifiedName;
        }

        public virtual object GetData()
        {
            return _data;
        }

        public virtual object GetMetaData()
        {
            return _metaData;
        }
    }
}

