using System.ComponentModel;
using Cerm.Input;
using Cerm.Render.Component;
using Cerm.Render.Screen;

namespace Cerm.Welcome
{
    public class TestScreen : ScreenBase
    {

        public override void OnInitialized()
        {
            int w = Component.ActualWidth;
            int h = Component.ActualHeight;
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    this.Component.Add(
                        new TestComponent(
                            ' ',
                            LayoutValue.Fixed(x), LayoutValue.Fixed(y),
                            new Color(0, 0, 0),
                            Color.FromHsv(
                                (int)((double)y / (double)h * 360),
                                (double)x / (double)w,
                                (double)x / (double)w)));
                }
            }
            
        }

        public override void OnKeyPressed(KeyPressedEvent e)
        {
            

           
        }
    }
}