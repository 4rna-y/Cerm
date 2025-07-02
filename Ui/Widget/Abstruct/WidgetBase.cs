namespace Cerm.Ui.Screen.Widget.Abstruct
{
    public abstract class WidgetBase
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public bool ShouldRerender { get; set; }
        

        public WidgetBase(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public abstract void OnRender();

    }
}