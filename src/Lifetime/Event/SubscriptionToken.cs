using System;

namespace Cerm.Lifetime.Event
{
    internal sealed class SubscriptionToken : IDisposable
    {
        private readonly Action _unsubscribe;
        private volatile bool _disposed;

        public SubscriptionToken(Action unsubscribe)
        {
            _unsubscribe = unsubscribe;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                _unsubscribe?.Invoke();
            }
        }
    }
}