using System.Windows.Media.Effects;
using UpdatesClient.Core.Effects;

namespace BlendModeEffectLibrary
{
    public class OverlayEffect : BlendModeEffect
    {
        static OverlayEffect()
        {
            _pixelShader.UriSource = Global.MakePackUri("Assets/ShaderSource/OverlayEffect.ps");
        }

        public OverlayEffect()
        {
            this.PixelShader = _pixelShader;
        }

        private static readonly PixelShader _pixelShader = new PixelShader();
    }
}
