using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blinking : MonoBehaviour
{

    public Texture eyesOpen, eyesClosed;
    public Renderer m_Renderer;

    // Start is called before the first frame update
    void Start()
    {
      InvokeRepeating ("Blinking", Random.Range(0,3), Random.Range(3,5));
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Blinking(){
      StartCoroutine(Blink());
    }

    IEnumerator Blink(){
      yield return new WaitForSeconds(Random.Range(0.25f,0.65f));

      m_Renderer.material.SetTexture ("_EyesTex", eyesClosed);

      yield return new WaitForSeconds(0.15f);

      m_Renderer.material.SetTexture ("_EyesTex", eyesOpen);
    }
}
