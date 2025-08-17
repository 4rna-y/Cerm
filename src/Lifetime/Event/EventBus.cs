using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace Cerm.Lifetime.Event
{
    public sealed class EventBus : IEventBus
    {
        public static IEventBus Instance { get; } = new EventBus();
        
        private readonly ConcurrentDictionary<Type, HandlerList> _handlers = new();
        private readonly object _cleanupLock = new();
        
        private EventBus() { }

        public IDisposable Subscribe<T>(Action<T> handler) where T : EventDataBase
        {
            var handlerWrapper = new HandlerWrapper<T>(handler);
            var handlerList = _handlers.GetOrAdd(typeof(T), _ => new HandlerList());
            
            handlerList.Add(handlerWrapper);
            
            return new SubscriptionToken(() => handlerList.Remove(handlerWrapper));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Publish<T>(T eventData) where T : EventDataBase
        {
            if (!_handlers.TryGetValue(typeof(T), out var handlerList)) 
                return;

            var handlers = handlerList.GetSnapshot();
            
            foreach (var wrapper in handlers)
            {
                try
                {
                    ((HandlerWrapper<T>)wrapper).Invoke(eventData);
                }
                catch
                {
                }
            }
            
            if (handlers.Length > 0 && handlerList.NeedsCleanup)
            {
                lock (_cleanupLock)
                {
                    handlerList.Cleanup();
                }
            }
        }
    }
}
