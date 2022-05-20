Shader "Unlit/Water_Mobile_Test2"
{
Properties {
    _MainTex ("Base (RGB)", 2D) = "white" {}
    [Header(UV Waves Animation)]
    _UVWaveSpeed ("Speed", Float) = 1
    _UVWaveAmplitude ("Amplitude", Range(0.001,0.5)) = 0.05
    _UVWaveFrequency ("Frequency", Range(0,10)) = 1
}
SubShader {
Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
Blend SrcAlpha One

    CGPROGRAM
    #pragma surface surf Lambert vertex:vert

    sampler2D _MainTex;
    float4 _MainTex_ST;


    half _UVWaveAmplitude;
    half _UVWaveFrequency;
    half _UVWaveSpeed;
    struct Input {
        float2 st_MainTex;
        half2 texcoord;
  			half2 sinAnim;
    };

#define TIME (_Time.y)

    void vert (inout appdata_full v, out Input o)
    {
        UNITY_INITIALIZE_OUTPUT(Input,o);

        o.st_MainTex = TRANSFORM_TEX(v.texcoord, _MainTex);

        // add distortion
        // this is the part you need to modify, i  recomment to expose such
        // hard-coded values to the inspector for easier tweaking.
        //o.st_MainTex.x += sin((o.st_MainTex.x+o.st_MainTex.y)*8 + _Time.g*1.3)*0.01;
        //o.st_MainTex.y += cos((o.st_MainTex.x-o.st_MainTex.y)*8 + _Time.g*2.7)*0.01;


        half2 x = ((v.vertex.xy+v.vertex.yz) * _UVWaveFrequency) + (TIME.xx * _UVWaveSpeed);
        o.sinAnim = x;

    }

    void surf (Input IN, inout SurfaceOutput o) {

    half2 uvDistort = ((sin(0.9*IN.sinAnim.xy) + sin(1.33*IN.sinAnim.xy+3.14) + sin(2.4*IN.sinAnim.xy+5.3))/3) * _UVWaveAmplitude;
    IN.texcoord.xy += uvDistort.xy;
    fixed4 mainTex = tex2D(_MainTex, IN.texcoord.xy);


        half4 c = tex2D (_MainTex, IN.st_MainTex);
        o.Albedo = c.rgb;
        o.Alpha = mainTex.a;
    }
    ENDCG
}
FallBack "Diffuse"
}
