using System.Diagnostics;
using System.Text;
using Cerm.Input;
using Cerm.Lifetime.Event;
using Cerm.Render.Component;
using Cerm.Render.Interfaces;
using Cerm.Render.Screen;
using Cerm.ScreenTest;
using Microsoft.Extensions.Hosting;

namespace Cerm.Render
{
    public class TerminalRenderer : BackgroundService
    {
        private readonly StringBuffer renderBuffer;
        private BufferedStream bufferedOutput;
        private StreamWriter bufferWriter;
        private bool requiredRecollect;
        private List<ComponentBase> components;
        private int cursorX = -1;
        private int cursorY = -1;
        private int screenWidth;
        private int screenHeight;

        public ScreenBase CurrentScreen { get; set; }

        public TerminalRenderer()
        {
            Console.OutputEncoding = Encoding.UTF8;

            renderBuffer = new StringBuffer(4096);
            bufferedOutput = new BufferedStream(Console.OpenStandardOutput(), 8192);
            bufferWriter = new StreamWriter(bufferedOutput) { AutoFlush = false };
            Console.SetOut(bufferWriter);
            components = new List<ComponentBase>();
            screenWidth = Console.WindowWidth;
            screenHeight = Console.WindowHeight;
            CurrentScreen = new ColorTestScreen();
            
            EventBus.Instance.Subscribe<KeyPressedEvent>(OnKeyPressed);
            EventBus.Instance.Subscribe<StructureChangedEvent>(OnStructureChanged);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            this.Clear();
            SetCursor(CurrentScreen.IsCursor);
            CurrentScreen.OnInitialized();

            double ideal = 16.0;
            double alpha = 0.1;
            double fps = 60.0;
            long renderTime = 0;
            long lastFrameStart = 0;

            while (!stoppingToken.IsCancellationRequested)
            {
                long frameStart = Environment.TickCount64;
                
                if (lastFrameStart > 0)
                {
                    long actualFrameTime = frameStart - lastFrameStart;
                    ideal = alpha * actualFrameTime + (1 - alpha) * ideal;
                    fps = 1000.0 / ideal;
                }

                long renderStart = Environment.TickCount64;
                RenderComponent();
                renderTime = Environment.TickCount64 - renderStart;

                EventBus.Instance.Publish(new RenderInfoNotificationEvent(fps, renderTime));
                await Task.Delay(16, stoppingToken);
                
                lastFrameStart = frameStart;
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            SetCursor(true);
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();

            return base.StopAsync(cancellationToken);
        }

        private void SetCursor(bool isCursor)
        {
            if (isCursor) Console.Out.Write("\x1b[?25h");
            else Console.Out.Write("\x1b[?25l");

            Console.Out.Flush();
        }

        private void CollectComponentsFromContainer(IContainer container)
        {
            for (int i = 0; i < container.Children.Count; i++)
            {
                if (container.Children[i] is IContainer childContainer)
                {
                    CollectComponentsFromContainer(childContainer);
                }

                components.Add(container.Children[i]);
            }
        }

        private void RenderComponent()
        {
            renderBuffer.Clear();

            if (requiredRecollect)
            {
                components.Clear();
                CollectComponentsFromContainer(CurrentScreen.Component);
                RenderFullScreen();
                
                if (renderBuffer.Length > 0)
                {
                    Console.Write(renderBuffer.ToString());
                    Console.Out.Flush();
                }
            }
            else
            {
                RenderDirtyComponents();
            }
            
            
            
            requiredRecollect = false;
        }

        private void RenderFullScreen()
        {
            renderBuffer.Write("\x1b[2J\x1b[H");
            cursorX = 0;
            cursorY = 0;
            
            for (int i = 0; i < components.Count; i++)
            {
                components[i].Render();
                RenderSingleComponentTo(renderBuffer, components[i]);
                components[i].RequiredRedraw = false;
            }
        }

        private void RenderDirtyComponents()
        {
            for (int i = 0; i < components.Count; i++)
            {
                if (components[i].RequiredRedraw)
                {
                    components[i].Render();
                    RenderSingleComponentTo(Console.Out, components[i]);
                    components[i].RequiredRedraw = false;
                }
            }
            Console.Out.Flush();
        }

        private void RenderSingleComponentTo(TextWriter writer, ComponentBase component)
        {
            char[] buffer = component.GetBuffer();
            int x = component.ActualX;
            int y = component.ActualY;
            int w = component.ActualWidth;
            int h = component.ActualHeight;

            if (x >= screenWidth || y >= screenHeight || x + w <= 0 || y + h <= 0) return;

            int clipLeft = Math.Max(0, -x);
            int clipTop = Math.Max(0, -y);
            int clipRight = Math.Max(0, (x + w) - screenWidth);
            int clipBottom = Math.Max(0, (y + h) - screenHeight);
            int renderWidth = w - clipLeft - clipRight;
            int renderHeight = h - clipTop - clipBottom;

            if (renderWidth <= 0 || renderHeight <= 0) return;

            Color fg = component.Foreground;
            Color bg = component.Background;

            writer.Write(fg.GetForeground());
            writer.Write(bg.GetBackground());

            for (int dy = 0; dy < renderHeight; dy++)
            {
                int ty = y + clipTop + dy;
                int tx = x + clipLeft;
                MoveCursor(writer, tx, ty);

                int bi = (clipTop + dy) * w + clipLeft;
                
                if (bi < 0 || bi >= buffer.Length) continue;
                
                int actualRenderWidth = Math.Min(renderWidth, buffer.Length - bi);
                if (actualRenderWidth <= 0) continue;
                
                Span<char> s = buffer.AsSpan(bi, actualRenderWidth);
                writer.Write(s);
                
                cursorX = tx + actualRenderWidth;
                cursorY = ty;
            }
            
            writer.Write("\x1b[0m");
        }

        private void MoveCursor(TextWriter writer, int tx, int ty)
        {
            if (tx == cursorX && ty == cursorY) return;

            if (ty == cursorY)
            {
                int deltaX = tx - cursorX;
                if (Math.Abs(deltaX) <= 3)
                {
                    if (deltaX > 0) writer.Write($"\x1b[{deltaX}C");
                    else
                    if (deltaX < 0) writer.Write($"\x1b[{-deltaX}D");
                }
                else
                {
                    writer.Write($"\x1b[{tx + 1}G");
                }
            }
            else
            {
                writer.Write($"\x1b[{ty + 1};{tx + 1}H");
            }

            cursorX = tx;
            cursorY = ty;
        }

        private void Clear()
        {
            Console.Clear();
        }

        private void OnKeyPressed(KeyPressedEvent e)
        {
            CurrentScreen.OnKeyPressed(e);
        }

        private void OnStructureChanged(StructureChangedEvent e)
        {
            requiredRecollect = true;
        }
    }
}