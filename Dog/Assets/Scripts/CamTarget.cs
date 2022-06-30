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
  public int cameraMode = 3;

  public float zoomMax = 20;
  public float zoomMin = -10;
  private Vector3 zoomOffset;
  public float zoomValue;
  public bool twoTouch;
  private Vector2 touchInitPos;
  private Vector3 touchOffset;

	public void Start () {
    StartCoroutine("CheckTargetPos");
		targetpos = new Vector3 (22, 27, -22);
		targetfocuspos = new Vector3 (0, -6.5f, 0);

        Application.targetFrameRate = 60;
        transform.position = new Vector3(-0.36f, 20, -80);
        transform.rotation = Quaternion.Euler(8, 0, 0);
        targetpos = new Vector3 (-0.36f, 20, -80);

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

      zoom(Input.GetAxis("Mouse ScrollWheel") * 1.5f);

      if(Input.touchCount == 2){
        Touch touchZero = Input.GetTouch(0);
        Touch touchOne = Input.GetTouch(1);

        Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        float prevMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        float currentMag = (touchZero.position - touchOne.position).magnitude;

        float diffMag = currentMag - prevMag;

        zoom(diffMag * 0.005f);

        if(!twoTouch){
          twoTouch = true;
          touchInitPos = (touchZero.position + touchOne.position) / 2;
        }

        touchOffset = (touchInitPos - ((touchZero.position + touchOne.position) / 2)) * -0.025f;

      } else {
        twoTouch = false;
        touchInitPos = Vector2.zero;
        touchOffset = Vector3.zero;
      }

      zoomOffset = new Vector3(0, zoomValue, -zoomValue);
      //targetpos += zoomOffset;
      //targetpos += touchOffset;

      transform.position = Vector3.SmoothDamp(transform.position, targetpos + zoomOffset + touchOffset, ref velocity , 35 * Time.deltaTime) + offsetCamera;
      transform.rotation = Quaternion.Slerp(transform.rotation, targetrotation, 1.75f * Time.deltaTime);

    }


    void zoom(float increment){
     zoomValue = Mathf.Clamp(zoomValue - (increment * 20), zoomMin, zoomMax);
    }


    IEnumerator CheckTargetPos(){
      //Follow Player Mode
      while(true){
      if(cameraMode == 1){
      targetfocuspos = focusplayer.transform.position;
        //if(focusplayer != null)
        //{
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

          //}
        }

        //Overview of Front Yard Mode
        if(cameraMode == 2){
          targetpos = new Vector3 (1, 32, -40);
          targetrotation = Quaternion.Euler (45, 0, 0);
        }

        if(cameraMode == 3){
          targetpos = new Vector3 (-0.36f, 20, -80);
          targetrotation = Quaternion.Euler (8, 0, 0);
        }

        yield return new WaitForSeconds(0.1f);
      }
    }
    public void SetTarget (Transform yourplayer) {
        focusplayer = yourplayer;
	}

}
