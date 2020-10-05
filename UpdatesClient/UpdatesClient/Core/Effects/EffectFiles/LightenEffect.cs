using System.Windows.Media.Effects;
using UpdatesClient.Core.Effects;

namespace BlendModeEffectLibrary
{
    public class LightenEffect : BlendModeEffect
    {
        static LightenEffect()
        {
            _pixelShader.UriSource = Global.MakePackUri("Assets/ShaderSource/LightenEffect.ps");
        }

        public LightenEffect()
        {
            this.PixelShader = _pixelShader;
        }

        private static readonly PixelShader _pixelShader = new PixelShader();
    }
}
