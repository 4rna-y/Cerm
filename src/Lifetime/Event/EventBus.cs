using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace Cerm.Lifetime.Event
{
    public sealed class EventBus : IEventBus
    {
        public static IEventBus Instance { get; private set; }
        
        private readonly ConcurrentDictionary<Type, HandlerList> handlers;

        static EventBus()
        {
            Instance = new EventBus();
        }

        private EventBus()
        {
            handlers = new ConcurrentDictionary<Type, HandlerList>();
        }

        public IDisposable Subscribe<T>(Action<T> handler) where T : EventDataBase
        {
            var handlerWrapper = new HandlerWrapper<T>(handler);
            var handlerList = handlers.GetOrAdd(typeof(T), _ => new HandlerList());
            
            handlerList.Add(handlerWrapper);
            
            return new SubscriptionToken(() => handlerList.Remove(handlerWrapper));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Publish<T>(T eventData) where T : EventDataBase
        {
            if (!handlers.TryGetValue(typeof(T), out var handlerList)) 
                return;

            var hs = handlerList.GetSnapshot();
            
            foreach (var wrapper in hs)
            {
                ((HandlerWrapper<T>)wrapper).Invoke(eventData);
            }
        }
    }
}
