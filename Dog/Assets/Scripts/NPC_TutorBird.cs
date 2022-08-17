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

  private int soundRefPrev;
  private int soundRefCur;

    // Start is called before the first frame update
    void Start()
    {
        mNavMeshA = this.GetComponent<NavMeshAgent>();
        NPCScript = this.GetComponent<NPC>();
        myanim = GetComponent<Animator>();
        playerObj = GameObject.FindWithTag("Player").transform;
        StartCoroutine("FollowPlayer");
    }

    // Update is called once per frame
    void Update()
    {

      myanim.SetFloat("Velocity", mNavMeshA.velocity.magnitude/mNavMeshA.speed);

      if(mNavMeshA.velocity.magnitude < 2 && rotToTarget){
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
      myanim.SetTrigger("Squawk");
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

    public void publicSound(float soundVolume, AudioClip soundFile){
      //GetComponent<AudioSource>().volume = soundVolume;
      //GetComponent<AudioSource>().pitch = soundPitch;
      GetComponent<AudioSource>().volume = 0.5f + Random.Range(-0.1f, 0.1f);
      GetComponent<AudioSource>().pitch = 1.25f + Random.Range(-0.2f,0.2f);
      GetComponent<AudioSource>().PlayOneShot(soundFile, soundVolume);
    }
}
