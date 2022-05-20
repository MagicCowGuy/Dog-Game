using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_EDITOR

public class NPC_Cat : MonoBehaviour
{
  //public Vector3[] pathPoints;

  public PathNavPoint[] PathNavPoints;
  public Vector3[] lineToDraw;

  public Animator catAnim;

  public bool isPathing;

  private Vector3 curMoveTarget;
  private Vector3 curMoveStart;
  private Vector3 curMovePos;
  private int curPathPoint = 0;
  private float curSpeed;
  private bool isJumping;
  private float moveDist;
  private float vertOffset;
  private float jumpTimer;
  private float moveDelay;

  private Vector3 targetDir;
  private Quaternion tarLookRot;
    // Start is called before the first frame update
    void Start()
    {
      isPathing = true;
      PathNextStep();
      StartCoroutine("PathCoRo");
    }

    // Update is called once per frame
    void Update()
    {

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

          curMovePos = Vector3.Lerp(curMoveStart, curMoveTarget, jumpTimer);
          curMovePos.y = curMovePos.y + Mathf.Sin(Mathf.PI * jumpTimer) * 3.6f;

          transform.position = curMovePos;

          jumpTimer += Time.deltaTime * 1.1f;
          if(jumpTimer > 1){
            PathNextStep();
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

      isJumping = false;

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

        catAnim.SetFloat("Velocity", 0);
      }

      jumpTimer = 0;
      curPathPoint ++;
      curMoveStart = transform.position;
      curMoveTarget = PathNavPoints[curPathPoint].pathPointPos;

      targetDir = (curMoveStart - curMoveTarget).normalized;
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



  }
}
#endif // UNITY_EDITOR
