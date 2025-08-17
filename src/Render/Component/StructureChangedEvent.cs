using Cerm.Lifetime.Event;
using Cerm.Render.Interfaces;

namespace Cerm.Render.Component
{
    public class StructureChangedEvent : EventDataBase
    {
        public IContainer Container { get; set; }
        public StructureChangedEvent(IContainer container)
        {
            Container = container;
        }
    }
}