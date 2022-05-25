using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_EDITOR

public class NPC_Cat : MonoBehaviour
{
  //public Vector3[] pathPoints;

  public PathNavPoint[] PathNavPoints;
  public Vector3[] lineToDraw;

  public Vector3 idleAreaStart;
  public float idleAreaWidth;
  public float idleAreaDepth;

  public Animator catAnim;

  public bool isPathing;

  public bool isEntering;
  public bool isHangingOut;
  public bool isLeaving;

  private Vector3 idleTargetPos;

  private Vector3 curMoveTarget;
  private Vector3 curMoveStart;
  private Vector3 curMovePos;
  private int curPathPoint = 0;
  private float curSpeed;
  private bool isJumping;
  private bool isSleeping;
  private float moveDist;
  private float vertOffset;
  private float jumpTimer;
  private float moveDelay;

  private Vector3 targetDir;
  private Quaternion tarLookRot;

  public BuildControl buildControlScript;
  public List<int> bedOptions;
  public GameObject targetBed;
  private Interactable targetBedScript;

  public bool goingToBed;

  public UnityEngine.AI.NavMeshAgent catNavMeshA;
    // Start is called before the first frame update
    void Awake()
    {
      isPathing = true;
      isEntering = true;
      isHangingOut = false;
      PathNextStep();
      StartCoroutine("PathCoRoEnter");

      catNavMeshA = GetComponent<UnityEngine.AI.NavMeshAgent>();
      catNavMeshA.updatePosition = false;
      catNavMeshA.enabled = false;
      //catNavMeshA.isStopped = true;

    }

    // Update is called once per frame
    void Update()
    {
      if(isHangingOut || goingToBed){
        catAnim.SetFloat("Velocity", catNavMeshA.velocity.magnitude);
      }
    }

    IEnumerator hangAbout() {

      while(isHangingOut){
        idleTargetPos = new Vector3(Random.Range(idleAreaStart.x, idleAreaStart.x + idleAreaWidth), idleAreaStart.y, Random.Range(idleAreaStart.z , idleAreaStart.z + idleAreaDepth));
        //print("CAT IS HANGING OUT");
        catNavMeshA.destination = idleTargetPos;
        yield return new WaitForSeconds(4);

        //Check for bedOptions
        bedOptions.Clear();

        for(int i = 0; i < buildControlScript.placementSaveList.Count; i++){

          if(buildControlScript.placementSaveList[i] == 2){
            bedOptions.Add(i);
          }
        }

        if(bedOptions.Count > 0){
          int r = Random.Range(0, bedOptions.Count);
          targetBed = buildControlScript.placementAreas[bedOptions[r]].GetComponent<placementLocation>().placedObject;
          //print("Bed selected will be no." + bedOptions[r] + ". Which refers to: " + targetBed);
          targetBedScript = targetBed.GetComponent<Interactable>();
          goingToBed = true;
          isHangingOut = false;
          StartCoroutine("GoToBed");
          yield break;
        }


      }



    }

    IEnumerator GoToBed() {

      while(goingToBed){

        //print("CAT IS GOING TO BED");
        catNavMeshA.destination = Vector3.MoveTowards(targetBed.transform.position, transform.position, 0.25f);
        yield return new WaitForSeconds(1);

        if(Vector3.Distance(transform.position, targetBed.transform.position) < 3){
          print("FOUND MY BED");
          StartCoroutine("jumpOnBed");
          yield break;
        }

      }
    }

    IEnumerator jumpOnBed() {

      isJumping = true;
      curMoveStart = transform.position;
      curMoveTarget = targetBed.transform.position + targetBedScript.posoffset;
      jumpTimer = 0;

      //catNavMeshA.enabled = false;
      catNavMeshA.updatePosition = false;

      while(isJumping){
        curMovePos = Vector3.Lerp(curMoveStart, curMoveTarget, jumpTimer);
        curMovePos.y = curMovePos.y + Mathf.Sin(Mathf.PI * jumpTimer) * 3.5f;

        transform.position = curMovePos;

        jumpTimer += Time.deltaTime * 1.5f;
        if(jumpTimer > 1){
          isJumping = false;
          print("Landed now parent to bed please!");
          transform.SetParent(targetBedScript.parentForPlayer.transform);
          targetBedScript.PlayerLanding(transform.forward);
          StartCoroutine("Sleeping");
          yield break;
        }
        yield return null;
      }
    }

    IEnumerator Sleeping(){
      isSleeping = true;
      catAnim.SetTrigger("Sleep");
        while(isSleeping){

          yield return new WaitForSeconds(4);

        }

      yield break;
    }


    IEnumerator PathCoRoEnter() {

      curMoveStart = transform.position;

      while(isPathing){

        tarLookRot = Quaternion.LookRotation(targetDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, tarLookRot, Time.deltaTime * 8.0f);
//        transform.rotation = Vector3.RotateTowards(transform.forward, targetDir, 15, 0.5f);

        if(isJumping){

          while(moveDelay > 0){
            moveDelay -= 0.1f;
            transform.rotation = Quaternion.Slerp(transform.rotation, tarLookRot, Time.deltaTime * 8.0f);
            yield return null;
          }

          curMovePos = Vector3.Lerp(curMoveStart, curMoveTarget, jumpTimer);
          curMovePos.y = curMovePos.y + Mathf.Sin(Mathf.PI * jumpTimer) * 3.5f;

          transform.position = curMovePos;

          jumpTimer += Time.deltaTime * 1.5f;
          if(jumpTimer > 1){
            moveDelay = 2.5f;
            PathNextStep();
            catAnim.SetTrigger("Land");
          }
        } else {
          transform.position = Vector3.MoveTowards(transform.position,curMoveTarget,curSpeed);
          if(Vector3.Distance(transform.position,curMoveTarget) <= 0.05f){
            PathNextStep();
          }
        }

        yield return null;
      }

      //yield return null;
    }

