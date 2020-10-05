using System.Windows.Media.Effects;
using UpdatesClient.Core.Effects;

namespace BlendModeEffectLibrary
{
    public class HardLightEffect : BlendModeEffect
    {
        static HardLightEffect()
        {
            _pixelShader.UriSource = Global.MakePackUri("Assets/ShaderSource/HardLightEffect.ps");
        }

        public HardLightEffect()
        {
            this.PixelShader = _pixelShader;
        }

        private static readonly PixelShader _pixelShader = new PixelShader();
    }
}
