using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using UpdatesClient.Core.Effects;

namespace BlendModeEffectLibrary
{
	public class ColorEffect : BlendModeEffect
	{
		static ColorEffect()
		{
			_pixelShader.UriSource = Global.MakePackUri("Assets/ShaderSource/ColorEffect.ps");
		}

		public ColorEffect()
		{
			this.PixelShader = _pixelShader;
		}

		private static PixelShader _pixelShader = new PixelShader();
	}
}
