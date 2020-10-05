using System.Windows.Media.Effects;
using UpdatesClient.Core.Effects;

namespace BlendModeEffectLibrary
{
    public class ReflectEffect : BlendModeEffect
    {
        static ReflectEffect()
        {
            _pixelShader.UriSource = Global.MakePackUri("Assets/ShaderSource/ReflectEffect.ps");
        }

        public ReflectEffect()
        {
            this.PixelShader = _pixelShader;
        }

        private static readonly PixelShader _pixelShader = new PixelShader();
    }
}
