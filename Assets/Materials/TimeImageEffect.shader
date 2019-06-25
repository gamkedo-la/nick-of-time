Shader "Custom/TimeImageEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            uniform float value;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                /*
                //Wobble Effect
                float val = (sin(_Time.w * 10)) / 25;
                if (val < 0) val *= -1;
                o.vertex.w += val * value;
                */

                o.uv = v.uv;

                return o;
            }

            sampler2D _MainTex;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                float var = sin(_Time.w);
                if(var < 0) var *= -1;
                var -= 2;

                if(col.g > 0.8)
                  col.g += value / (4 * var);
                else if(col.g > 0.6)
                  col.g += value / (2 * var);
                else if(col.g > 0.4)
                  col.g -= value / (2 * var);
                else if(col.g > 0.2)
                  col.g -= value / (4 * var);

                if(col.r < 0.75)
                  col.r -= value;

                if (col.b < 0.75)
                  col.b -= value;

                return col;
            }
            ENDCG
        }
    }
}
