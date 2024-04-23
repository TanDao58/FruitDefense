Shader "Custom/WaveEffect"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _CenterX("Center X", float) = 300
        _CenterY("Center Y", float) = 250
        _Amount("Amount", float) = 25
        _Strength("Strength", range(0, 10)) = 3.45
        _ScaleRatio("ScaleRatio",range(0, 50)) = 23
        _Speed("Speed",range(0, 60)) = 11
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

                sampler2D _MainTex;
                float _CenterX;
                float _CenterY;
                float _Amount;
                float _Strength;
                float _ScaleRatio;
                float _Speed;

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f {
                    float2 uv : TEXCOORD0;
                    float4 pos : SV_POSITION;
                };

                v2f vert(
                    float4 vertex : POSITION, // vertex position input
                    float2 uv : TEXCOORD0 // first texture coordinate input
                    )
                {
                    v2f o;
                    o.pos = UnityObjectToClipPos(vertex);
                    o.uv = uv;
                    return o;
                }
                //                CGPROGRAM
//                #pragma vertex vert
//                #pragma fragment frag
//
//                #include "UnityCG.cginc"
//
//                struct appdata
//                {
//                    float4 vertex : POSITION;
//                    float2 uv : TEXCOORD0;
//                };
//
//                struct v2f
//                {
//                    float2 uv : TEXCOORD0;
//                    float4 vertex : SV_POSITION;
//                };
//
//                v2f vert(appdata v)
//                {
//                    v2f o;
//                    o.vertex = UnityObjectToClipPos(v.vertex);
//                    o.uv = v.uv;
//                    return o;
//                }


                //                fixed4 frag(v2f i) : SV_Target
//                {
//                    fixed2 center = fixed2(_CenterX / _ScreenParams.x, _CenterY / _ScreenParams.y);
//                    fixed time = _Time.y * _WaveSpeed;
//                    fixed amt = _Amount / 1000;
//
//                    fixed2 uv = center.xy - i.uv;
//                    uv.x *= _ScreenParams.x / _ScreenParams.y;
//
//                    fixed dist = sqrt(dot(uv,uv));
//                    fixed ang = dist * _WaveAmount - time;
//                    uv = i.uv + normalize(uv) * sin(ang) * amt;
//
//                    return tex2D(_MainTex, uv);
//                }
                fixed4 frag(v2f i) : SV_Target
                {
                    fixed screenPosX = _ScreenParams.x * i.uv.x;
                    fixed screenPosY = _ScreenParams.y * i.uv.y;
                    fixed x = _CenterX - screenPosX;
                    fixed y = _CenterY - screenPosY;
                    fixed amt = _Amount / 10;

                    fixed distance = sqrt(x * x + y * y);
                    fixed maxDistance = _ScreenParams.x / 4;
                    fixed distancePercent = (maxDistance - distance) / maxDistance;

                    fixed time = _Time.y * _Speed;
                    fixed ang = (distance)*_Strength / 100 - time;

                    fixed2 uv = i.uv;

                    if (distance < maxDistance) {
                        uv = i.uv + normalize(fixed2(x,y)) * sin(ang) * _ScaleRatio / 1000 * distancePercent * amt;
                    }
                    

                    //fixed2 center = fixed2(_CenterX / _ScreenParams.x, _CenterY / _ScreenParams.y);
                    //fixed2 uv = center.xy - i.uv;

                    return tex2D(_MainTex, uv);
                }
                ENDCG
            }
        }
}

