// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Custom/LightSpriteForWaterTiles"
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

          float rnd(float2 x)
          {
            int n = int(x.x * 40.0 + x.y * 6400.0);
            n = (n << 13) ^ n;
            return 1.0 - float((n * (n * n * 15731 + 789221) + \
              1376312589) & 0x7fffffff) / 1073741824.0;
          }

          float4 _MainTex_TexelSize;

          void vert(inout appdata_full v, out Input o)
          {
              v.vertex = UnityFlipSprite(v.vertex, _Flip);

              #if defined(PIXELSNAP_ON)
              v.vertex = UnityPixelSnap(v.vertex);
              #endif

              UNITY_INITIALIZE_OUTPUT(Input, o);
              o.color = v.color * _Color * _RendererColor;

              v.vertex.y += ((sin(_Time.y) + 1) / 2) / rnd(float2(o.uv_MainTex.x * _MainTex_TexelSize.x, o.uv_MainTex.y * _MainTex_TexelSize.y)) / 200;
          }

          void surf(Input IN, inout SurfaceOutput o)
          {
              fixed4 c = SampleSpriteTexture(IN.uv_MainTex) * IN.color;

              fixed4 cRight = SampleSpriteTexture(float2(IN.uv_MainTex.x + _MainTex_TexelSize.x * ((sin(_Time.y) + 1) / 2), IN.uv_MainTex.y)) * IN.color;
              c = cRight;

              if (c.g >= 0.9 && c.b >= 0.9 && c.r < 0.8)
              {
                c.b = 0.9 + (((sin(_Time.y) + 1) / 2) / 10);
                c.g = 0.9 + (((sin(_Time.y) + 1) / 2) / 10);
                c.r = 0.75 + (((sin(_Time.y) + 1) / 2) / rnd(float2(IN.uv_MainTex.x * _MainTex_TexelSize.x, IN.uv_MainTex.y * _MainTex_TexelSize.y)) / 10);
              }
              else if (c.b >= 0.9 && c.g < 0.9 && c.r < 0.5)
              {
                c.b = 0.8 + (((sin(_Time.y) + 1) / 2) / 10);
                c.g = 0.9 + (((sin(_Time.y) + 1) / 2) / 8);
              }
              else if(c.g >= 0.9 && c.r < 0.5)
              {
                c.g = 0.9 - (((sin(_Time.y) + 1) / 2) / 10);
              }

              o.Albedo = c.rgb * c.a;
              o.Alpha = c.a;
          }
          ENDCG
      }

        //Fallback "Transparent/VertexLit"
}
