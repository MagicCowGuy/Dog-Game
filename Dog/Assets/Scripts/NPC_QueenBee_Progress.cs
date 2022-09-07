using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_QueenBee_Progress : MonoBehaviour
{
    private NPC_TutorBird tbNPC;
    public NPC NPCScript;
    private int watchedChap = -1;
    private int watchedSubChap = -1;
    private GameObject playerObj;
    public NavMeshAgent playerNavMA;
    private TouchMovement playerScript;
    public int watchCounter;
    private GameObject watchObj;
    public bool diaOpened;
    public bool diaDone;

    public bool alerting;
    public GameObject alertObj;

    private GameObject gameControlObj;
    private SmallTalk_Control stCont;

    // Start is called before the first frame update
    void Start()
    {
      tbNPC = this.GetComponent<NPC_TutorBird>();
      NPCScript = this.GetComponent<NPC>();
      playerObj = GameObject.FindWithTag("Player");
      playerScript = playerObj.GetComponent<TouchMovement>();
      playerNavMA = playerObj.GetComponent<NavMeshAgent>();
      gameControlObj = GameObject.FindWithTag("GameController");
      stCont = gameControlObj.GetComponent<SmallTalk_Control>();
    }

    // Update is called once per frame
    void Update()
    {
      if(alerting && alertObj != null){
          alertObj.transform.LookAt (Camera.main.transform.position);
      }
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

    public void alertStatus(bool newAlertStatus){
      alerting = newAlertStatus;
      alertObj.SetActive(newAlertStatus);
    }

    IEnumerator watchStat_0_0() {
      //print("Bee wants to talk flowers");
      alertStatus(true);
      watchCounter = 0;
      yield return new WaitForFixedUpdate();

      while(!diaOpened || !diaDone){

        yield return new WaitForSeconds(0.5f);
      }

      alertStatus(false);
      stCont.startMonoThread(this.gameObject, NPCScript.curSmallTalk[0]);
      while(true){
        print("Bee watching for you to water flowers");
        yield return new WaitForSeconds(2.2f);
      }
      //yield break;
    }
}
