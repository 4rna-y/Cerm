using System.ComponentModel;
using Cerm.Input;
using Cerm.Lifetime.Event;
using Cerm.Render;
using Cerm.Render.Component;
using Cerm.Render.Screen;

namespace Cerm.ScreenTest
{
    public class ColorTestScreen : ScreenBase
    {
        private int hue = 0;
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
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    this.Component.Add(
                        new ColorPixelComponent(
                            LayoutValue.Fixed(x), LayoutValue.Fixed(y),
                            new Color(0, 0, 0),
                            Color.FromHsv(
                                hue,
                                (double)y / (double)h,
                                (double)x / (double)w)));
                }
            }

            hueText = new ColorStatusTextComponent(hue.ToString(), LayoutValue.Fixed(0), LayoutValue.Fixed(h), 10, new Color(0, 0, 0), new Color(255, 255, 255));
            fpsText = new ColorStatusTextComponent(" ", LayoutValue.Fixed(10), LayoutValue.Fixed(h), 10, new Color(0, 0, 0), new Color(255, 255, 255));
            rtText = new ColorStatusTextComponent(" ", LayoutValue.Fixed(20), LayoutValue.Fixed(h), 10, new Color(0, 0, 0), new Color(255, 255, 255));

            EventBus.Instance.Subscribe<RenderInfoNotificationEvent>(OnRenderInfoNotified);

            this.Component.Add(hueText);
            this.Component.Add(fpsText);
            this.Component.Add(rtText);
        }

        public override void OnKeyPressed(KeyPressedEvent e)
        {
            if (e.Key == ConsoleKey.LeftArrow)
            {
                hue--;
                ChangeColors();
            }

            if (e.Key == ConsoleKey.RightArrow)
            {
                hue++;
                ChangeColors();
            }
        }

        private void ChangeColors()
        {
            int w = Component.ActualWidth;
            int h = Component.ActualHeight - 1;
            for (int i = 0; i < Component.Children.Count - 3; i++)
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

            //全画面再描画リクエスト.
            //ちらつきが気になる
            //EventBus.Instance.Publish(new StructureChangedEvent(this.Component));
        }

        private void OnRenderInfoNotified(RenderInfoNotificationEvent e)
        {
            try
            {
                fpsText.Text = $"{e.Fps:F1}fps";
                rtText.Text = $"{e.RenderTime}ms";
                
                hueText.Text = $"{hue}";
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