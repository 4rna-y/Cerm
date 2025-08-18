using Cerm.Lifetime.Event;

namespace Cerm.Render
{
    public class RenderInfoNotificationEvent : EventDataBase
    {
        public double Fps { get; }
        public long RenderTime { get; }

        public RenderInfoNotificationEvent(double fps, long renderTime)
        {
            Fps = fps;
            RenderTime = renderTime;
        }
    }
}