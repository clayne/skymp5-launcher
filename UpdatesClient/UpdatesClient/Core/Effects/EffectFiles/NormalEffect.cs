﻿using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using UpdatesClient.Core.Effects;

namespace BlendModeEffectLibrary
{
	public class NormalEffect : BlendModeEffect
	{
		static NormalEffect()
		{
			_pixelShader.UriSource = Global.MakePackUri("Assets/ShaderSource/NormalEffect.ps");
		}

		public NormalEffect()
		{
			this.PixelShader = _pixelShader;
		}

		private static PixelShader _pixelShader = new PixelShader();
	}
}
