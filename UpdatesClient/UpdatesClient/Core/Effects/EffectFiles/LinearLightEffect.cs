using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using UpdatesClient.Core.Effects;

namespace BlendModeEffectLibrary
{
	public class LinearLightEffect : BlendModeEffect
	{
		static LinearLightEffect()
		{
			_pixelShader.UriSource = Global.MakePackUri("Assets/ShaderSource/LinearLightEffect.ps");
		}

		public LinearLightEffect()
		{
			this.PixelShader = _pixelShader;
		}

		private static PixelShader _pixelShader = new PixelShader();
	}
}
