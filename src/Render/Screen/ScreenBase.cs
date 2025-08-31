using Cerm.Input;
using Cerm.Render.Component;
using Cerm.Render.Events;
using Cerm.Render.Interfaces;

namespace Cerm.Render.Screen
{
    public abstract class ScreenBase
    {
        public bool IsCursor { get; }
        public ScreenLayer Component { get; }
        public ScreenLayer Modal { get; }
        public ScreenLayer Notification { get; }

        public ScreenBase(bool isCursor = true)
        {
            IsCursor = isCursor;
            Component = new ScreenLayer();
            Modal = new ScreenLayer();
            Notification = new ScreenLayer();
        }

        public abstract void OnInitialized();
        public abstract void OnKeyPressed(KeyPressedEvent e);
        public abstract void OnSizeChanged(WindowResizedEvent e);
    }
}