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
    private TouchMovement playerScript;
    public int watchCounter;
    private GameObject watchObj;
    public bool diaOpened;
    public bool diaDone;

    public bool alerting;
    public GameObject alertObj;

    private GameObject gameControlObj;
    private SmallTalk_Control stCont;
    private notifyControl notifyConScript;
    private pickupControl pickupConScript;
    private cameraControl camConScript;

    public GameObject ballPrefab;

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
      notifyConScript = gameControlObj.GetComponent<notifyControl>();
      pickupConScript = gameControlObj.GetComponent<pickupControl>();
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
      print("Watching for completion of moving Tutorial");
      alertStatus(true);
      watchCounter = 0;
      yield return new WaitForFixedUpdate();

      while(!diaOpened || !diaDone){

        if(watchCounter == 0 && Vector3.Distance(transform.position, playerObj.transform.position) < 5){
          stCont.startMonoThread(this.gameObject, NPCScript.curSmallTalk[0]);
          watchCounter = 1;
        }

        if(diaOpened && alerting){
          alertStatus(false);
          }

        yield return new WaitForSeconds(0.1f);
      }

      notifyConScript.InstructNote(new NoteToDisplay(3, "Run around the yard.", "Tap locations to run to them."));

      while(true){
        if(playerNavMA != null){
          if(playerNavMA.velocity.magnitude > 0.2f){
            watchCounter += 1;
          }
        }
        if(watchCounter > 10){
          stCont.startMonoThread(this.gameObject, NPCScript.curSmallTalk[1]);

          watchCounter = 0;
          NPCScript.ProgressUpdate(0,1);
          notifyConScript.InstructClear();
          yield break;
        }
        yield return new WaitForSeconds(0.2f);
      }
      //yield break;
    }

    IEnumerator watchStat_0_1() {
      print("Watching for completion of Rotating Tutorial");
      alertStatus(true);
      yield return new WaitForFixedUpdate();
      while(!diaOpened || !diaDone){

        if(diaOpened && alerting){
          alertStatus(false);
        }

        yield return new WaitForSeconds(0.1f);
      }

      notifyConScript.InstructNote(new NoteToDisplay(3, "Spin around on the spot.", "Tap & hold, then drag your finger around."));

      watchCounter = 0;
      while(true){
        Quaternion tempRot = playerObj.transform.rotation;
        yield return new WaitForSeconds(0.2f);
        if(Quaternion.Angle(tempRot, playerObj.transform.rotation) > 1 && playerScript.rotating){
          watchCounter += 1;
        }
        if(watchCounter > 5){
          stCont.startMonoThread(this.gameObject, NPCScript.curSmallTalk[0]);

          NPCScript.ProgressUpdate(0,2);
          notifyConScript.InstructClear();
          yield break;
        }
      }
      //yield break;
    }

    IEnumerator watchStat_0_2() {
      print("Watching for completion of Pickup Tutorial");
      alertStatus(true);
      yield return new WaitForFixedUpdate();
      while(!diaOpened || !diaDone){

        if(diaOpened && alerting){
          alertStatus(false);
        }

        yield return new WaitForSeconds(0.1f);
      }

      pickupConScript.spawnPickup(ballPrefab, true, transform.position + transform.forward * 3.5f, Vector3.zero);

      notifyConScript.InstructNote(new NoteToDisplay(3, "Pick up the ball.", "Tap the ball to pick it up."));

      watchCounter = 0;

      while(watchCounter == 0){
        if(playerScript.holdingObj != null){
          watchCounter = 1;
          stCont.startMonoThread(this.gameObject, NPCScript.curSmallTalk[0]);
          notifyConScript.InstructClear();
          NPCScript.ProgressUpdate(0,3);
          yield break;
        }
        yield return new WaitForSeconds(0.3f);
      }
      //yield break;
    }

    IEnumerator watchStat_0_3() {
      print("Watching for completion of Dropping Tutorial");
      watchCounter = 0;
      notifyConScript.InstructNote(new NoteToDisplay(3, "Drop the ball.", "Tap yourself to drop it."));

      //print("yaypickedup");
      while(watchCounter == 0){
        if(playerScript.holdingObj == null){
          watchCounter = 1;
          stCont.startMonoThread(this.gameObject, NPCScript.curSmallTalk[0]);
          NPCScript.ProgressUpdate(0,4);
          notifyConScript.InstructClear();
          yield break;
        }
        yield return new WaitForSeconds(0.3f);
      }
    }

    IEnumerator watchStat_0_4() {
      print("Watching for completion of Throwing Tutorial");
      alertStatus(true);
      yield return new WaitForFixedUpdate();

      while(!diaOpened || !diaDone){
        if(diaOpened && alerting){
          alertStatus(false);
        }
        yield return new WaitForSeconds(0.1f);
      }

      notifyConScript.InstructNote(new NoteToDisplay(3, "Throw the ball.", "Drag to aim and release to throw."));

      watchObj = null;
      watchCounter = 0;

      while(true){
        if(watchObj == null && playerScript.rotating && playerScript.holdingObj != null){
          watchObj = playerScript.holdingObj;
        }
        if(watchObj != null){
          if(watchObj.GetComponent<Rigidbody>().velocity.y > 5){
            watchObj = null;
            stCont.startMonoThread(this.gameObject, NPCScript.curSmallTalk[0]);
            NPCScript.ProgressUpdate(0,5);
            notifyConScript.InstructClear();
            yield break;
          }
        }
        yield return new WaitForSeconds(0.1f);
      }
      //yield break;
    }

    IEnumerator watchStat_0_5() {
      print("Watching for completion of Trash Task Tutorial");
      alertStatus(true);
      yield return new WaitForFixedUpdate();

      while(!diaOpened || !diaDone){
        if(diaOpened && alerting){
          alertStatus(false);
        }
        yield return new WaitForSeconds(0.1f);
      }
      watchObj = null;
      watchCounter = 0;

      while(true){
        if(watchObj == null && playerScript.holdingObj != null){
          if(playerScript.holdingObj.GetComponent<pickup>().IDTag == "Trash"){
            watchObj = playerScript.holdingObj;
            watchCounter = 1;
          }
        }
        if(watchObj == null && watchCounter == 1){
          print("YOU PUT OUT THE TRAAAAAAAAAAAAAAAAAAAAAAAAAAAAAASH OMG");
          NPCScript.ProgressUpdate(0,6);
          yield break;
        }
        yield return new WaitForSeconds(0.5f);
      }
      //yield break;
    }

    IEnumerator watchStat_0_6() {
      print("Watching for Talk to say Farewell");
      alertStatus(true);
      yield return new WaitForFixedUpdate();

      while(!diaOpened || !diaDone){
        if(diaOpened && alerting){
          alertStatus(false);
        }
        yield return new WaitForSeconds(0.1f);
      }

      tbNPC.StartCoroutine("FlyAway");
      gameControlObj.GetComponent<AchievementsControl>().AchievementUpdate (0, 1);
      stCont.startMonoThread(this.gameObject, NPCScript.curSmallTalk[0]);
      gameControlObj.GetComponent<NPC_Control>().StartSpawning();
      yield break;
    }
}
