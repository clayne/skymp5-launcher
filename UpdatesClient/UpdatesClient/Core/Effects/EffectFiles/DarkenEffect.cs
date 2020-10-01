using System;
using System.Windows;
using System.Windows.Media;
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

		private static PixelShader _pixelShader = new PixelShader();
	}
}
