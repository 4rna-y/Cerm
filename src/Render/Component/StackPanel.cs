using Cerm.Lifetime.Event;
using Cerm.Render.Interfaces;
using Cerm.Render.Component.Layout;

namespace Cerm.Render.Component
{
    public class StackPanel : ComponentBase, IContainer
    {
        public Orientation StackOrientation { get; }
        public List<ComponentBase> Children { get; }
        public StackPanel(Orientation orientation, LayoutValue x, LayoutValue y, PositionAnchor anchor, LayoutValue width, LayoutValue height) : base(x, y, anchor, width, height, Color.Black, Color.Black)
        {
            StackOrientation = orientation;
            Children = new List<ComponentBase>();
        }

        public void Add(ComponentBase child)
        {
            Children.Add(child);
            child.Parent = this;
            EventBus.Instance.Publish(new StructureChangedEvent(this));
            InvalidateLayout();
        }

        public void Remove(ComponentBase child)
        {
            Children.Remove(child);
            child.Parent = null;
            EventBus.Instance.Publish(new StructureChangedEvent(this));
            InvalidateLayout();
        }

        public override void EnsureLayout()
        {
            if (!IsLayoutValid)
            {
                CalculateActualLayout();

                ArrangeChildren();
                IsLayoutValid = true;
            }
        }

        private void ArrangeChildren()
        {
            int offset = 0;
            if (StackOrientation == Orientation.Horizontal)
            {
                for (int i = 0; i < Children.Count; i++)
                {
                    ComponentBase child = Children[i];
                    if (!child.IsVisible) continue;

                    child.Layout.X = LayoutValue.Fixed(offset);
                    child.Layout.Y = LayoutValue.Fixed(0);
                    child.Layout.Height = LayoutValue.Fixed(ActualHeight);
                    
                    child.EnsureLayout();
                    offset += child.ActualWidth;

                    if (offset > ActualWidth)
                    {
                        child.IsVisible = false;
                    }
                    else
                    {
                        child.RequiredRedraw = true;
                    }
                }
            }
            else
            {
                for (int i = 0; i < Children.Count; i++)
                {
                    ComponentBase child = Children[i];
                    if (!child.IsVisible) continue;

                    child.Layout.X = LayoutValue.Fixed(0);
                    child.Layout.Y = LayoutValue.Fixed(offset);
                    child.Layout.Width = LayoutValue.Fixed(ActualWidth);
                    
                    child.EnsureLayout();
                    offset += child.ActualHeight;

                    if (offset > ActualHeight)
                    {
                        child.IsVisible = false;
                    }
                    else
                    {
                        child.RequiredRedraw = true;
                    }
                }
            }
        }

        public override void Render()
        {
            //nop
        }
    }
}