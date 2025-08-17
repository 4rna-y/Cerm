using System;
using System.Buffers;
using Cerm.Render.Interfaces;

namespace Cerm.Render.Component
{
    public abstract class ComponentBase : IDisposable
    {
        private static readonly ArrayPool<char> pool = ArrayPool<char>.Shared;

        private Color foreground;
        private Color background;
        private IContainer? parent = null;

        protected char[] buffer;
        protected int actualX;
        protected int actualY;
        protected int actualWidth;
        protected int actualHeight;
        
        public char[] GetBuffer() => buffer;
        public int ActualX => actualX;
        public int ActualY => actualY;
        public int ActualWidth => actualWidth;
        public int ActualHeight => actualHeight;

        public Guid Id { get; }
        public LayoutData Layout { get; }
        public Color Foreground
        {
            get => foreground;
            set
            {
                RequiredRedraw = true;
                foreground = value;
            }
        }
        public Color Background
        {
            get => background;
            set
            {
                RequiredRedraw = true;
                background = value;
            }
        }
        public IContainer? Parent
        {
            get => parent;
            set
            {
                parent = value;
                CalculateActualLayout();
            }
        }
        public bool RequiredRedraw { get; set; } = true;

        public ComponentBase(LayoutValue x, LayoutValue y, PositionAnchor anchor, LayoutValue width, LayoutValue height, Color foreground, Color background)
        {
            Id = Guid.NewGuid();
            Layout = new LayoutData(x, y, anchor, width, height, OnPositionAnchorChanged, OnPositionChanged, OnSizeChanged);
            this.foreground = foreground;
            this.background = background;

            buffer = pool.Rent(Math.Max(1, actualWidth * actualHeight));
        }

        public abstract void Render();

        private void CalculateActualLayout()
        {
            PositionAnchor anchor = Layout.Anchor;
            LayoutValue x = Layout.X;
            LayoutValue y = Layout.Y;
            LayoutValue w = Layout.Width;
            LayoutValue h = Layout.Height;

            int parentActualX = 0;
            int parentActualY = 0;
            int parentActualWidth = 0;
            int parentActualHeight = 0;

            double xAnchor = 0;
            double yAnchor = 0;

            GetAnchorRatio(anchor, ref xAnchor, ref yAnchor);

            if (parent != null)
            {
                parentActualX = Parent!.ActualX;
                parentActualY = Parent!.ActualY;
                parentActualWidth = Parent!.ActualWidth;
                parentActualHeight = Parent!.ActualHeight;
            }

            if (w.Mode == LayoutMode.Proportional) actualWidth = (int)(parentActualWidth * w.Value);
            if (h.Mode == LayoutMode.Proportional) actualHeight = (int)(parentActualHeight * h.Value);
            if (w.Mode == LayoutMode.Fixed) actualWidth = (int)w.Value;
            if (h.Mode == LayoutMode.Fixed) actualHeight = (int)h.Value;

            if (x.Mode == LayoutMode.Proportional) actualX = (int)(parentActualX + (x.Value * parentActualWidth) + (xAnchor * actualWidth));
            if (y.Mode == LayoutMode.Proportional) actualY = (int)(parentActualY + (y.Value * parentActualHeight) + (yAnchor * actualHeight));
            if (x.Mode == LayoutMode.Fixed) actualX = (int)(parentActualX + x.Value + (xAnchor * actualWidth));
            if (y.Mode == LayoutMode.Fixed) actualY = (int)(parentActualY + y.Value + (yAnchor * actualHeight)); 
        }

        private void GetAnchorRatio(PositionAnchor anchor, ref double xAnchor, ref double yAnchor)
        {
            if (anchor == PositionAnchor.CenterTop)
            {
                xAnchor = 0.5;
            }
            else
            if (anchor == PositionAnchor.RightTop)
            {
                xAnchor = 1;
            }
            else
            if (anchor == PositionAnchor.LeftMiddle)
            {
                yAnchor = 0.5;
            }
            else
            if (anchor == PositionAnchor.CenterMiddle)
            {
                xAnchor = 0.5;
                yAnchor = 0.5;
            }
            else
            if (anchor == PositionAnchor.RightMiddle)
            {
                xAnchor = 1;
                yAnchor = 0.5;
            }
            else
            if (anchor == PositionAnchor.LeftBottom)
            {
                yAnchor = 1;
            }
            else
            if (anchor == PositionAnchor.CenterBottom)
            {
                xAnchor = 0.5;
                yAnchor = 1;
            }
            else
            if (anchor == PositionAnchor.RightBottom)
            {
                xAnchor = 1;
                yAnchor = 1;
            }
        }

        private void OnPositionChanged()
        {
            RequiredRedraw = true;
        }

        private void OnPositionAnchorChanged()
        {
            RequiredRedraw = true;
            CalculateActualLayout();
        }

        private void OnSizeChanged()
        {
            pool.Return(buffer, true);
            CalculateActualLayout();
            buffer = pool.Rent(actualHeight * actualWidth);
            RequiredRedraw = true;
        }

        public void Dispose()
        {
            pool.Return(buffer, true);
        }
    }
}
