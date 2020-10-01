﻿using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using UpdatesClient.Core.Effects;

namespace BlendModeEffectLibrary
{
	public class ColorDodgeEffect : BlendModeEffect
	{
		static ColorDodgeEffect()
		{
			_pixelShader.UriSource = Global.MakePackUri("Assets/ShaderSource/ColorDodgeEffect.ps");
		}

		public ColorDodgeEffect()
		{
			this.PixelShader = _pixelShader;
		}

		private static PixelShader _pixelShader = new PixelShader();
	}
}
