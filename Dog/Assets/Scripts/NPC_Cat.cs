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
  public PathNavPoint[] PathEnter;
  public PathNavPoint[] PathExit;
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
  private bool isPrepJump;
  private bool isSleeping;
  private float moveDist;
  private float vertOffset;
  private float jumpTimer;
  private float moveDelay;
  private float sleepTimer;
  private float hangTimer;

  private Vector3 targetDir;
  private Quaternion tarLookRot;

  public BuildControl buildControlScript;
  public List<int> bedOptions;
  public GameObject targetBed;
  private Interactable targetBedScript;
  public Transform catHead;

  public bool goingToBed;

  private GameObject gameControlObj;

  public UnityEngine.AI.NavMeshAgent catNavMeshA;
    // Start is called before the first frame update
    void Awake()
    {
      PathNavPoints = PathEnter;

      gameControlObj = GameObject.Find("GameManagment");
      buildControlScript = gameControlObj.GetComponent<BuildControl>();

      isPathing = true;
      isEntering = true;
      isHangingOut = false;

      hangTimer = Random.Range(3, 5);
      PathNextStep();
      StartCoroutine("PathCoRo");

      catNavMeshA = GetComponent<UnityEngine.AI.NavMeshAgent>();
      catNavMeshA.updatePosition = false;
      catNavMeshA.enabled = false;
      //catNavMeshA.isStopped = true;

    }

    // Update is called once per frame
    void Update()
    {
      if(catNavMeshA.updatePosition){
        catAnim.SetFloat("Velocity", catNavMeshA.velocity.magnitude);
      }
    }

    public void SetupNPC(){
      //Space to setup NPC after spawn point is assigned
    }

    IEnumerator hangAbout() {

      while(isHangingOut){
        if(hangTimer <= 0){
          StartCoroutine("goToLeave");
          yield break;
        }
        idleTargetPos = new Vector3(Random.Range(idleAreaStart.x, idleAreaStart.x + idleAreaWidth), idleAreaStart.y, Random.Range(idleAreaStart.z , idleAreaStart.z + idleAreaDepth));
        //print("CAT IS HANGING OUT");
        catNavMeshA.destination = idleTargetPos;
        yield return new WaitForSeconds(4);

        //Check for bedOptions
        bedOptions.Clear();

        if(Random.Range(0, 10) > 8){
          for(int i = 0; i < buildControlScript.placementSaveList.Count; i++){
            if(buildControlScript.placementSaveList[i] == 2 || buildControlScript.placementSaveList[i] == 4){
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

        hangTimer -= 0.1f;
      }



    }

    IEnumerator GoToBed() {

      while(goingToBed){
        targetBed.GetComponent<NavMeshObstacle>().carving = false;
        //Wait a frame for NavMesh to update
        yield return null;
        targetBed.GetComponent<NavMeshObstacle>().carving = true;

        catNavMeshA.destination = Vector3.MoveTowards(targetBed.transform.position, transform.position, 0.25f);
        yield return new WaitForSeconds(1);

        if(Vector3.Distance(catHead.position, targetBed.transform.position) < targetBedScript.radius + 0.95f){
          if(targetBedScript.interacting){
            //print("THE BED IS BUSY GET OFF MY BED DOG");
            //Waiting for bed to become available. maybe program in something here.
          }else{
            targetBedScript.interacting = true;
            //print("FOUND MY BED");
            goingToBed = false;
            StartCoroutine("jumpOnBed");
            yield break;
          }
        }

      }
    }

    IEnumerator goToLeave() {
      isLeaving = true;
      PathNavPoints = PathExit;

      catNavMeshA.destination = PathNavPoints[0].pathPointPos;
      print("Cat leaving");
      while(isLeaving){
        if(Vector3.Distance(transform.position, catNavMeshA.destination) < 3){
          catNavMeshA.updatePosition = false;
          isPathing = true;
          curPathPoint = 0;
          PathNextStep();
          StartCoroutine(PathCoRo());
        }
        yield return new WaitForSeconds(1);
      }
      yield return null;
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

        tarLookRot = Quaternion.LookRotation(curMoveTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, tarLookRot, Time.deltaTime * 8.0f);
        //transform.rotation.y = 0;

        jumpTimer += Time.deltaTime * 1.5f;
        if(jumpTimer > 1){
          isJumping = false;
          //print("Landed now parent to bed please!");
          transform.SetParent(targetBedScript.parentForPlayer.transform);
          targetBedScript.PlayerLanding(transform.forward);
          StartCoroutine("Sleeping");
          yield break;
        }
        yield return null;
      }
    }

    IEnumerator jumpOffBed(Vector3 jumpTarget, bool toGround) {

      catAnim.SetTrigger("Jump");
      isJumping = true;
      curMoveStart = transform.position;
      curMoveTarget = jumpTarget;
      jumpTimer = 0;

      catNavMeshA.updatePosition = false;

      while(isJumping){
        transform.parent = null;
        curMovePos = Vector3.Lerp(curMoveStart, curMoveTarget, jumpTimer);
        curMovePos.y = curMovePos.y + Mathf.Sin(Mathf.PI * jumpTimer) * 3.5f;

        transform.position = curMovePos;

        jumpTimer += Time.deltaTime * 1.5f;
        if(jumpTimer > 1){
          isJumping = false;
          catAnim.SetTrigger("Land");
          if(toGround){
            print("Kitty landed on the GROWNEEED");
            catNavMeshA.updatePosition = true;
            catNavMeshA.Warp(transform.position);
            StartCoroutine(goToLeave());
            targetBed = null;
          } else {
            transform.SetParent(targetBedScript.parentForPlayer.transform);
            targetBedScript.PlayerLanding(transform.forward);
            StartCoroutine("Sleeping");
          }
          yield break;
        }
        yield return null;
      }
    }

    IEnumerator Sleeping(){
      isSleeping = true;
      sleepTimer = Random.Range(1, 2);
      catAnim.SetTrigger("Sleep");
        while(isSleeping){

          if(sleepTimer <= 0){
            print("WAKE UP CAT");
            catAnim.SetTrigger("Wake");
            yield return new WaitForSeconds(1);

            targetBed.GetComponent<NavMeshObstacle>().carving = false;
            //Wait a frame for NavMesh to update
            yield return null;
            targetBed.GetComponent<NavMeshObstacle>().carving = true;

            PathNavPoints = PathExit;
            //catNavMeshA.destination = PathNavPoints[0].pathPointPos;

            catNavMeshA.Warp(transform.position);
            NavMeshPath path = new NavMeshPath();
            catNavMeshA.CalculatePath(PathNavPoints[0].pathPointPos, path);
            curMoveTarget = Vector3.MoveTowards(path.corners[0],path.corners[1],targetBedScript.radius + 2.2f);

            targetBedScript.interacting = false;
            StartCoroutine(jumpOffBed(curMoveTarget, true));
            yield break;

          }

          sleepTimer -= 0.1f;
          yield return new WaitForSeconds(2);

        }

      yield break;
    }


    IEnumerator PathCoRo() {

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

          if(isPrepJump){
            catAnim.SetTrigger("Jump");
            catAnim.SetFloat("Velocity", 0);
            isPrepJump = false;
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
        if(isEntering){
          isPathing = false;
          isEntering = false;
          catNavMeshA.enabled = true;
          catNavMeshA.updatePosition = true;
          catNavMeshA.Warp(transform.position);
          isHangingOut = true;
          StartCoroutine("hangAbout");
        } else {
          //destroy cat
          gameControlObj.GetComponent<NPC_Control>().DespawnMob(this.gameObject);
        }

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
        moveDelay = 2.5f;
        isJumping = true;
        isPrepJump = true;
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
  private PathNavPoint[] PathPointsGUI;

  public void OnSceneGUI()
  {
      var LObj = target as NPC_Cat;

      drawPath(LObj.PathEnter);
      drawPath(LObj.PathExit);
      //Path in to scene
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

    public void drawPath(PathNavPoint[] pointsToDraw){
      for(int i = 0; i < pointsToDraw.Length; i++)
      {
        EditorGUI.BeginChangeCheck();
        Vector3 newPathPoint = Handles.PositionHandle(pointsToDraw[i].pathPointPos, Quaternion.identity);
        if (EditorGUI.EndChangeCheck())
        {
          Undo.RecordObject(target, "Update Path Point");
          pointsToDraw[i].pathPointPos = newPathPoint;
        }

        if( i+1 < pointsToDraw.Length){
          if(pointsToDraw[i].moveType == MovementType.Walk){
            Handles.color = Color.white;
            Handles.DrawLine(pointsToDraw[i].pathPointPos, pointsToDraw[i+1].pathPointPos, 3);
         }

         if(pointsToDraw[i].moveType == MovementType.Run){
           Handles.color = Color.yellow;
           Handles.DrawLine(pointsToDraw[i].pathPointPos, pointsToDraw[i+1].pathPointPos, 3);
         }

         if(pointsToDraw[i].moveType == MovementType.Jump){
           Handles.DrawBezier(pointsToDraw[i].pathPointPos, pointsToDraw[i+1].pathPointPos, pointsToDraw[i].pathPointPos + 3.5f * Vector3.up, pointsToDraw[i+1].pathPointPos + 3.5f * Vector3.up, Color.cyan, null, 3);
         }

        }
      }
    }

}
#endif // UNITY_EDITOR
