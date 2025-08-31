using Cerm.Render.Component.Layout;

namespace Cerm.Render.Component
{
    public class Border : ComponentBase
    {
        protected readonly char[] leftTopCornerBorders = ['┌', '╭', '╔'];
        protected readonly char[] rightTopCornerBorders = ['┐', '╮', '╗'];
        protected readonly char[] leftBottomCornerBorders = ['└', '╰', '╚'];
        protected readonly char[] rightBottomCornerBorders = ['┘', '╯', '╝'];
        protected readonly char[] horizontalBorders = ['─', '─', '═'];
        protected readonly char[] verticalBorders = ['│', '│', '║'];
        
        private BorderCorners corners;
        public BorderCorners Corners
        {
            get => corners;
            set
            {
                corners = value;
                RequiredRedraw = true;
            }
        }

        public Border(
            BorderCorners corners,
            LayoutValue x,
            LayoutValue y,
            PositionAnchor anchor,
            LayoutValue width,
            LayoutValue height,
            Color foreground,
            Color background)
            : base(x, y, anchor, width, height, foreground, background)
        {
            this.corners = corners;
        }
        
        

        public override void Render()
        {
            int width = ActualWidth;
            int height = ActualHeight;

            if (width <= 0 || height <= 0 || buffer.Length < width * height) return;

            char leftTop = leftTopCornerBorders[corners.Doubled ? (int)BorderType.Double : (int)corners.LeftTop];
            char rightTop = rightTopCornerBorders[corners.Doubled ? (int)BorderType.Double : (int)corners.RightTop];
            char leftBottom = leftBottomCornerBorders[corners.Doubled ? (int)BorderType.Double : (int)corners.LeftBottom];
            char rightBottom = rightBottomCornerBorders[corners.Doubled ? (int)BorderType.Double : (int)corners.RightBottom];
            
            char topHorizontal = GetHorizontalBorder(corners.LeftTop, corners.RightTop);
            char bottomHorizontal = GetHorizontalBorder(corners.LeftBottom, corners.RightBottom);
            char leftVertical = GetVerticalBorder(corners.LeftTop, corners.LeftBottom);
            char rightVertical = GetVerticalBorder(corners.RightTop, corners.RightBottom);

            Span<char> span = buffer.AsSpan(0, width * height);
            span.Fill(' ');

            if (height == 1)
            {
                span[0] = leftTop;
                span.Slice(1, width - 2).Fill(topHorizontal);
                if (width > 1) span[width - 1] = rightTop;
                return;
            }

            span[0] = leftTop;
            span.Slice(1, width - 2).Fill(topHorizontal);
            span[width - 1] = rightTop;

            int leftIndex = width;
            int rightIndex = width + width - 1;
            int lastRowStart = (height - 1) * width;

            while (leftIndex < lastRowStart)
            {
                span[leftIndex] = leftVertical;
                span[rightIndex] = rightVertical;
                leftIndex += width;
                rightIndex += width;
            }

            span[lastRowStart] = leftBottom;
            span.Slice(lastRowStart + 1, width - 2).Fill(bottomHorizontal);
            span[lastRowStart + width - 1] = rightBottom;
        }

        private char GetHorizontalBorder(BorderType leftCorner, BorderType rightCorner)
        {
            if (corners.Doubled)
                return horizontalBorders[(int)BorderType.Double];
            
            BorderType dominantType = (BorderType)Math.Max((int)leftCorner, (int)rightCorner);
            return horizontalBorders[(int)dominantType];
        }

        private char GetVerticalBorder(BorderType topCorner, BorderType bottomCorner)
        {
            if (corners.Doubled)
                return verticalBorders[(int)BorderType.Double];
            
            BorderType dominantType = (BorderType)Math.Max((int)topCorner, (int)bottomCorner);
            return verticalBorders[(int)dominantType];
        }
    }
}
