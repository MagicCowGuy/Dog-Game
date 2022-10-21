using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_OldMan : MonoBehaviour
{
    public bool holdforplayer = false;
    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    private Animator myanim;

    public float delayTimer;
    public bool delaying = false;

    //public bool walking = false;

    public int NPCMode = 1;


    public GameObject targetPickup;
    public GameObject heldPickup;
    private Rigidbody pickupRB;

    private GameObject[] possibletargets;
    private GameObject closesttarget;
    private float distfromtarget;
    private float distcheck;
    private float tempdist;

    public Transform leftHand;
    public GameObject gameControlObj;

    public GameObject[] seats;

    public int dirFactor;

    // Start is called before the first frame update
    void Awake()
    {
      gameControlObj = GameObject.Find("GameManagment");

      navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
      myanim = GetComponent<Animator>();

      print("OLD MAN IS " + transform.position.x);
      StartCoroutine("DelayCheck");
      StartCoroutine("CheckForItems");

      seats = GameObject.FindGameObjectsWithTag("NPC_Seat");

    }


    // Update is called once per frame
    void Update()
    {
      myanim.SetFloat("Velocity", navMeshAgent.velocity.magnitude/navMeshAgent.speed);
    }

    public void SetupNPC(){
      if(transform.position.x < 0){
        dirFactor = 1;
      } else {
        dirFactor = -1;
      }
    }

    public void PauseOrderNPC(){
      print("Pause Gramps to talk to him");
      holdforplayer = true;
    }

    public void DelayOrderNPC(float delayLength){

    }

    public void ResumeOrderNPC(){
      holdforplayer = false;
    }

    IEnumerator CheckForItems() {

      while(NPCMode > 0){
        //CHECK FOR DESPAWN

        if(transform.position.x / dirFactor > 40){
          gameControlObj.GetComponent<NPC_Control>().DespawnMob(this.gameObject);
        }

        //DESPAWN CHECK OVER

        if(NPCMode == 1){

        possibletargets = GameObject.FindGameObjectsWithTag ("Pickup");
    		closesttarget = null;
        distfromtarget = Mathf.Infinity;
    		foreach (GameObject postar in possibletargets) {
          if(postar.transform.position.z < -16.7f && postar.GetComponent<pickup>().pickedUp == false){

            distcheck = Vector3.Distance(postar.transform.position, transform.position);
            if(closesttarget == null || distcheck < tempdist){
              tempdist = distcheck;
              closesttarget = postar;
            }
          }
        }

        if(closesttarget != null){
          targetPickup = closesttarget;
          NPCMode = 2;
          print("Oh look it's a " + targetPickup.name);
        }
      }

      yield return new WaitForSeconds(1);
    }
    }

    IEnumerator DelayCheck() {
      //Walking along the path.
      while(NPCMode > 0){

      if(NPCMode == 1){
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance){
          //walking = false;
          delaying = true;
          //delayTimer = 3;
        }
        CheckAndMove();

        }

      //Retrieving items to throw back over fence
      if(NPCMode == 2){

        navMeshAgent.destination = targetPickup.transform.position;
        if(Vector3.Distance(targetPickup.transform.position, transform.position) < 2){

          myanim.SetTrigger("TrigPickup");
    			//navMeshAgent.isStopped = true;
    			delaying = true;
          delayTimer = 2;
          NPCMode = 3;

        }


        if(targetPickup.transform.position.z > -16.7f){
          delaying = false;
          NPCMode = 1;
          targetPickup = null;
          navMeshAgent.isStopped = true;
          navMeshAgent.ResetPath();
          navMeshAgent.isStopped = false;
        }
      }

      if(NPCMode == 3){

        if(delaying){
          if(delayTimer > 0){
            delayTimer -= 0.1f;
          } else {
            delaying = false;
            navMeshAgent.destination = new Vector3 (transform.position.x, transform.position.y, -17f);
            //navMeshAgent.isStopped = false;
          }
        } else {
          if (transform.position.z > -18.2f && myanim.GetCurrentAnimatorStateInfo(0).IsName("Standing")){
            myanim.SetTrigger("TrigThrow");
            //print ("Old man throw ball good");
            navMeshAgent.isStopped = true;
            navMeshAgent.ResetPath();
            navMeshAgent.isStopped = false;

            delaying = true;
            delayTimer = 3;
            NPCMode = 4;
            //navMeshAgent.isStopped = true;

          }
        }
      }

      if(NPCMode == 4){
        if(delaying){
          if(delayTimer > 0){
            delayTimer -= 0.1f;
          } else {
            delaying = false;
            NPCMode =  1;
          }
        }
      }
      yield return new WaitForSeconds(.1f);

    }
    }

    private void CheckAndMove(){
      if(delaying){
        if(delayTimer > 0){
          if(!holdforplayer){
            delayTimer -= 0.1f;
          }
        } else {
          delaying = false;
          navMeshAgent.destination = new Vector3(transform.position.x + (Random.Range(2f,5f) * dirFactor), -2.5f, Random.Range(-23.5f, -20.5f));
          delayTimer = 3;
        }
      }
    }

    void PickupPickup () {
      heldPickup = targetPickup;
      heldPickup.transform.SetParent(leftHand);
      pickupRB = heldPickup.GetComponent<Rigidbody>();
      pickupRB.isKinematic = true;
  		pickupRB.useGravity = false;
  		heldPickup.transform.localPosition = new Vector3 (-0.4f, -0.2f, 0);
    }

    void ThrowPickup(){
      //do this bit
      heldPickup.transform.parent = null;
      pickupRB.isKinematic = false;
  		pickupRB.useGravity = true;
      pickupRB.velocity = new Vector3(0, 13, 13);
  		targetPickup = null;
      heldPickup = null;

      NPCMode = 1;
      //navMeshAgent.destination = transform.position;
      gameControlObj.GetComponent<QuestControl>().QuestProgress(3,1);


    }

}
