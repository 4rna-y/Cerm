using System;
using System.Collections.Generic;

namespace Cerm.Lifetime.Event
{
    internal sealed class HandlerList
    {
        private volatile HandlerWrapper[] _handlers = Array.Empty<HandlerWrapper>();
        private volatile bool _needsCleanup;
        private readonly object _lock = new();

        public bool NeedsCleanup => _needsCleanup;

        public void Add(HandlerWrapper handler)
        {
            lock (_lock)
            {
                var newHandlers = new HandlerWrapper[_handlers.Length + 1];
                Array.Copy(_handlers, newHandlers, _handlers.Length);
                newHandlers[_handlers.Length] = handler;
                _handlers = newHandlers;
            }
        }

        public void Remove(HandlerWrapper handler)
        {
            lock (_lock)
            {
                var index = Array.IndexOf(_handlers, handler);
                if (index >= 0)
                {
                    var newHandlers = new HandlerWrapper[_handlers.Length - 1];
                    Array.Copy(_handlers, 0, newHandlers, 0, index);
                    Array.Copy(_handlers, index + 1, newHandlers, index, _handlers.Length - index - 1);
                    _handlers = newHandlers;
                }
            }
        }

        public HandlerWrapper[] GetSnapshot()
        {
            return _handlers;
        }

        public void Cleanup()
        {
            lock (_lock)
            {
                var activeHandlers = new List<HandlerWrapper>();
                foreach (var handler in _handlers)
                {
                    if (handler.IsAlive)
                        activeHandlers.Add(handler);
                }
                _handlers = activeHandlers.ToArray();
                _needsCleanup = false;
            }
        }
    }
}