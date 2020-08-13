//-----------------------------------------------------------------------------------------

float OutlineIntensity : register(C0);

//--------------------------------------------------------------------------------------
// Sampler Inputs (Brushes, including ImplicitInput)
//--------------------------------------------------------------------------------------

sampler2D implicitInputSampler : register(S0);


//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------

float4 main(float2 uv : TEXCOORD) : COLOR
{
    float4 prepassColor = float4(0,0,0,0);
    float4 bluredColor = float4(0,0,0,0);
    float4 difColor = max( 0, bluredColor - prepassColor);
    float4 color = difColor* tex2D(implicitInputSampler, uv) * 5;
    color.a = 1;    
    return color;
    
    
    /*float4 c = 0;
    float rad = 0 * 0.0174533f;
    float xOffset = cos(rad);
    float yOffset = sin(rad);

    for(int i=0; i<16; i++)
    {
        uv.x = uv.x - 5 * xOffset;
        uv.y = uv.y - 5 * yOffset;
        c += tex2D(implicitInputSampler, uv);
    }
    c /= 16;
    
    return c;*/
}
