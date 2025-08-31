using Cerm.Lifetime.Event;
using Cerm.Render.Component;
using Cerm.Render.Interfaces;

namespace Cerm.Render.Screen
{
    public class ScreenLayer : IContainer
    {
        private int focusingIndex = 0;
        private int width;
        private int height;

        public int ActualX => 0;
        public int ActualY => 0;
        public int ActualWidth => width;
        public int ActualHeight => height;

        public List<ComponentBase> Children { get; set; }

        private List<ComponentBase> flatComponents;

        public ScreenLayer()
        {
            Children = new List<ComponentBase>();
            width = Console.WindowWidth;
            height = Console.WindowHeight;
            flatComponents = new List<ComponentBase>();

            EventBus.Instance.Subscribe<StructureChangedEvent>(OnStructureChanged);
        }

        public void SetSize(int newWidth, int newHeight)
        {
            if (width != newWidth || height != newHeight)
            {
                width = newWidth;
                height = newHeight;
                InvalidateLayout();
            }
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

        public void EnsureLayout()
        {
            foreach (var child in Children)
            {
                child.EnsureLayout();
            }
        }

        public virtual void InvalidateLayout()
        {
            foreach (var child in Children)
            {
                child.InvalidateLayout();
            }
        }

        protected void Focus(int index)
        {
            if (index >= flatComponents.Count) return;

            IFocusable? pfc = flatComponents[focusingIndex] as IFocusable;
            pfc?.Unfocus();

            IFocusable? fc = flatComponents[index] as IFocusable;
            if (fc == null) return;

            fc.Focus();
            focusingIndex = index;
        }

        public void FocusToFirst()
        {
            

            for (int i = 0; i < flatComponents.Count; i++)
            {
                if (flatComponents[i] is IFocusable)
                {
                    Focus(i);
                    return;
                }
            }
        }

        public void FocusToNext()
        {
            if (focusingIndex + 1 >= flatComponents.Count) return;
            for (int i = focusingIndex + 1; i < flatComponents.Count; i++)
            {
                if (flatComponents[i] is IFocusable)
                {
                    Focus(i);
                    return;
                }
            }
        }

        public void FocusToPrevious()
        {
            if (focusingIndex - 1 < 0) return;
            for (int i = focusingIndex - 1; i >= 0; i--)
            {
                if (flatComponents[i] is IFocusable)
                {
                    Focus(i);
                    return;
                }
            }
        }

        public List<ComponentBase> GetComponentsAsFlat() => flatComponents;

        public void CollectComponentsAsFlat()
        {
            flatComponents.Clear();
            for (int i = 0; i < Children.Count; i++)
            {
                flatComponents.Add(Children[i]);
                if (Children[i] is IContainer childContainer)
                {
                    CollectComponentsAsFlat(childContainer);
                }
            }
        }

        private void CollectComponentsAsFlat(IContainer container)
        {
            for (int i = 0; i < container.Children.Count; i++)
            {
                flatComponents.Add(container.Children[i]);
                if (container.Children[i] is IContainer childContainer)
                {
                    CollectComponentsAsFlat(childContainer);
                }
            }
        }

        private void OnStructureChanged(StructureChangedEvent e)
        {

        }
    }
}