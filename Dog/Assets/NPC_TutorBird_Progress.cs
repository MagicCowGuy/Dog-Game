using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_TutorBird_Progress : MonoBehaviour
{
    private NPC_TutorBird tbNPC;
    public NPC NPCScript;
    private int watchedChap = -1;
    private int watchedSubChap = -1;
    private GameObject playerObj;
    public NavMeshAgent playerNavMA;
    public int watchCounter;
    public bool diaOpened;
    public bool diaDone;
    private GameObject gameControlObj;
    private SmallTalk_Control stCont;

    // Start is called before the first frame update
    void Start()
    {
      tbNPC = this.GetComponent<NPC_TutorBird>();
      NPCScript = this.GetComponent<NPC>();
      playerObj = GameObject.FindWithTag("Player");
      playerNavMA = playerObj.GetComponent<NavMeshAgent>();
      gameControlObj = GameObject.FindWithTag("GameController");
      stCont = gameControlObj.GetComponent<SmallTalk_Control>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PauseOrderNPC(){
      //Space for holding NPC while interacting with player.
      diaOpened = true;
    }

    public void ResumeOrderNPC(){
      //Space for finishing dialogue with player.
      diaDone = true;
    }

    public void updateWatcher(){
      diaOpened = false;
      diaDone = false;
      watchCounter = 0;
      NPCScript = this.GetComponent<NPC>();
      //print("UPDATING THE WATCHER");
      if(NPCScript.curProgChapter != watchedChap || NPCScript.curProgSubChapter != watchedSubChap){
        //print("SO HE WAS THE DOCTOR ALL THE TIME");
          watchedChap = NPCScript.curProgChapter;
          watchedSubChap = NPCScript.curProgSubChapter;
          StopAllCoroutines();
          StartCoroutine("watchStat_"+ watchedChap + "_" + watchedSubChap);
      }
    }

    IEnumerator watchStat_0_0() {
      print("Watching for completion of moving Tutorial");
      while(!diaOpened || !diaDone){
        yield return new WaitForSeconds(0.1f);
      }
      while(true){
        if(playerNavMA != null){
          if(playerNavMA.velocity.magnitude > 0.2f){
            watchCounter += 1;
          }
        }
        if(watchCounter > 10){
          watchCounter = 0;
          NPCScript.ProgressUpdate(0,1);
          yield break;
        }
        yield return new WaitForSeconds(0.2f);
      }
      yield break;
    }

    IEnumerator watchStat_0_1() {
      print("Watching for completion of Rotating Tutorial");
      while(!diaOpened || !diaDone){
        yield return new WaitForSeconds(0.1f);
      }
      Quaternion startRot = playerObj.transform.rotation;
      while(true){
        if(Quaternion.Angle(startRot, playerObj.transform.rotation) > 30){
          NPCScript.ProgressUpdate(0,2);
        }
        yield return new WaitForSeconds(0.2f);
      }
    }

    IEnumerator watchStat_0_2() {
      print("Watching for completion of Pickup Tutorial");
      yield break;
    }
}
