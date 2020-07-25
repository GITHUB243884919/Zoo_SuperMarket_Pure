/// <summary>
/// data   :2016-07-21
/// author :fanzhengyong
/// UGUI图片单一颜色效果
/// </summary>
Shader "UFrame/OneColor" 
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
        _CustomColor ("_CustomColor", Color) = (1,1,1,1) 
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
            fixed4 _CustomColor;
			uniform sampler2D _MainTex;
			float4 ToGray(float4 c)
			{
                return float4(_CustomColor.r, _CustomColor.g, _CustomColor.b, c.a);
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