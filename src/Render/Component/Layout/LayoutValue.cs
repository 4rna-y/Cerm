namespace Cerm.Render.Component.Layout
{
    public struct LayoutValue
    {
        public static LayoutValue Proportional(double value) => new LayoutValue(value, LayoutMode.Proportional);
        public static LayoutValue Fixed(int value) => new LayoutValue(value, LayoutMode.Fixed);
        public static LayoutValue Auto() => new LayoutValue(double.NaN, LayoutMode.Auto);
        
        public double Value { get; }
        public LayoutMode Mode { get; }

        public LayoutValue(double value, LayoutMode mode)
        {
            this.Value = value;
            this.Mode = mode;
        }
    }
}