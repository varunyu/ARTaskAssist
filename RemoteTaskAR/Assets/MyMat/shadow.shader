Shader "Custom/TransparentShadowReceiver"
{
 
Properties
{
    _ShadowIntensity ("Shadow Intensity", Range (0, 1)) = 0.6
}
 
 
SubShader
{
    Tags
    {
    "Queue"="AlphaTest"
    "IgnoreProjector"="True"
    "RenderType"="Transparent"
    }
 
    LOD 300
 
    // Shadow Pass : Adding the shadows (from Directional Light)
    // by blending the light attenuation
    Pass
    {
        Blend SrcAlpha OneMinusSrcAlpha
        Name "ShadowPass"
        Tags {"LightMode" = "ForwardBase"}
 
        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile_fwdbase
        #pragma fragmentoption ARB_fog_exp2
        #pragma fragmentoption ARB_precision_hint_fastest
        #include "UnityCG.cginc"
        #include "AutoLight.cginc"
 
        struct v2f {
            fixed4 pos : SV_POSITION;
            LIGHTING_COORDS(3,4)
            fixed3    lightDir:LIGHTDIR;
        };
 
        fixed _ShadowIntensity;
 
        v2f vert (appdata_base v)
        {
            v2f o;
            o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
            o.lightDir = ObjSpaceLightDir( v.vertex );
            TRANSFER_VERTEX_TO_FRAGMENT(o);
            return o;
        }
 
        fixed4 frag (v2f i) : COLOR
        {
            fixed atten = LIGHT_ATTENUATION(i);
 
            fixed4 c;
            c.rgb =  0;
            c.a = (1-atten) * _ShadowIntensity;
            return c;
        }
        ENDCG
    }
}
 
FallBack "Transparent/Cutout/VertexLit"
}