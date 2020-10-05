using System.Windows.Media.Effects;
using UpdatesClient.Core.Effects;

namespace BlendModeEffectLibrary
{
    public class NegationEffect : BlendModeEffect
    {
        static NegationEffect()
        {
            _pixelShader.UriSource = Global.MakePackUri("Assets/ShaderSource/NegationEffect.ps");
        }

        public NegationEffect()
        {
            this.PixelShader = _pixelShader;
        }

        private static readonly PixelShader _pixelShader = new PixelShader();
    }
}
