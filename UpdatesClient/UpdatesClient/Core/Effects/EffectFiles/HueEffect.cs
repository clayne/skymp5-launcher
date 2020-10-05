using System.Windows.Media.Effects;
using UpdatesClient.Core.Effects;

namespace BlendModeEffectLibrary
{
    public class HueEffect : BlendModeEffect
    {
        static HueEffect()
        {
            _pixelShader.UriSource = Global.MakePackUri("Assets/ShaderSource/HueEffect.ps");
        }

        public HueEffect()
        {
            this.PixelShader = _pixelShader;
        }

        private static readonly PixelShader _pixelShader = new PixelShader();
    }
}
