using System;
using Cerm.Lifetime.Event;

namespace Cerm.Render.Events
{
    public class WindowResizedEvent : EventDataBase
    {
        public int NewWidth { get; }
        public int NewHeight { get; }

        public WindowResizedEvent(int newWidth, int newHeight)
        {
            NewWidth = newWidth;
            NewHeight = newHeight;
        }
    }
}
