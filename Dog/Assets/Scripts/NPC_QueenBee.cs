using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_EDITOR

public class NPC_QueenBee : MonoBehaviour
{
    public Vector3[] interestPoints;
    private int pointRef;

    private Vector3 curInterestPos;
    private float flyOffset;
    public Vector3 targetDirect;
    public Vector3 tarDirNorm;
    public Vector3 tarDirEul;
    private Vector3 momentum;
    public float tiltAngle = 45;
    public Quaternion targetTiltRot;

    public GameObject playerObj;

    private Quaternion tiltQuat;
    private Quaternion spinQuat;

    public float yAngle;

    private float RdmSinEffect;
    // Start is called before the first frame update
    void Start()
    {
      flyOffset = 2.5f;
      playerObj = GameObject.FindWithTag("Player");
      StartCoroutine("flyAbout");
      //curInterestPos = new Vector3(4,4,-9);
    }

    // Update is called once per frame
    void Update()
    {
      RdmSinEffect = Mathf.Sin(2.5f * Time.time) * 0.05f;
      targetDirect = curInterestPos - transform.position;

      momentum += Vector3.Normalize(targetDirect) * 0.0015f;
      momentum = Vector3.ClampMagnitude(momentum, 0.09f);

      tarDirNorm = Vector3.Normalize(targetDirect);
      transform.position += momentum;

      Vector3 flatTarDir = new Vector3(targetDirect.x,0,targetDirect.z);

      //ROTATION
      Vector3 tiltInfo = Vector3.Normalize(flatTarDir * 10) * tiltAngle;
      Vector3 tiltRotVect = new Vector3(tiltInfo.z, 0, -tiltInfo.x);
      //Vector3 tiltRotVect = new Vector3(Mathf.Abs(tiltInfo.z), 0, tiltInfo.x);

      //targetTiltRot = Quaternion.Euler(tiltRotVect);

      yAngle = Vector3.Angle(flatTarDir, Vector3.forward);
      if(targetDirect.x < 0){
        yAngle = -yAngle;
      }
      tiltQuat = Quaternion.RotateTowards(tiltQuat, Quaternion.Euler(tiltRotVect), 5);
      spinQuat = Quaternion.RotateTowards(spinQuat, Quaternion.AngleAxis(yAngle, Vector3.up), 3);
      targetTiltRot = tiltQuat * spinQuat;
      //targetTiltRot = Quaternion.LookRotation(flatTarDir, new Vector3(0,2,0)) * Quaternion.Euler(tiltRotVect);
      //targetTiltRot.y = yAngle;
      transform.rotation = targetTiltRot;

      //transform.Rotate(0,yAngle,0,Space.Self);
    }

    public void SetupNPC(){
      //Space to setup NPC after spawn point is assigned
    }

    public void PauseOrderNPC(){
      print("BEE PAUSUING");
      StopCoroutine("flyAbout");
      StartCoroutine("hoverAtPlayer");
    }

    public void ResumeOrderNPC(){
      StartCoroutine("flyAbout");
      StopCoroutine("hoverAtPlayer");
    }

    IEnumerator hoverAtPlayer(){
      while(true){
        curInterestPos = playerObj.transform.position + new Vector3(Random.Range(-1, 1),flyOffset + 2,Random.Range(-1, 1));
        yield return new WaitForSeconds(2);
      }
    }

    IEnumerator flyAbout(){
      while(true){
        for(int y = 0; y < 6; y++)
        {
          curInterestPos = interestPoints[pointRef] + new Vector3(Random.Range(-1, 1),flyOffset,Random.Range(-1, 1));
          yield return new WaitForSeconds(2);
        }
        pointRef = Random.Range(0, interestPoints.Length);

        yield return new WaitForSeconds(2);
      }
    }

}

#if UNITY_EDITOR
[CustomEditor(typeof(NPC_QueenBee))]
public class NPC_QueenBeeEditor : Editor
{

  public void OnSceneGUI()
  {
    var LObj = target as NPC_QueenBee;

    if(LObj.interestPoints != null)
    for(int i = 0; i < LObj.interestPoints.Length; i++)
    {
      EditorGUI.BeginChangeCheck();
      Vector3 interestPoint = Handles.PositionHandle(LObj.interestPoints[i], Quaternion.identity);
      if (EditorGUI.EndChangeCheck())
      {
        Undo.RecordObject(target, "Update Point of Interest");
        LObj.interestPoints[i] = interestPoint;
      }
    }
  }
}
#endif // UNITY_EDITOR
