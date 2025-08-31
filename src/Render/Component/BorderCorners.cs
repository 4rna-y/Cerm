namespace Cerm.Render.Component
{
    public struct BorderCorners
    {
        public bool Doubled { get; }
        public BorderType LeftTop { get; }
        public BorderType RightTop { get; }
        public BorderType RightBottom { get; }
        public BorderType LeftBottom { get; }

        public static BorderCorners Round()
        {
            return new BorderCorners(BorderType.Round, BorderType.Round, BorderType.Round, BorderType.Round);
        }

        public BorderCorners(BorderType leftTop, BorderType rightTop, BorderType rightBottom, BorderType leftBottom)
        {
            Doubled = false;
            LeftTop = leftTop;
            RightTop = rightTop;
            RightBottom = rightBottom;
            LeftBottom = leftBottom;
        }

        public BorderCorners(bool doubled = false)
        {
            Doubled = doubled;
            if (doubled)
            {
                LeftTop = BorderType.Double;
                RightTop = BorderType.Double;
                RightBottom = BorderType.Double;
                LeftBottom = BorderType.Double;
            }
            else
            {
                LeftTop = BorderType.Square;
                RightTop = BorderType.Square;
                RightBottom = BorderType.Square;
                LeftBottom = BorderType.Square;
            }
        }
    }
}