namespace Cerm.Render.Component
{
    public class LayoutData
    {
        private PositionAnchor anchor;
        private LayoutValue x;
        private LayoutValue y;
        private LayoutValue width;
        private LayoutValue height;
        private Action onPositionAnchorChanged;
        private Action onPositionChanged;
        private Action onSizeChanged;

        public PositionAnchor Anchor
        {
            get => this.anchor;
            set
            {
                this.anchor = value;
                onPositionAnchorChanged();
            }
        }
        public LayoutValue X
        {
            get => this.x;
            set
            {
                this.x = value;
                onPositionChanged();
            }
        }
        public LayoutValue Y
        {
            get => this.y;
            set
            {
                this.y = value;
                onPositionChanged();
            }
        }
        public LayoutValue Width
        {
            get => this.width;
            set
            {
                this.width = value;
                onSizeChanged();
            }
        }
        public LayoutValue Height
        {
            get => this.height;
            set
            {
                this.height = value;
                onSizeChanged();
            }
        }

        public LayoutData(LayoutValue x, LayoutValue y, PositionAnchor anchor, LayoutValue width, LayoutValue height, Action onPositionAnchorChanged, Action onPositionChanged, Action onSizeChanged)
        {
            this.anchor = anchor;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.onPositionAnchorChanged = onPositionAnchorChanged;
            this.onPositionChanged = onPositionChanged;
            this.onSizeChanged = onSizeChanged; 
        }
    }
}
