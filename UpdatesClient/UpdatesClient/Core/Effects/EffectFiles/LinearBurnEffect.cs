using System.Windows.Media.Effects;
using UpdatesClient.Core.Effects;

namespace BlendModeEffectLibrary
{
    public class LinearBurnEffect : BlendModeEffect
    {
        static LinearBurnEffect()
        {
            _pixelShader.UriSource = Global.MakePackUri("Assets/ShaderSource/LinearBurnEffect.ps");
        }

        public LinearBurnEffect()
        {
            this.PixelShader = _pixelShader;
        }

        private static readonly PixelShader _pixelShader = new PixelShader();
    }
}
