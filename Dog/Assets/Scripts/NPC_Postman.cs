using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_EDITOR

public class NPC_Postman : MonoBehaviour
{

  public GameObject postmanObj;
  private NavMeshAgent postNavMeshA;
  private GameObject gameControlObj;
  private Animator myanim;

  private Vector3 posloop;
  public bool stopping = false;

  public float delayTimer;
  private bool delaying = false;

  public bool holdforplayer = false;

  public Vector3[] postalRoute;
  public int routeProgress;

    void Start (){
      gameControlObj = GameObject.Find("GameManagment");
  		postNavMeshA = GetComponent<NavMeshAgent>();
  		myanim = GetComponent<Animator>();
      //postNavMeshA.destination = new Vector3(1.75f ,2.47f,-18.5f);
      postNavMeshA.destination = postalRoute[routeProgress];
      StartCoroutine(headToPos());
    }

    public void DelayOrderNPC(float delayLength){

    }

    public void SetupNPC(){
      //Space to setup NPC after spawn point is assigned
    }

    public void PauseOrderNPC(){
      holdforplayer = true;
      postNavMeshA.ResetPath();
      myanim.SetTrigger("Stop");
    }

    public void ResumeOrderNPC(){
      holdforplayer = false;
      postNavMeshA.destination = postalRoute[routeProgress];
      myanim.SetTrigger("Start");
    }

    // Update is called once per frame
    void Update()
    {


            if(transform.position.x > 40){
              gameControlObj.GetComponent<NPC_Control>().DespawnMob(this.gameObject);
            }
    }

    public void nextStop(){
      routeProgress ++;
      postNavMeshA.destination = postalRoute[routeProgress];
      myanim.SetTrigger("Start");
    }

    IEnumerator headToPos(){
      while(true){
        myanim.SetFloat("Velocity", postNavMeshA.velocity.magnitude * 0.1f);
        if(!holdforplayer && postNavMeshA.remainingDistance < 2.85f){

          postNavMeshA.ResetPath();
          myanim.SetTrigger("Stop");

          yield return new WaitForSeconds(2);
          nextStop();
        }
        yield return new WaitForSeconds(0.1f);
      }

    }

    public void DepartScreen(){
      postNavMeshA.destination = new Vector3(50,2.47f,-18.5f);
      myanim.SetBool("Stopping", false);
      stopping = false;
      print("Postman departing");

    }

}

#if UNITY_EDITOR
[CustomEditor(typeof(NPC_Postman))]
public class NPC_PostmanEditor : Editor
{

  public void OnSceneGUI()
  {
    var LObj = target as NPC_Postman;

    for(int i = 0; i < LObj.postalRoute.Length; i++)
    {
      EditorGUI.BeginChangeCheck();
      Vector3 postalRoutePoint = Handles.PositionHandle(LObj.postalRoute[i], Quaternion.identity);
      if (EditorGUI.EndChangeCheck())
      {
        Undo.RecordObject(target, "Update Postal Route Path Point");
        LObj.postalRoute[i] = postalRoutePoint;
      }
    }
  }
}
#endif // UNITY_EDITOR
