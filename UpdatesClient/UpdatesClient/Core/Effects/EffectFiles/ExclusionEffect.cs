using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using UpdatesClient.Core.Effects;

namespace BlendModeEffectLibrary
{
	public class ExclusionEffect : BlendModeEffect
	{
		static ExclusionEffect()
		{
			_pixelShader.UriSource = Global.MakePackUri("Assets/ShaderSource/ExclusionEffect.ps");
		}

		public ExclusionEffect()
		{
			this.PixelShader = _pixelShader;
		}

		private static PixelShader _pixelShader = new PixelShader();
	}
}
