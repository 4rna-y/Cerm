using Cerm.Lifetime.Event;
using Cerm.Render.Interfaces;
using Cerm.Render.Component.Layout;

namespace Cerm.Render.Component
{
    public class Button : Border, IContainer, IFocusable
    {
        private string text;
        private TextLine textComponent;

        public List<ComponentBase> Children { get; }
        
        public Color UnfocusedForeground { get; }
        public Color UnfocusedBackground { get; }
        public Color FocusedForeground { get; }
        public Color FocusedBackground { get; }

        public string Text
        {
            get => text;
            set
            {
                text = value;
                RequiredRedraw = true;
            }
        }

        public Button(
            string text, BorderCorners corners,
            LayoutValue x, LayoutValue y, PositionAnchor anchor,
            LayoutValue width, LayoutValue height,
            Color focusedForeground, Color focusedBackground)
            : base(corners, x, y, anchor, width, height, focusedForeground, focusedBackground)
        {
            this.text = text;
            Children = new List<ComponentBase>();
            FocusedForeground = focusedForeground;
            FocusedBackground = focusedBackground;
            UnfocusedForeground = focusedForeground.AddBrightness(-0.5);
            UnfocusedBackground = focusedBackground.AddBrightness(-0.5);

            textComponent = new TextLine(text, LayoutValue.Proportional(0.5), LayoutValue.Proportional(0.5), PositionAnchor.CenterMiddle, UnfocusedForeground, UnfocusedBackground);
            
            Add(textComponent);
            Foreground = UnfocusedForeground;
            Background = UnfocusedBackground;
        }

        public void Focus()
        {
            Foreground = FocusedForeground;
            Background = FocusedBackground;
            textComponent.Foreground = FocusedForeground;
            textComponent.Background = FocusedBackground;
        }

        public void Unfocus()
        {
            Foreground = UnfocusedForeground;
            Background = UnfocusedBackground;
            textComponent.Foreground = UnfocusedForeground;
            textComponent.Background = UnfocusedBackground;
        }

        public void Add(ComponentBase child)
        {
            Children.Add(child);
            child.Parent = this;
            EventBus.Instance.Publish(new StructureChangedEvent(this));
        }

        public void Remove(ComponentBase child)
        {
            Children.Remove(child);
            child.Parent = null;
            EventBus.Instance.Publish(new StructureChangedEvent(this));
        }

        public override void InvalidateLayout()
        {
            base.InvalidateLayout();
            foreach (var child in Children)
            {
                child.InvalidateLayout();
            }
        }

        public override void Render()
        {
            base.Render();
        }

       
    }
}