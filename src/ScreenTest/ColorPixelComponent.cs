using Cerm.Render;
using Cerm.Render.Component;
using Cerm.Render.Component.Layout;

namespace Cerm.ScreenTest
{
    public class ColorPixelComponent : ComponentBase
    {
        private char charactor;
        public ColorPixelComponent(LayoutValue x, LayoutValue y, Color fg, Color bg) : base(x, y, PositionAnchor.LeftTop, LayoutValue.Fixed(1), LayoutValue.Fixed(1), fg, bg)
        {
            charactor = ' ';
        }

        public override void Render()
        {
            if (buffer.Length > 0)
            {
                buffer[0] = charactor;
            }
        }
    }
}