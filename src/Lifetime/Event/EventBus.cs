using System;
using System.Collections.Concurrent;

namespace Cerm.Lifetime.Event
{
    public class EventBus : IEventBus
    {
        private readonly ConcurrentDictionary<Type, ConcurrentBag<Delegate>> handlers;

        public EventBus()
        {
            handlers = new ConcurrentDictionary<Type, ConcurrentBag<Delegate>>();
        }

        public void Subscribe<T>(Action<T> eventHandler) where T : EventDataBase
        {
            handlers.AddOrUpdate(
                typeof(T),
                new ConcurrentBag<Delegate> { eventHandler },
                (k, e) =>
                {
                    e.Add(eventHandler);
                    return e;
                });
        }

        public void Unsubscribe<T>() where T : EventDataBase
        {
            handlers.Remove(typeof(T), out _);
        }

        public void Publish<T>(T eventData) where T : EventDataBase
        {
            if (handlers.TryGetValue(typeof(T), out ConcurrentBag<Delegate>? eventHandlers))
            {
                foreach (Action<T> handler in eventHandlers.Cast<Action<T>>())
                {
                    handler(eventData);
                }
            }
        }
    }
}
