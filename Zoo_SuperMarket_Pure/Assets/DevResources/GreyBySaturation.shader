/// <summary>
/// data   :2020-05-25
/// author :fanzhengyong
/// UI图片黑白效果
/// 通过调整色彩饱和度而实现图片变黑白
/// </summary>
Shader "UFrame/GreyBySaturation" 
{
	Properties 
	{
		[PerRendererData]_MainTex ("Base (RGB)", 2D) = "white" {}
		[PerRendererData]_StencilComp ("Stencil Comparison", Float) = 8
        [PerRendererData]_Stencil ("Stencil ID", Float) = 0
        [PerRendererData]_StencilOp ("Stencil Operation", Float) = 0
        [PerRendererData]_StencilWriteMask ("Stencil Write Mask", Float) = 255
        [PerRendererData]_StencilReadMask ("Stencil Read Mask", Float) = 255
        [PerRendererData]_ColorMask ("Color Mask", Float) = 15
        _Range("Range", Range (0, 1)) = 1
	}
	SubShader 
	{
		Tags      
        {       
            "Queue"="Transparent"       
            "IgnoreProjector"="True"       
            "RenderType"="Transparent"       
            "PreviewType"="Plane"      
            "CanUseSpriteAtlas"="True"      
        }

		Stencil
		{
			Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp] 
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }
        ColorMask [_ColorMask]

		Pass
		{
			Cull Off ZWrite Off ZTest Always
			blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest 
			#include "UnityCG.cginc"

            fixed _Range;
			uniform sampler2D _MainTex;
			float4 ToGray(float4 c)
			{

                
                fixed minv = min(c.r, c.g);
                minv = min(minv, c.b);

                fixed maxv = max(c.r, c.g);
				maxv = max(maxv, c.b);

                fixed halfv = (minv + maxv) * 0.5f;

                return fixed4(halfv * _Range, halfv * _Range, halfv * _Range, c.a);

				//return float4(maxv-c.r + c.r, 
				//	maxv-c.g + c.g, maxv-c.b + c.b, c.a);
     //           return float4(maxv * _Range, 
					//maxv * _Range, maxv * _Range, c.a);
     //           return float4( sum  * _Range, 
					//sum * _Range, sum * _Range, c.a);

     //           return float4( halfv  * _Range, 
					//halfv * _Range, halfv * _Range, c.a);
                 //return float4(_CustomColor.r, _CustomColor.r, _CustomColor.r, c.a);
                 //return float4(lerp(c.rgb, fixed3(halfv, halfv, halfv), _Range), c.a);
                  //fixed calcv = c.r * 0.3 + c.g * 0.59 + c.b * 0.11;
                  //fixed calcv =  (c.r * 0.299 + c.g * 0.518 + c.b * 0.184);
                  //fixed calcv =  (c.r * 0.22 + c.g * 0.707 + c.b * 0.071);
                  //return float4(lerp(c.rgb, fixed3(calcv, calcv, calcv), _Range), c.a);
			}
			fixed4 frag (v2f_img i) : SV_Target
			{
				fixed4 original = tex2D(_MainTex, i.uv);
				return ToGray(original);
			}
			ENDCG
		}
	}
	//Fallback "Diffuse"
}