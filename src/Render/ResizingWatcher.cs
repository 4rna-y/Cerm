using Cerm.Lifetime.Event;
using Cerm.Render.Events;
using Microsoft.Extensions.Hosting;

namespace Cerm.Render
{
    public class ResizingWatcher : BackgroundService
    {
        private int lastWidth;
        private int lastHeight;

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            lastWidth = Console.WindowWidth;
            lastHeight = Console.WindowHeight;

            return base.StartAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (lastWidth != Console.WindowWidth || lastHeight != Console.WindowHeight)
                {
                    lastWidth = Console.WindowWidth;
                    lastHeight = Console.WindowHeight;
                    EventBus.Instance.Publish(new WindowResizedEvent(lastWidth, lastHeight));
                }

                await Task.Delay(250, stoppingToken);
            }
        }
    }
}