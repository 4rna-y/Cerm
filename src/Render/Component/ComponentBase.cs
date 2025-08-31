using System;
using System.Buffers;
using Cerm.Render.Interfaces;
using Cerm.Render.Component.Layout;

namespace Cerm.Render.Component
{
    public abstract class ComponentBase : IDisposable
    {
        private static readonly ArrayPool<char> pool = ArrayPool<char>.Shared;
        protected char[] buffer;
        public char[] GetBuffer() => buffer;

        public Guid Id { get; }
        public LayoutData Layout { get; }
        public int ActualX { get; protected set; }
        public int ActualY { get; protected set; }
        public int ActualWidth { get; protected set; }
        public int ActualHeight { get; protected set; }
        public Color Foreground
        {
            get => field;
            set
            {
                RequiredRedraw = true;
                field = value;
            }
        }
        public Color Background
        {
            get => field;
            set
            {
                RequiredRedraw = true;
                field = value;
            }
        }
        public IContainer? Parent
        {
            get => field;
            set
            {
                field = value;
                InvalidateLayout();
            }
        }
        public bool RequiredRedraw { get; set; } = true;
        public bool IsLayoutValid { get; set; }

        public bool IsVisible
        {
            get => field;
            set
            {
                field = value;
                InvalidateLayout();
            }
        } = true;

        public ComponentBase(LayoutValue x, LayoutValue y, PositionAnchor anchor, LayoutValue width, LayoutValue height, Color foreground, Color background)
        {
            Id = Guid.NewGuid();
            Layout = new LayoutData(x, y, anchor, width, height, OnPositionAnchorChanged, OnPositionChanged, OnSizeChanged);
            this.Foreground = foreground;
            this.Background = background;

            buffer = pool.Rent(Math.Max(1, ActualWidth * ActualHeight));
        }

        public abstract void Render();

        private protected void CalculateActualLayout()
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

            if (Parent != null)
            {
                parentActualX = Parent!.ActualX;
                parentActualY = Parent!.ActualY;
                parentActualWidth = Parent!.ActualWidth;
                parentActualHeight = Parent!.ActualHeight;
            }

            int oldWidth = ActualWidth;
            int oldHeight = ActualHeight;

            if (w.Mode == LayoutMode.Proportional) ActualWidth = (int)(parentActualWidth * w.Value);
            if (h.Mode == LayoutMode.Proportional) ActualHeight = (int)(parentActualHeight * h.Value);
            if (w.Mode == LayoutMode.Fixed) ActualWidth = (int)w.Value;
            if (h.Mode == LayoutMode.Fixed) ActualHeight = (int)h.Value;

            if (x.Mode == LayoutMode.Proportional) ActualX = (int)(parentActualX + (x.Value * parentActualWidth) - (xAnchor * ActualWidth));
            if (y.Mode == LayoutMode.Proportional) ActualY = (int)(parentActualY + (y.Value * parentActualHeight) - (yAnchor * ActualHeight));
            if (x.Mode == LayoutMode.Fixed) ActualX = (int)(parentActualX + x.Value - (xAnchor * ActualWidth));
            if (y.Mode == LayoutMode.Fixed) ActualY = (int)(parentActualY + y.Value - (yAnchor * ActualHeight));


            if (oldWidth != ActualWidth || oldHeight != ActualHeight)
            {
                pool.Return(buffer, true);
                buffer = pool.Rent(Math.Max(1, ActualWidth * ActualHeight));
            }
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

        public virtual void InvalidateLayout()
        {
            if (IsLayoutValid)
            {
                IsLayoutValid = false;
                RequiredRedraw = true;
                Parent?.InvalidateLayout();
            }
        }

        public virtual void EnsureLayout()
        {
            if (!IsLayoutValid)
            {
                if (IsVisible)
                {
                    CalculateActualLayout();
                }
                else
                {
                    ActualX = 0;
                    ActualY = 0;
                    ActualWidth = 0;
                    ActualHeight = 0;
                    pool.Return(buffer, true);
                    buffer = pool.Rent(1);
                }
                IsLayoutValid = true;
                
                if (this is IContainer container)
                {
                    foreach (var child in container.Children)
                    {
                        child.EnsureLayout();
                    }
                }
            }
        }

        private void OnPositionChanged()
        {
            InvalidateLayout();
        }

        private void OnPositionAnchorChanged()
        {
            InvalidateLayout();
        }

        private void OnSizeChanged()
        {
            InvalidateLayout();
        }

        public void Dispose()
        {
            pool.Return(buffer, true);
        }
    }
}
