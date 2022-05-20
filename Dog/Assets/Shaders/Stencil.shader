Shader "Unlit/Stencil"
{
Properties {
    _Color ("Main Color", COLOR) = (1,1,1,1)
}
SubShader {

Stencil {
          Ref 1
          Comp always
          Pass replace
      }

    Pass {
        Material {
            Diffuse [_Color]
        }
        Lighting Off
    }
}
}
