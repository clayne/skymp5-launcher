using System.Windows.Media.Effects;
using UpdatesClient.Core.Effects;

namespace BlendModeEffectLibrary
{
    public class LinearDodgeEffect : BlendModeEffect
    {
        static LinearDodgeEffect()
        {
            _pixelShader.UriSource = Global.MakePackUri("Assets/ShaderSource/LinearDodgeEffect.ps");
        }

        public LinearDodgeEffect()
        {
            this.PixelShader = _pixelShader;
        }

        private static readonly PixelShader _pixelShader = new PixelShader();
    }
}
