Shader "Unlit/WaterMobile"
{
Properties {
    _MainTex ("Base (RGB)", 2D) = "white" {}
}
SubShader {
Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
Blend SrcAlpha One

    CGPROGRAM
    #pragma surface surf Lambert vertex:vert

    sampler2D _MainTex;
    float4 _MainTex_ST;

    struct Input {
        float2 st_MainTex;
    };

    void vert (inout appdata_full v, out Input o)
    {
        UNITY_INITIALIZE_OUTPUT(Input,o);

        o.st_MainTex = TRANSFORM_TEX(v.texcoord, _MainTex);

        // add distortion
        // this is the part you need to modify, i  recomment to expose such
        // hard-coded values to the inspector for easier tweaking.
        o.st_MainTex.x += sin((o.st_MainTex.x+o.st_MainTex.y)*8 + _Time.g*0.5)*0.05;
        o.st_MainTex.y += cos((o.st_MainTex.x-o.st_MainTex.y)*8 + _Time.g*0.5)*0.05;
    }

    void surf (Input IN, inout SurfaceOutput o) {
        half4 c = tex2D (_MainTex, IN.st_MainTex);
        o.Albedo = c.rgb;
        o.Alpha = c.a;
    }
    ENDCG
}
FallBack "Diffuse"
}
