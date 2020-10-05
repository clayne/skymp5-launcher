using System.Windows.Media.Effects;
using UpdatesClient.Core.Effects;

namespace BlendModeEffectLibrary
{
    public class PhoenixEffect : BlendModeEffect
    {
        static PhoenixEffect()
        {
            _pixelShader.UriSource = Global.MakePackUri("Assets/ShaderSource/PhoenixEffect.ps");
        }

        public PhoenixEffect()
        {
            this.PixelShader = _pixelShader;
        }

        private static readonly PixelShader _pixelShader = new PixelShader();
    }
}
