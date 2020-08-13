using System;

namespace UpdatesClient.Core.Helpers
{
    public class OutlineEffect : ShaderEffect
    {
        private PixelShader pixelShader = new PixelShader();

        public OutlineEffect()
        {
            pixelShader.UriSource = new Uri("/UpdatesClient;component/Outline.ps", UriKind.RelativeOrAbsolute);
            PixelShader = pixelShader;
            UpdateShaderValue(InputProperty);
        }

        public static readonly DependencyProperty InputProperty = RegisterPixelShaderSamplerProperty("Input",
            typeof(OutlineEffect), 0);

        public Brush Input
        {
            get
            {
                return ((Brush)GetValue(InputProperty));
            }
            set
            {
                this.SetValue(InputProperty, value);
            }
        }
    }
}
