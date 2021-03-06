// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Custom/LightSpriteForLightEmitterObjects"
{
  Properties
  {
      [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
      _Color("Tint", Color) = (1,1,1,1)
      [MaterialToggle] PixelSnap("Pixel snap", Float) = 0
      [HideInInspector] _RendererColor("RendererColor", Color) = (1,1,1,1)
      [HideInInspector] _Flip("Flip", Vector) = (1,1,1,1)
      [PerRendererData] _AlphaTex("External Alpha", 2D) = "white" {}
      [PerRendererData] _EnableExternalAlpha("Enable External Alpha", Float) = 0
  }

    SubShader
      {
          Tags
          {
              "Queue" = "Transparent"
              "IgnoreProjector" = "True"
              "RenderType" = "Transparent"
              "PreviewType" = "Plane"
              "CanUseSpriteAtlas" = "True"
          }

          Cull Off
          Lighting Off
          ZWrite Off
          Blend One OneMinusSrcAlpha

          CGPROGRAM
          #pragma surface surf Lambert vertex:vert nofog nolightmap nodynlightmap keepalpha noinstancing
          #pragma multi_compile_local _ PIXELSNAP_ON
          #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
          #include "UnitySprites.cginc"

          struct Input
          {
              float2 uv_MainTex;
              fixed4 color;
          };

          void vert(inout appdata_full v, out Input o)
          {
              v.vertex = UnityFlipSprite(v.vertex, _Flip);

              #if defined(PIXELSNAP_ON)
              v.vertex = UnityPixelSnap(v.vertex);
              #endif

              UNITY_INITIALIZE_OUTPUT(Input, o);
              o.color = v.color * _Color * _RendererColor;
          }

          uniform float4 highLightColor;
          uniform float4 midLightColor;
          uniform float4 lowLightColor;
          uniform float4 holderLightColor;

          float4 _MainTex_TexelSize;

          void surf(Input IN, inout SurfaceOutput o)
          {
              fixed4 c = SampleSpriteTexture(IN.uv_MainTex) * IN.color;

              if (c.a > 0.1)
              {
                if (c.r > 0.3 || c.g > 0.3 || c.b > 0.3)
                {
                  if (c.r > 0.9 && c.b > 0.9 && c.b > 0.9)
                  {
                    c = highLightColor;
                    /*
                    c.g = 1;
                    c.r = 1;
                    c.b = 0;
                    */
                    o.Emission = c.rgb;
                  }
                  else if (c.r > 0.7 && c.g > 0.7 && c.b > 0.7)
                  {
                    c = midLightColor;
                    /*
                    c.g = 0.65;
                    c.r = 1;
                    c.b = 0;
                    */
                  }
                  else
                  {
                    c = lowLightColor;
                    /*
                    c.g = 0.2;
                    c.r = 1;
                    c.b = 0;
                    */
                  }
                }
                else if (c.r > 0 && c.b > 0 && c.g > 0)
                {
                  float tempa = c.a;
                  c = holderLightColor;
                  c.a = tempa;

                  /*
                  c.r = 0.55;
                  c.g = 0.27;
                  c.b = 0.08;
                  */
                }
              }

              o.Albedo = c.rgb * c.a;
              o.Alpha = c.a;
          }
          ENDCG
      }

        //Fallback "Transparent/VertexLit"
}
