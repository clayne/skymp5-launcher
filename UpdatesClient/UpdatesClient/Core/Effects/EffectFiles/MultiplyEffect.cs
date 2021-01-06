﻿using System.Windows.Media.Effects;
using UpdatesClient.Core.Effects;

namespace BlendModeEffectLibrary
{
    public class MultiplyEffect : BlendModeEffect
    {
        static MultiplyEffect()
        {
            _pixelShader.UriSource = Global.MakePackUri("Assets/ShaderSource/MultiplyEffect.ps");
        }

        public MultiplyEffect()
        {
            this.PixelShader = _pixelShader;
        }

        private static readonly PixelShader _pixelShader = new PixelShader();
    }
}
