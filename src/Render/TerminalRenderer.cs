using Cerm.Lifetime.Event;
using Microsoft.Extensions.Hosting;

namespace Cerm.Render
{
    public class TerminalRenderer : BackgroundService
    {
        private readonly IEventBus eventBus;

        public TerminalRenderer(IEventBus eventBus)
        {
            this.eventBus = eventBus;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            this.Clear();
            
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(16, stoppingToken);
            }
        }

        private void Clear()
        {
            Console.Clear();
        }
    }
}