using Cerm.Lifetime.Event;
using Cerm.Render.Component;
using Cerm.Render.Interfaces;

namespace Cerm.Render.Screen
{
    public class ScreenLayer : IContainer
    {
        public int ActualX => 0;
        public int ActualY => 0;
        public int ActualWidth => Console.WindowWidth;

        public int ActualHeight => Console.WindowHeight;

        public List<ComponentBase> Children { get; set; }

        

        public ScreenLayer()
        {
            Children = new List<ComponentBase>();
        }

        public void Add(ComponentBase child)
        {
            Children.Add(child);
            child.Parent = this;
            EventBus.Instance.Publish(new StructureChangedEvent(this));
        }

        public void Remove(ComponentBase child)
        {
            if (Children.Remove(child))
            {
                EventBus.Instance.Publish(new StructureChangedEvent(this));
            }
        }
    }
}