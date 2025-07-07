using Cerm.Ui.Screen.Abstruct;

namespace Cerm.Ui.Manager
{
    public interface IScreenManager
    {
        public bool IsRunning { get; set; }
        void Push(ScreenBase screen);
        void Pop();
        void Run();
        void OnResized(int width, int height);
    }
}