using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_TutorBird : MonoBehaviour
{

  public UnityEngine.AI.NavMeshAgent mNavMeshA;
  public Transform playerObj;
  private Animator myanim;
  private bool rotToTarget;
  private NPC NPCScript;

  private Vector3 flyTarget;

  private int soundRefPrev;
  private int soundRefCur;

  private bool flying = false;
  private Quaternion lookRot;
  private Vector3 lookDir;
  private Vector3 groundPos;
  private Vector3 flyVelocity;
  private float takeOff;
  private float flyMaxSpeed;

    // Start is called before the first frame update
    void Start()
    {
        mNavMeshA = this.GetComponent<NavMeshAgent>();
        NPCScript = this.GetComponent<NPC>();
        myanim = GetComponent<Animator>();
        playerObj = GameObject.FindWithTag("Player").transform;
        StartCoroutine("FlyIn");
    }

    // Update is called once per frame
    void Update()
    {

      myanim.SetFloat("Velocity", mNavMeshA.velocity.magnitude/mNavMeshA.speed);

      if(!flying && mNavMeshA.velocity.magnitude < 2 && rotToTarget){
        Vector3 tarDir = playerObj.position - transform.position;
        tarDir.y = 0;
        Quaternion tarRot = Quaternion.LookRotation(tarDir);
        //targetDirection.x = y;
        transform.rotation = Quaternion.Lerp(transform.rotation, tarRot, 2 * Time.deltaTime);
        if(Quaternion.Angle(transform.rotation, tarRot) > 10){
          myanim.SetBool("Rotating", true);
        } else {
          myanim.SetBool("Rotating", false);
        }
      } else {
        myanim.SetBool("Rotating", false);
      }

    }

    IEnumerator FollowPlayer(){
      while(true){
        if(Vector3.Distance(playerObj.position, transform.position) > 10){
          mNavMeshA.destination = playerObj.position;
          rotToTarget = false;
        } else {
          mNavMeshA.ResetPath();
          rotToTarget = true;
        }
        yield return new WaitForSeconds(1);
      }
    }

    public void TalkTrigger(){
      if(!flying){
      myanim.SetTrigger("Squawk");
      }
      while(soundRefPrev == soundRefCur){
        soundRefCur = Random.Range(0, NPCScript.talkingHappy.Length);
      }
      publicSound(0.65f,NPCScript.talkingHappy[soundRefCur]);
      soundRefPrev = soundRefCur;
    }

    public void PauseOrderNPC(){
      //Space for holding NPC while interacting with player.
    }

    public void ResumeOrderNPC(){
      //Space for finishing dialogue with player.
    }

    public IEnumerator FlyIn(){
      flying = true;
      myanim.SetTrigger("Fly");
      flyTarget = new Vector3(Random.Range(-14,16), 5.5f, Random.Range(-10,-24));
      flyMaxSpeed = 0.3f;
      while (flying){
        flyVelocity += (flyTarget - transform.position).normalized * 0.01f;
        flyVelocity = Vector3.ClampMagnitude(flyVelocity, flyMaxSpeed);
        transform.position += flyVelocity;
        
        lookDir = (flyTarget - transform.position).normalized;
        lookDir = new Vector3(0,0, lookDir.z);
        lookRot = Quaternion.LookRotation(lookDir);
        transform.rotation = lookRot;

        if(flyTarget.y > 0 && Vector3.Distance(flyTarget,transform.position) < 2.5f){
          flyTarget.y = -2.35f;
          flyMaxSpeed = 0.1f;
        }

        if(Vector3.Distance(flyTarget,transform.position) < 0.3f){
          mNavMeshA.enabled = true;
          flying = false;
          myanim.SetTrigger("Land");
        }
      
        yield return null;
      }
      StartCoroutine("FollowPlayer");
      yield break;
    }

    public IEnumerator FlyAway(){
      StopCoroutine("FollowPlayer");
      mNavMeshA.enabled = false;
      rotToTarget = false;
      flying = true;
      myanim.SetTrigger("Fly");
    
      groundPos = transform.position;
      flyTarget = transform.position + (Vector3.up * 4.5f);
      takeOff = 0;
      flyMaxSpeed = 0.1f;
      while(takeOff < 1){
        flyVelocity += (flyTarget - transform.position).normalized * 0.01f;
        flyVelocity = Vector3.ClampMagnitude(flyVelocity, flyMaxSpeed);
        transform.position += flyVelocity;
        //transform.position = Vector3.Slerp(groundPos,flyTarget,takeOff);
        takeOff += 0.01f;
        yield return null;
      }

      flyTarget = new Vector3(Random.Range(-10,10),-1,-10);
      flyMaxSpeed = 0.3f;

      while(true){
        
        lookDir = (flyTarget - transform.position).normalized;
        lookDir = new Vector3(0,0, lookDir.z);
        lookRot = Quaternion.LookRotation(lookDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 0.5f);
        
        flyVelocity += (flyTarget - transform.position).normalized * 0.01f;
        //flyVelocity += transform.forward * 0.01f;
        flyVelocity = Vector3.ClampMagnitude(flyVelocity, flyMaxSpeed);
        transform.position += flyVelocity;
        

        flyTarget += (Vector3.up * 0.2f) - (Vector3.forward * 0.1f);
        yield return null;
      }
      //yield break;
    }

    public void publicSound(float soundVolume, AudioClip soundFile){
      //GetComponent<AudioSource>().volume = soundVolume;
      //GetComponent<AudioSource>().pitch = soundPitch;
      GetComponent<AudioSource>().volume = 0.5f + Random.Range(-0.1f, 0.1f);
      GetComponent<AudioSource>().pitch = 1.25f + Random.Range(-0.2f,0.2f);
      GetComponent<AudioSource>().PlayOneShot(soundFile, soundVolume);
    }
}