//Shader "PostEffects/Ripple"
//{
//    Properties
//    {
//        _MainTex("Texture", 2D) = "white" {}
//        _CenterX("Center X", float) = 300
//        _CenterY("Center Y", float) = 250
//        _Amount("Amount", float) = 25
//        _WaveSpeed("Wave Speed", range(.50, 50)) = 20
//        _WaveAmount("Wave Amount", range(0, 20)) = 10
//        _CircleNbr("Circle quantity", Range(10,1000)) = 60.0
//        _Radius("Radius", Range(0, 100)) = 10
//    }
//        SubShader
//        {
//            // No culling or depth
//            Cull Off ZWrite Off ZTest Always
//
//            Pass
//            {
//                CGPROGRAM
//                #pragma vertex vert
//                #pragma fragment frag
//
//                #include "UnityCG.cginc"
//
//                struct appdata
//                {
//                    float4 vertex : POSITION;
//                    float2 uv : TEXCOORD0;
//                };
//
//                struct v2f
//                {
//                    float2 uv : TEXCOORD0;
//                    float4 vertex : SV_POSITION;
//                };
//                
//
//                v2f vert(appdata v)
//                {
//                    v2f o;
//                    o.vertex = UnityObjectToClipPos(v.vertex);
//                    o.uv = v.uv;
//                    return o;
//                }
//
//                sampler2D _MainTex;
//                float _CenterX;
//                float _CenterY;
//                float _Amount;
//                float _WaveSpeed;
//                float _WaveAmount;
//                float _CircleNbr;
//                float _Radius;
//
//                struct Input {
//                    float2 uv_MainTex;
//                    float3 worldPos;
//                };
//
//                fixed4 frag(v2f i) : SV_Target
//                {
//                    fixed2 center = fixed2(_CenterX / _ScreenParams.x, _CenterY / _ScreenParams.y);
//                    fixed time = _Time.y * _WaveSpeed;
//                    fixed amt = _Amount / 1000;
//
//                    
//                    fixed2 direction = center.xy - i.uv;                
//
//                    fixed dist = sqrt(dot(direction, direction));
//                    fixed ang = dist * _WaveAmount - time;                    
//                    
//                    fixed2 r = center - fixed2(0.4, 0.4);
//                    fixed maxDistance = sqrt(dot(r, r));
//                    fixed2 uv = i.uv;
//                    if (dist < maxDistance) {
//                    uv = i.uv + normalize(uv) * sin(ang) * amt;
//                    }
//                    return tex2D(_MainTex, uv);
//                }
//                ENDCG
//            }
//        }
    //Properties{
    //    _OrigineX("PosX Origine", Range(0,1)) = 0.5
    //    _OrigineY("PosY Origine", Range(0,1)) = 0.5
    //    _Speed("Speed", Range(-100,100)) = 60.0
    //    _CircleNbr("Circle quantity", Range(10,1000)) = 60.0
    //    _Color("Main Color", Color) = (1,1,1,1)
    //}

    //    SubShader{
    //        Pass {
    //            CGPROGRAM
    //            #pragma vertex vert
    //            #pragma fragment frag
    //            #include "UnityCG.cginc"
    //            #pragma target 3.0

    //            float _OrigineX;
    //            float _OrigineY;
    //            float _Speed;
    //            float _CircleNbr;
    //            fixed4 _Color;

    //            struct vertexInput {
    //                float4 vertex : POSITION;
    //                float4 texcoord0 : TEXCOORD0;
    //            };

    //            struct fragmentInput {
    //                float4 position : SV_POSITION;
    //                float4 texcoord0 : TEXCOORD0;
    //            };

    //            fragmentInput vert(vertexInput i) {
    //                fragmentInput o;
    //                o.position = UnityObjectToClipPos(i.vertex);
    //                o.texcoord0 = i.texcoord0;
    //                return o;
    //            }



    //            fixed4 frag(fragmentInput i) : SV_Target {
    //                fixed4 color;
    //                float distanceToCenter;
    //                float time = _Time.x * _Speed;

    //                float xdist = _OrigineX - i.texcoord0.x;
    //                float ydist = _OrigineY - i.texcoord0.y;

    //                
    //                distanceToCenter = (xdist * xdist + ydist * ydist) * _CircleNbr;
    //                if (distanceToCenter < 5) {
    //                    color = sin(distanceToCenter + time) + _Color;
    //                }
    //                else {
    //                    color = _Color;
    //                }
    //                
    //                return color;
    //            }
    //            ENDCG
    //        }
    //}
//}
//Shader "Hidden/NewImageEffectShader"
//{
//    Properties
//    {
//        _MainTex("Texture", 2D) = "white" {}
//        _CenterX("Center X", float) = 300
//        _CenterY("Center Y", float) = 250
//        _Amount("Amount", float) = 10
//        _WaveSpeed("Wave Speed", range(.50, 50)) = 20
//        _WaveAmount("Wave Amount", range(0, 20)) = 10
//    }
//        SubShader
//        {
//            // No culling or depth
//            Cull Off ZWrite Off ZTest Always
//
//            Pass
//            {
//                CGPROGRAM
//                #pragma vertex vert
//                #pragma fragment frag
//
//                #include "UnityCG.cginc"
//
//                struct appdata
//                {
//                    float4 vertex : POSITION;
//                    float2 uv : TEXCOORD0;
//                };
//
//                struct v2f
//                {
//                    float2 uv : TEXCOORD0;
//                    float4 vertex : SV_POSITION;
//                };
//
//                v2f vert(appdata v)
//                {
//                    v2f o;
//                    o.vertex = UnityObjectToClipPos(v.vertex);
//                    o.uv = v.uv;
//                    return o;
//                }
//
//                sampler2D _MainTex;
//                float _CenterX;
//                float _CenterY;
//                float _Amount;
//                float _WaveSpeed;
//                float _WaveAmount;
//
//                fixed4 frag(v2f i) : SV_Target
//                {
//                    fixed2 center = fixed2(_CenterX / _ScreenParams.x, _CenterY / _ScreenParams.y);
//                    fixed time = _Time.y * _WaveSpeed;
//                    fixed amt = _Amount / 1000;
//
//                    fixed2 uv = center.xy - i.uv;
//                    uv.x *= _ScreenParams.x / _ScreenParams.y;
//
//                    fixed dist = sqrt(dot(uv,uv));
//                    fixed ang = dist * _WaveAmount - time;
//                    uv = i.uv + normalize(uv) * sin(ang) * amt;
//
//                    return tex2D(_MainTex, uv);
//                }
//                ENDCG
//            }
//        }
//}