using Cerm.Lifetime.Event;
using Microsoft.Extensions.Hosting;

namespace Cerm.Input
{
    public class InputHandler : BackgroundService
    {
        public InputHandler()
        {
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                    EventBus.Instance.Publish(new KeyPressedEvent()
                    {
                        Charactor = keyInfo.KeyChar,
                        Key = keyInfo.Key,
                        Modifiers = keyInfo.Modifiers
                    });
                }

                await Task.Delay(1, stoppingToken);
            }
        }
    }
}