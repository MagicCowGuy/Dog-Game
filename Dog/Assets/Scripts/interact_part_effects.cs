using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interact_part_effects : MonoBehaviour
{

    public ParticleSystem partSys;
    public Interactable interactScript;
    public GameObject playerObj;
    public TouchMovement playerMoveScript;

    private float delayStop;

    void Start(){
      //interactScript = this.GetComponent<Interactable>();
      playerObj = GameObject.Find("Solo_Player");
      playerMoveScript = playerObj.GetComponent<TouchMovement>();
    }

void Awake(){
  playerObj = GameObject.Find("Solo_Player");
  playerMoveScript = playerObj.GetComponent<TouchMovement>();
}

public void PlayerLandingFunction(){
  partSys.Play();
  delayStop = 0.5f;
  print("palyer has landed do part effects please!");
}

    // Update is called once per frame
    void Update()
    {



      if (interactScript.interacting && playerMoveScript.rotating) {
        if(!partSys.isPlaying){
          partSys.Play();
        }
      } else {
        if(partSys.isPlaying && delayStop < 0.1f){
          partSys.Stop();
        }
      }

      delayStop -= 0.1f;
    }
}
