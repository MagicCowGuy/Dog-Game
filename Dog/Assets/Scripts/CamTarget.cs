using UnityEngine;
using System.Collections;

public class CamTarget : MonoBehaviour {

  //private float zoomdistance = 22;
  public Transform focusplayer;
	public float shakeStrength = 0f;
  private Vector3 offsetCamera;
	public Vector3 centerpoint;
	public Vector3 targetpos;
	public Quaternion targetrotation;
	private Vector3 targetvecrotation;
	private Vector3 vecrot;
	public Vector3 targetfocuspos;
	private Vector3 velocity = Vector3.zero;
  public int cameraMode = 1;


	public void Start () {
    StartCoroutine("CheckTargetPos");
		targetpos = new Vector3 (22, 27, -22);
		targetfocuspos = new Vector3 (0, -6.5f, 0);

        Application.targetFrameRate = 60;
    }

public void Awake(){

}

	public void Update () {

    //if(shakeStrength > 0.1)
    //{
    //    offsetCamera.x = Random.Range(-shakeStrength, shakeStrength);
    //    offsetCamera.y = Random.Range(-shakeStrength, shakeStrength);
    //    offsetCamera.z = Random.Range(-shakeStrength, shakeStrength);
    //    shakeStrength = Mathf.Lerp(shakeStrength,0,Time.deltaTime*4f);
    //}
    //else
    //{
    //    shakeStrength = 0;
    //}

      transform.position = Vector3.SmoothDamp(transform.position, targetpos, ref velocity , 30 * Time.deltaTime) + offsetCamera;
      transform.rotation = Quaternion.Slerp(transform.rotation, targetrotation, 2 * Time.deltaTime);
    }

    IEnumerator CheckTargetPos(){
      //Follow Player Mode
      while(cameraMode != 0){
      if(cameraMode == 1){
      targetfocuspos = focusplayer.transform.position;
        if(focusplayer != null)
        {
          Vector3 focusplayerpos = focusplayer.position - new Vector3(0,0,-10);
          //is player on the ground level
          if (focusplayerpos.y < 0.5) {
            if(focusplayerpos.z > 0){
              targetpos = new Vector3 (focusplayerpos.x / 2.5f, 27, focusplayerpos.z / 3f - 35);
              if (focusplayerpos.z < 13) {
                targetrotation = Quaternion.Euler (50f - (focusplayerpos.z * 1.2f), 0, 0);
              }
              if (focusplayerpos.x < -10){
                targetpos = new Vector3 (-11.3f, 28.3f, -32.3f);
              }
            } else {
                //Near front fence line.
              if (focusplayerpos.y < -2) {
                targetpos = new Vector3 (focusplayerpos.x / 2.5f, 39.5f, focusplayerpos.z / 3f - 52.5f);
              }
              //targetpos = new Vector3 (focusplayerpos.x / 2.5f, 33, focusplayerpos.z / 3f - 48.5f);
            }
          } else {
          //back garden
          //above ground level
           if (focusplayerpos.z > 10.5f) {
             //print("OMG PORTCH __ " + focusplayerpos.z);
            //front (probably portch)
            targetpos = new Vector3 (1, 24, -23.5f);
            targetrotation = Quaternion.Euler (40, 0, 0);
            }
           }
          }
        }

        //Overview of Front Yard Mode
        if(cameraMode == 2){
          targetpos = new Vector3 (1, 32, -40);
          targetrotation = Quaternion.Euler (45, 0, 0);
        }

        yield return new WaitForSeconds(0.2f);
      }
    }
    public void SetTarget (Transform yourplayer) {
        focusplayer = yourplayer;
	}
}
