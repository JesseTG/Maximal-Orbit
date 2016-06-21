Shader "Custom/Moving Radial Gradient" {
 Properties {
     _Color ("Color", Color) = (1,0,0,1)
     _Color2 ("Color 2", Color) = (0,0,1,1)
     _WaveLength ("Wavelength", Float) = 1
     _WaveSpeed ("Wave Speed", Float) = 1
     _Origin ("Origin", Vector) = (0,0,0,0)
 }
 SubShader {
 Pass {
    CGPROGRAM
    #pragma vertex vert
    #pragma fragment frag
    #pragma target 3.0

    #include "UnityCG.cginc"
    
    fixed4 _Color;
    fixed4 _Color2;
    float _WaveLength;
    float _WaveSpeed;
    float4 _Origin;
    
    float4 vert(float4 v : POSITION) : POSITION {
        return mul (UNITY_MATRIX_MVP, v);
    }

    fixed4 frag(float4 sp : WPOS) : COLOR {
        float2 diff = _Origin.xy - float2(sp.x,  _ScreenParams.y - sp.y);
        float d = length(diff);
        
        return lerp(_Color, _Color2,  (sin((d + _Time.y * _WaveSpeed) / _WaveLength) + 1) * .5);
    }
    ENDCG
 } 
}
 FallBack "Legacy/Unlit/Color"
}