    private void PathNextStep() {

      if(curPathPoint >= PathNavPoints.Length - 1){
        print("Pathing complete!!!!11111111111");
        isPathing = false;
        isEntering = false;
        //catNavMeshA.isStopped = false;
        catNavMeshA.enabled = true;
        catNavMeshA.updatePosition = true;
        catNavMeshA.Warp(transform.position);
        isHangingOut = true;
        StartCoroutine("hangAbout");

        return;
      }

      if(isJumping){
        isJumping = false;
        moveDelay = 2.5f;
      }

      if(PathNavPoints[curPathPoint].moveType == MovementType.Walk)
      {
        curSpeed = 0.06f;
        catAnim.SetFloat("Velocity", 2.0f);
      }
      if(PathNavPoints[curPathPoint].moveType == MovementType.Run)
      {
        curSpeed = 0.1f;

        catAnim.SetFloat("Velocity", 3.5f);
      }
      if(PathNavPoints[curPathPoint].moveType == MovementType.Jump)
      {
        curSpeed = 0.3f;
        moveDelay = 1.0f;
        isJumping = true;
        catAnim.SetTrigger("Jump");
        catAnim.SetFloat("Velocity", 0);
      }

      jumpTimer = 0;
      curPathPoint ++;
      curMoveStart = transform.position;
      curMoveTarget = PathNavPoints[curPathPoint].pathPointPos;

      targetDir = (curMoveTarget - curMoveStart).normalized;
      if(isJumping){
        targetDir.y = 0;
      }
      moveDist = Vector3.Distance(curMoveStart, curMoveTarget);
    }

}


#if UNITY_EDITOR
[CustomEditor(typeof(NPC_Cat))]
public class NPC_CatEditor : Editor
{
  public void OnSceneGUI()
  {
      var LObj = target as NPC_Cat;

      //Path in to scene
      for(int i = 0; i < LObj.PathNavPoints.Length; i++)
      {
        EditorGUI.BeginChangeCheck();
        Vector3 newPathPoint = Handles.PositionHandle(LObj.PathNavPoints[i].pathPointPos, Quaternion.identity);
//      Handles.DrawLine(LObj.PathNavPoints[i].pathPointPos, LObj.pathPoints[(int)Mathf.Repeat(i+1, LObj.pathPoints.Length)]);
        if (EditorGUI.EndChangeCheck())
        {
          Undo.RecordObject(target, "Update Path Point");
          //LObj.pathPoints[i] = newPathPoint;
          LObj.PathNavPoints[i].pathPointPos = newPathPoint;
        }

        if( i+1 < LObj.PathNavPoints.Length){
          if(LObj.PathNavPoints[i].moveType == MovementType.Walk){
            Handles.color = Color.white;
            Handles.DrawLine(LObj.PathNavPoints[i].pathPointPos, LObj.PathNavPoints[i+1].pathPointPos, 3);
         }

         if(LObj.PathNavPoints[i].moveType == MovementType.Run){
           Handles.color = Color.yellow;
           Handles.DrawLine(LObj.PathNavPoints[i].pathPointPos, LObj.PathNavPoints[i+1].pathPointPos, 3);
         }

         if(LObj.PathNavPoints[i].moveType == MovementType.Jump){
           Handles.DrawBezier(LObj.PathNavPoints[i].pathPointPos, LObj.PathNavPoints[i+1].pathPointPos, LObj.PathNavPoints[i].pathPointPos + 3.5f * Vector3.up, LObj.PathNavPoints[i+1].pathPointPos + 3.5f * Vector3.up, Color.cyan, null, 3);
         }

        }
      }

      //Idle NavMesh Area to roam

      Vector3[] idleAreaVerts = new Vector3[]
              {
                  new Vector3(LObj.idleAreaStart.x + LObj.idleAreaWidth, LObj.idleAreaStart.y, LObj.idleAreaStart.z + LObj.idleAreaDepth),
                  new Vector3(LObj.idleAreaStart.x, LObj.idleAreaStart.y, LObj.idleAreaStart.z + LObj.idleAreaDepth),
                  new Vector3(LObj.idleAreaStart.x, LObj.idleAreaStart.y, LObj.idleAreaStart.z),
                  new Vector3(LObj.idleAreaStart.x + LObj.idleAreaWidth, LObj.idleAreaStart.y, LObj.idleAreaStart.z),

              };


        //EditorGUI.DrawRect(new Rect(LObj.idleAreaCorners[0].x, LObj.idleAreaCorners[0].z, LObj.idleAreaCorners[1].x, LObj.idleAreaCorners[1].z), Color.green);

      Handles.DrawSolidRectangleWithOutline(idleAreaVerts, new Color(0.0f, 0.75f, 0.0f, 0.1f), new Color(0, 0, 0, 1));

    }

}
#endif // UNITY_EDITOR
