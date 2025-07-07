namespace Cerm.Ui.Screen.Widget.Abstruct
{
    public abstract class WidgetBase
    {
        private int x;
        private int y;
        private int width;
        private int height;

        public int X
        {
            get => x;
            set => SetProperty(ref x, value);
        }

        public int Y
        {
            get => y;
            set => SetProperty(ref y, value);
        }

        public int Width
        {
            get => width;
            set => SetProperty(ref width, value);
        }

        public int Height
        {
            get => height;
            set => SetProperty(ref height, value);
        }

        public bool ShouldRerender { get; set; } = true;

        public WidgetBase(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public abstract void OnUpdate();
        public abstract void OnRender();

        internal void SetProperty<T>(ref T prop, T value)
        {
            prop = value;
            ShouldRerender = true;
        }

    }
}