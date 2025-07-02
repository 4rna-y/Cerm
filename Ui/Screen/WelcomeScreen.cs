using System;
using Cerm.Ui.Manager;
using Cerm.Ui.Screen.Abstruct;

namespace Cerm.Ui.Screen
{
    public class WelcomeScreen : ScreenBase
    {
        private readonly IScreenManager screen;

        public WelcomeScreen(IScreenManager screen)
        {
            this.screen = screen;
        }

        public override void OnEnter()
        {
            Console.Clear();

            string title = "Cerm";
            int titleWidth = title.Length;

            Console.SetCursorPosition((this.Width / 2) - (titleWidth / 2), (this.Height / 2) - (1 / 2));
            Console.WriteLine(title);
        }

        public override void OnResized()
        {
            
        }

        public override void Update()
        {

        }

        public override void Render()
        {
            
        }

        public override void HandleInput(ConsoleKeyInfo key)
        {
            if (key.Modifiers == ConsoleModifiers.Control && key.Key == ConsoleKey.C)
            {
                Console.Clear();
                screen.IsRunning = false;
            }
        }
    }
}