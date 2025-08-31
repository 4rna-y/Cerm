namespace Cerm.Lifetime.Event
{
    internal abstract class HandlerWrapper
    {
        public abstract bool IsAlive { get; }
    }

    internal sealed class HandlerWrapper<T> : HandlerWrapper where T : EventDataBase
    {
        private readonly Action<T> _handler;
        private readonly int _hashCode;

        public HandlerWrapper(Action<T> handler)
        {
            _handler = handler;
            _hashCode = handler.GetHashCode();
        }

        public override bool IsAlive => true;

        public void Invoke(T eventData)
        {
            _handler(eventData);
        }

        public override int GetHashCode() => _hashCode;
        
        public override bool Equals(object? obj)
        {
            if (obj is HandlerWrapper<T> other)
            {
                return ReferenceEquals(_handler, other._handler);
            }
            return false;
        }
    }
}