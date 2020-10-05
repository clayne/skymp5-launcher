using System.Windows.Media.Effects;
using UpdatesClient.Core.Effects;

namespace BlendModeEffectLibrary
{
    public class ScreenEffect : BlendModeEffect
    {
        static ScreenEffect()
        {
            _pixelShader.UriSource = Global.MakePackUri("Assets/ShaderSource/ScreenEffect.ps");
        }

        public ScreenEffect()
        {
            this.PixelShader = _pixelShader;
        }

        private static readonly PixelShader _pixelShader = new PixelShader();
    }
}
