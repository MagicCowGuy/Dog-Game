using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_TutorBird : MonoBehaviour
{

  public UnityEngine.AI.NavMeshAgent mNavMeshA;
  public Transform playerObj;
    // Start is called before the first frame update
    void Start()
    {
        mNavMeshA = this.GetComponent<NavMeshAgent>();
        playerObj = GameObject.FindWithTag("Player").transform;
        StartCoroutine("FollowPlayer");
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator FollowPlayer(){
      while(true){
        mNavMeshA.destination = playerObj.position;
        yield return new WaitForSeconds(3);
      }
    }



        public void PauseOrderNPC(){
          //Space for holding NPC while interacting with player.
        }

        public void ResumeOrderNPC(){
          //Space for finishing dialogue with player.
        }
}
