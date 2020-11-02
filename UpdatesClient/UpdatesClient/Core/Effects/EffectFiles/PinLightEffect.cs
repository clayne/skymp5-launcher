using System.Windows.Media.Effects;
using UpdatesClient.Core.Effects;

namespace BlendModeEffectLibrary
{
    public class PinLightEffect : BlendModeEffect
    {
        static PinLightEffect()
        {
            _pixelShader.UriSource = Global.MakePackUri("Assets/ShaderSource/PinLightEffect.ps");
        }

        public PinLightEffect()
        {
            this.PixelShader = _pixelShader;
        }

        private static readonly PixelShader _pixelShader = new PixelShader();
    }
}
