using Cerm.Lifetime.Event;

namespace Cerm.Input
{
    public class KeyPressedEvent : EventDataBase
    {
        public char Charactor { get; set; }
        public ConsoleKey Key { get; set; }
        public ConsoleModifiers Modifiers { get; set; }
    }
}