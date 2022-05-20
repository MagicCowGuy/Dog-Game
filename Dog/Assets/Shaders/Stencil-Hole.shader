Shader "Unlit/Stencil-Hole"
{
Properties {
    _Color ("Main Color", COLOR) = (1,1,1,1)
}
SubShader {

Stencil {
        Ref 1
        Comp notequal
      }

    Pass {
        Material {
            Diffuse [_Color]
        }
        Lighting Off
    }
}
}
