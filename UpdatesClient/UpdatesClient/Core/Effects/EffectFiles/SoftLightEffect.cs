using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using UpdatesClient.Core.Effects;

namespace BlendModeEffectLibrary
{
	public class SoftLightEffect : BlendModeEffect
	{
		static SoftLightEffect()
		{
			_pixelShader.UriSource = Global.MakePackUri("Assets/ShaderSource/SoftLightEffect.ps");
		}

		public SoftLightEffect()
		{
			this.PixelShader = _pixelShader;
		}

		private static PixelShader _pixelShader = new PixelShader();
	}
}
