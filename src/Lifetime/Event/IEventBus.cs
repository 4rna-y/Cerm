using System;

namespace Cerm.Lifetime.Event
{
    public interface IEventBus
    {
        IDisposable Subscribe<T>(Action<T> handler) where T : EventDataBase;
        void Publish<T>(T eventData) where T : EventDataBase;
    }
}