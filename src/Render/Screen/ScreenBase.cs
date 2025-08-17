using Cerm.Input;
using Cerm.Render.Component;
using Cerm.Render.Interfaces;

namespace Cerm.Render.Screen
{
    public abstract class ScreenBase
    {
        public ScreenLayer Component { get; }
        public ScreenLayer Modal { get; }
        public ScreenLayer Notification { get; }

        public ScreenBase()
        {
            Component = new ScreenLayer();
            Modal = new ScreenLayer();
            Notification = new ScreenLayer();
        }

        public abstract void OnInitialized();
        public abstract void OnKeyPressed(KeyPressedEvent e);
    }
}