using System.Windows.Media.Effects;
using UpdatesClient.Core.Effects;

namespace BlendModeEffectLibrary
{
    public class LuminosityEffect : BlendModeEffect
    {
        static LuminosityEffect()
        {
            _pixelShader.UriSource = Global.MakePackUri("Assets/ShaderSource/LuminosityEffect.ps");
        }

        public LuminosityEffect()
        {
            this.PixelShader = _pixelShader;
        }

        private static readonly PixelShader _pixelShader = new PixelShader();
    }
}
