using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Postman : MonoBehaviour
{

  public GameObject postmanObj;
  private UnityEngine.AI.NavMeshAgent navMeshAgent;
  private GameObject gameControlObj;
  private Animator myanim;

  private Vector3 posloop;
  public bool stopping;

  public float delayTimer;
  private bool delaying = false;

  public bool holdforplayer = false;

    void Awake (){
      gameControlObj = GameObject.Find("GameManagment");
  		navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
  		myanim = GetComponent<Animator>();
      navMeshAgent.destination = new Vector3(1.75f ,2.47f,-18.5f);
    }
    // Start is called before the first frame update
    void Start()
    {



    }

    public void PauseOrderNPC(){
      print("Pause the postie to talk to him");
      holdforplayer = true;
    }

    public void DelayOrderNPC(float delayLength){

    }

    public void ResumeOrderNPC(){
      holdforplayer = false;
    }

    // Update is called once per frame
    void Update()
    {


      myanim.SetFloat("Velocity", navMeshAgent.velocity.magnitude/navMeshAgent.speed);
      if (!stopping && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance){
        stopping = true;
        myanim.SetBool("Stopping", true);
        delaying = true;
        delayTimer = 5;
        if(transform.position.x > 40){
          gameControlObj.GetComponent<NPC_Control>().DespawnMob(this.gameObject);
        }
      }

      if(delaying){
        if(delayTimer > 0){
          if(!holdforplayer){
            delayTimer -= Time.deltaTime;
          }
        } else {
            delaying = false;
            DepartScreen();
        }
      }
    }

    public void DepartScreen(){
      navMeshAgent.destination = new Vector3(50,2.47f,-18.5f);
      myanim.SetBool("Stopping", false);
      stopping = false;
      print("Postman departing");

    }

}
