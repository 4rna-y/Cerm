using Cerm.Input;
using Cerm.Render;
using Cerm.Render.Component;
using Cerm.Render.Component.Layout;
using Cerm.Render.Events;
using Cerm.Render.Screen;

namespace Cerm.Welcome
{
    public class WelcomeScreen : ScreenBase
    {
        public override void OnInitialized()
        {
            StackPanel stackPanel = new StackPanel(
                Orientation.Vertical,
                LayoutValue.Proportional(0.5), LayoutValue.Proportional(0.5), PositionAnchor.CenterMiddle,
                LayoutValue.Proportional(0.5), LayoutValue.Fixed(6));

            stackPanel.Add(new Button("1", BorderCorners.Round(), LayoutValue.Fixed(5), LayoutValue.Fixed(5), PositionAnchor.LeftTop, LayoutValue.Fixed(5), LayoutValue.Fixed(3), Color.White, Color.Black));
            stackPanel.Add(new Button("2", BorderCorners.Round(), LayoutValue.Fixed(5), LayoutValue.Fixed(5), PositionAnchor.LeftTop, LayoutValue.Fixed(5), LayoutValue.Fixed(3), Color.White, Color.Black));

            this.Component.Add(stackPanel);
        }

        public override void OnKeyPressed(KeyPressedEvent e)
        {
            if (e.Key == ConsoleKey.UpArrow)
            {
                this.Component.FocusToPrevious();
            }
            else
            if (e.Key == ConsoleKey.DownArrow)
            {
                this.Component.FocusToNext();
            }
        }

        public override void OnSizeChanged(WindowResizedEvent e)
        {

        }
    }
}