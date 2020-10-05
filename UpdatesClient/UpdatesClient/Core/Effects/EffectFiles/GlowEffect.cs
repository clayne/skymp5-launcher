using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using UpdatesClient.Core.Effects;

namespace BlendModeEffectLibrary
{
	public class GlowEffect : BlendModeEffect
	{
		static GlowEffect()
		{
			_pixelShader.UriSource = Global.MakePackUri("Assets/ShaderSource/GlowEffect.ps");
		}

		public GlowEffect()
		{
			this.PixelShader = _pixelShader;
		}

		private static PixelShader _pixelShader = new PixelShader();
	}
}
