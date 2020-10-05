using System.Windows.Media.Effects;
using UpdatesClient.Core.Effects;

namespace BlendModeEffectLibrary
{
    public class DarkenEffect : BlendModeEffect
    {
        static DarkenEffect()
        {
            _pixelShader.UriSource = Global.MakePackUri("Assets/ShaderSource/DarkenEffect.ps");
        }

        public DarkenEffect()
        {
            this.PixelShader = _pixelShader;
        }

        private static readonly PixelShader _pixelShader = new PixelShader();
    }
}
