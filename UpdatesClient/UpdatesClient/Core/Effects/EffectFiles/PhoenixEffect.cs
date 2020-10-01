using System;
using System.Windows;
using System.Windows.Media;
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

		private static PixelShader _pixelShader = new PixelShader();
	}
}
