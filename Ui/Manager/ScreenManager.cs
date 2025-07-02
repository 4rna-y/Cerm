using System;
using System.Collections.Generic;
using Cerm.Ui.Screen.Abstruct;

namespace Cerm.Ui.Manager
{
    public class ScreenManager : IScreenManager
    {
        private Stack<ScreenBase> stack;
        public bool IsRunning { get; set; } = true;

        public ScreenManager()
        {
            stack = new Stack<ScreenBase>();
        }

        public void Push(ScreenBase screen)
        {
            if (stack.Count > 0)
                stack.Peek().OnExit();
            stack.Push(screen);
            screen.OnEnter();
        }

        public void Pop()
        {
            if (stack.Count > 1)
            {
                stack.Pop().OnExit();
                stack.Peek().OnEnter();
            }
            else if (stack.Count == 1)
            {
                stack.Pop().OnExit();
                IsRunning = false;
            }
        }

        public void Run()
        {
            while (IsRunning && stack.Count > 0)
            {
                ScreenBase current = stack.Peek();
                current.Update();
                current.Render();

                ConsoleKeyInfo key = Console.ReadKey(true);
                current.HandleInput(key);
            }
        }

        public void OnResized(int width, int height)
        {
            ScreenBase screen = stack.Peek();
            screen.Width = width;
            screen.Height = height;
            screen.OnResized();
        }
    }
}