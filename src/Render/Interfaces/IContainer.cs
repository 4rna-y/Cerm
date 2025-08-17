using System;
using Cerm.Render.Component;

namespace Cerm.Render.Interfaces
{
    public interface IContainer
    {
        public int ActualX { get; }
        public int ActualY { get; }
        public int ActualWidth { get; }
        public int ActualHeight { get; }
        public void Add(ComponentBase child);
        public void Remove(ComponentBase child);
        public List<ComponentBase> Children { get; }
    }
}
