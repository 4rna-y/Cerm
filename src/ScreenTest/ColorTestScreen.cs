using Cerm.Render;
using Cerm.Input;
using Cerm.Lifetime.Event;
using Cerm.Render.Events;
using Cerm.Render.Component.Layout;
using Cerm.Render.Screen;

namespace Cerm.ScreenTest
{
    public class ColorTestScreen : ScreenBase
    {
        private int hue = 0;
        private ColorStatusTextComponent titleText;
        private ColorStatusTextComponent hueText;
        private ColorStatusTextComponent fpsText;
        private ColorStatusTextComponent rtText;

        public ColorTestScreen() : base(false)
        {
        }

        public override void OnInitialized()
        {
            int w = Component.ActualWidth;
            int h = Component.ActualHeight - 1;
            titleText = new ColorStatusTextComponent("Color Tester", LayoutValue.Fixed(0), LayoutValue.Fixed(0), w, Color.Black, Color.White);
            hueText = new ColorStatusTextComponent(hue.ToString(), LayoutValue.Fixed(0), LayoutValue.Fixed(h), 10, Color.Black, Color.White);
            fpsText = new ColorStatusTextComponent(" ", LayoutValue.Fixed(10), LayoutValue.Fixed(h), 10, Color.Black, Color.White);
            rtText = new ColorStatusTextComponent(" ", LayoutValue.Fixed(20), LayoutValue.Fixed(h), 10, Color.Black, Color.White);

            EventBus.Instance.Subscribe<RenderInfoNotificationEvent>(OnRenderInfoNotified);

            CreateColorPixels();
        }

        public override void OnKeyPressed(KeyPressedEvent e)
        {
            if (e.Key == ConsoleKey.LeftArrow && hue > 0)
            {
                hue--;
                ChangeColors();
            }

            if (e.Key == ConsoleKey.RightArrow && hue < 360)
            {
                hue++;
                ChangeColors();
            }
        }

        public override void OnSizeChanged(WindowResizedEvent e)
        {
            CreateColorPixels();
        }

        private void CreateColorPixels()
        {
            this.Component.Children.Clear();

            int w = Component.ActualWidth;
            int h = Component.ActualHeight - 2;

            titleText.Layout.Width = LayoutValue.Fixed(w);

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    this.Component.Add(
                        new ColorPixelComponent(
                            LayoutValue.Fixed(x), LayoutValue.Fixed(y + 1),
                            Color.Black,
                            Color.FromHsv(
                                hue,
                                (double)y / (double)h,
                                (double)x / (double)w)));
                }
            }

            this.Component.Add(titleText);
            this.Component.Add(hueText);
            this.Component.Add(fpsText);
            this.Component.Add(rtText);
        }

        private void ChangeColors()
        {
            int w = Component.ActualWidth;
            int h = Component.ActualHeight - 2;
            for (int i = 1; i < Component.Children.Count - 4; i++)
            {
                int y = i / w;
                int x = i % w;

                ColorPixelComponent cc = (ColorPixelComponent)Component.Children[i];
                cc.Background =
                    Color.FromHsv(
                        hue,
                        (double)y / (double)h,
                        (double)x / (double)w);
            }

            hueText.Text = hue.ToString();

            //全画面再描画リクエスト
            //ちらつきが気になる
            //EventBus.Instance.Publish(new StructureChangedEvent(this.Component));
        }

        private void OnRenderInfoNotified(RenderInfoNotificationEvent e)
        {
            try
            {
                fpsText.Text = $"{e.Fps:F1}fps";
                rtText.Text = $"{e.RenderTime}ms";
            }
            catch
            {
                fpsText.Text = "ERR";
                rtText.Text = "ERR";
                hueText.Text = "ERR";
            }
        }
    }
}