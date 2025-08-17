using System;

namespace Cerm.Lifetime.Event
{
    internal abstract class HandlerWrapper
    {
        public abstract bool IsAlive { get; }
    }

    internal sealed class HandlerWrapper<T> : HandlerWrapper where T : EventDataBase
    {
        private readonly WeakReference<Action<T>> _handlerRef;
        private readonly int _hashCode;

        public HandlerWrapper(Action<T> handler)
        {
            _handlerRef = new WeakReference<Action<T>>(handler);
            _hashCode = handler.GetHashCode();
        }

        public override bool IsAlive => _handlerRef.TryGetTarget(out _);

        public void Invoke(T eventData)
        {
            if (_handlerRef.TryGetTarget(out var handler))
                handler(eventData);
        }

        public override int GetHashCode() => _hashCode;
        
        public override bool Equals(object obj)
        {
            if (obj is HandlerWrapper<T> other && 
                _handlerRef.TryGetTarget(out var thisHandler) &&
                other._handlerRef.TryGetTarget(out var otherHandler))
            {
                return ReferenceEquals(thisHandler, otherHandler);
            }
            return false;
        }
    }
}