using System.Windows.Media.Effects;
using UpdatesClient.Core.Effects;

namespace BlendModeEffectLibrary
{
    public class ColorBurnEffect : BlendModeEffect
    {
        static ColorBurnEffect()
        {
            _pixelShader.UriSource = Global.MakePackUri("Assets/ShaderSource/ColorBurnEffect.ps");
        }

        public ColorBurnEffect()
        {
            this.PixelShader = _pixelShader;
        }

        private static readonly PixelShader _pixelShader = new PixelShader();
    }
}
