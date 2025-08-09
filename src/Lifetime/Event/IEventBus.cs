using System;

namespace Cerm.Lifetime.Event
{
    public interface IEventBus
    {
        void Subscribe<T>(Action<T> eventHandler) where T : EventDataBase;
        void Unsubscribe<T>() where T : EventDataBase;
        void Publish<T>(T eventData) where T : EventDataBase;
    }
}
