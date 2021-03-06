// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Custom/LightSpriteForEnemy"
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

          uniform float outlineEffect = 0;
          uniform float hitEffect = 0;
          float4 _MainTex_TexelSize;

          void surf(Input IN, inout SurfaceOutput o)
          {
              fixed4 c = SampleSpriteTexture(IN.uv_MainTex) * IN.color;

              if (c.a < 0.1)
              {
                  fixed4 cUp = SampleSpriteTexture(float2(IN.uv_MainTex.x, IN.uv_MainTex.y - _MainTex_TexelSize.y)) * IN.color;
                  fixed4 cDown = SampleSpriteTexture(float2(IN.uv_MainTex.x, IN.uv_MainTex.y + _MainTex_TexelSize.y)) * IN.color;
                  fixed4 cLeft = SampleSpriteTexture(float2(IN.uv_MainTex.x - _MainTex_TexelSize.x, IN.uv_MainTex.y)) * IN.color;
                  fixed4 cRight = SampleSpriteTexture(float2(IN.uv_MainTex.x + _MainTex_TexelSize.x, IN.uv_MainTex.y)) * IN.color;

                  int neighbours = 0;

                  if (cUp.a > 0.5) neighbours++;
                  if (cDown.a > 0.5) neighbours++;
                  if (cLeft.a > 0.5) neighbours++;
                  if (cRight.a > 0.5) neighbours++;

                  if (neighbours > 0 && neighbours < 5)
                  {
                      c.a = outlineEffect;
                      c.r = outlineEffect + hitEffect;
                      c.g = hitEffect * 2;
                      c.b = hitEffect * 2;
                  }
              }

              o.Albedo = c.rgb * c.a;
              o.Alpha = c.a;

              if(c.a >= 0.5 && c.a < 0.6)
              {
                o.Emission = c.rgb;
              }
          }
          ENDCG
      }

        //Fallback "Transparent/VertexLit"
}
