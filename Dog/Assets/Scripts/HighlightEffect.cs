using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightEffect : MonoBehaviour
{

    public GameObject LeftHandSide;
    public GameObject RightHandSide;
    public GameObject Middle;

    public GameObject shineGroup;
    private Camera mainCam;
    private Quaternion camDirection;
    private float zalignment;

    // Start is called before the first frame update
    void Start()
    {
      mainCam = Camera.main;

    }

    // Update is called once per frame
    void Update()
    {
      if(shineGroup != null){
        alignToCam();
      }
    }

    private void alignToCam(){
      if(Middle != null){
        zalignment = Quaternion.LookRotation(mainCam.transform.forward, LeftHandSide.transform.position - RightHandSide.transform.position).eulerAngles.z - 90;
      } else {
        zalignment = 0;
      }

      camDirection = Quaternion.LookRotation(transform.position - mainCam.transform.position, Vector3.up);
      foreach (Transform child in shineGroup.transform){
        child.transform.eulerAngles = new Vector3(camDirection.eulerAngles.x, camDirection.eulerAngles.y, zalignment);
      }
    }

    public void pubFlashCall(){
      StartCoroutine("flashEffect");
    }

    IEnumerator flashEffect(){
      ShineOn();
      for (int i = 0; i < 1; i++)
             {
              FlashOn();
              yield return new WaitForSeconds(.15f);
              FlashOff();
              yield return new WaitForSeconds(.15f);
             }
             yield return new WaitForSeconds(.5f);
             ShineOff();
    }

    public void pubPulseFlashCall(){
      StartCoroutine("pulseEffect");
    }

    IEnumerator pulseEffect(){
      //if(shineGroup == null) return;
      ShineOn();
      //transform.GetComponent<Animator>().SetTrigger("Pulse");
      foreach (Transform child in shineGroup.transform){
        child.GetComponent<Animator>().SetTrigger("Pulse");
      }
      for (int i = 0; i < 1; i++)
             {
              FlashOn();
              yield return new WaitForSeconds(.15f);
              FlashOff();
              yield return new WaitForSeconds(.15f);
             }
             yield return new WaitForSeconds(.5f);
    }

    void FlashOn(){
      foreach(Renderer r in transform.GetComponentsInChildren<Renderer>()){
        r.material.SetFloat("HighlightFloat", 0.25f);
      }
    }

    void FlashOff(){
      foreach(Renderer r in transform.GetComponentsInChildren<Renderer>()){
        r.material.SetFloat("HighlightFloat", 0);
      }
    }

    void ShineOn(){
      if(shineGroup != null){
        shineGroup.SetActive(true);
      }
    }

    public void ShineOff(){
      if(shineGroup != null){
        shineGroup.SetActive(false);
      }
    }


}
