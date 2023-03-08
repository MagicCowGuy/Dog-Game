using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Collections;

public class cameraControl : MonoBehaviour
{
	private Vector3 targetPos;
	private Quaternion targetrotation;
	//for shake
	private Vector3 offsetCamera;
	private Vector3 velocity = Vector3.zero;
  
	public Camera mCam;
	private bool twoTouch;
	private Vector2 touchInitPos;
	private Vector3 touchOffset;

	private Vector3 zoomOffset;
	private float zoomValue;
	private float zoomMax = 18;
	private float zoomMin = -15;
	private float zoomEffect = 1;

	private GameObject focusObj;
	private GameObject splitFocusObj;
	private float splitDistance;
	public GameObject playerObj;
	private Vector3 focusPos;

	private TimeControl timeConScript;

	// Start is called before the first frame update
	void Start()
	{
		Application.targetFrameRate = 60;
		//mCam = Camera.main;
		//watchPlayer();
		timeConScript = this.GetComponent<TimeControl>();
		StartCoroutine("zoomWatch");
	}

	// Update is called once per frame
	void Update()
	{
		if(focusObj != null){
			if(splitFocusObj == null){
				if(focusObj == playerObj){
					focusPos = focusObj.transform.position;
					focusPos.x = focusPos.x * 0.75f;
				} else {
					focusPos = focusObj.transform.position;
				}
			} else {
				splitDistance = Vector3.Distance(splitFocusObj.transform.position, focusObj.transform.position);

				offsetCamera = new Vector3(0 ,splitDistance + 9, -splitDistance - 8);
				focusPos = Vector3.Lerp(splitFocusObj.transform.position, focusObj.transform.position, 0.5f);

				
			}
		}
		
		targetPos = focusPos + offsetCamera + (zoomOffset * zoomEffect) + touchOffset;

		mCam.transform.position = Vector3.SmoothDamp(mCam.transform.position, targetPos, ref velocity , 20 * Time.unscaledDeltaTime);
		mCam.transform.rotation = Quaternion.Slerp(mCam.transform.rotation, targetrotation, (4 - Mathf.Clamp(Vector3.Distance(mCam.transform.position, targetPos)/20, 0, 3.5f)) * Time.unscaledDeltaTime);
	}

	public IEnumerator tempFocus(GameObject tmpFocusObj, float timeToFocus) {

		focusObj = tmpFocusObj;
		splitFocusObj = null;
		offsetCamera = new Vector3(0,6,-6);
		zoomEffect = 0;
		targetrotation = Quaternion.Euler (45, 0, 0);
		
		yield return new WaitForSecondsRealtime(1.5f);
		timeConScript.setTimeScale(0.1f);
		
		yield return new WaitForSecondsRealtime(timeToFocus);

		timeConScript.setTimeScale(1);
		watchPlayer();

		yield break;
	}

	public void watchPlayer(){
		focusObj = playerObj;
		splitFocusObj = null;
		//objLocked = true;
		//offsetCamera = new Vector3(0,30,-32);
		//targetrotation = Quaternion.Euler (45, 0, 0);
		offsetCamera = new Vector3(0,35,-32);
		targetrotation = Quaternion.Euler (45, 0, 0);
		zoomEffect = 1;
		//StartCoroutine("smoothCamChange", 1.5f);
	}

	public void watchObj(GameObject objToWatch){
		focusObj = objToWatch;
		splitFocusObj = null;
		offsetCamera = new Vector3(0,26,-25);
		if(objToWatch.CompareTag ("Interactable")){
			offsetCamera += objToWatch.GetComponent<Interactable>().camOffset;
		}
		zoomEffect = 1;
	}

	public void watchConvo(GameObject objToWatch1, GameObject objToWatch2){
		focusObj = objToWatch1;
		splitFocusObj = objToWatch2;
		offsetCamera = new Vector3(0,0,-20);
		zoomEffect = 0;
		//print("ANGLE BETWEEN CONVO IS " + (objToWatch1.transform.position.z - objToWatch2.transform.position.z));
	}

	public void buildMode(){
		focusObj = null;
		splitFocusObj = null;
		focusPos = new Vector3 (1.6f, 0, 0);
		offsetCamera = new Vector3(0,43.5f,-52);
		targetrotation = Quaternion.Euler (50, 0, 0);
		zoomEffect = 0;
	}

	public void menuMode(){
		focusObj = null;
		splitFocusObj = null;
		focusPos = new Vector3 (0, 0, 0);
		offsetCamera = new Vector3(0,5,-95);
		targetrotation = Quaternion.Euler (-5.5f, 0, 0);
		zoomEffect = 0;
	}

	IEnumerator zoomWatch(){
		while(true){
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

	  		zoomOffset = new Vector3(0, zoomValue * 1.2f, -zoomValue);
			yield return null;
		}
	  //yield return null;
	}

	void zoom(float increment){
		if(zoomEffect == 1){
		    zoomValue = Mathf.Clamp(zoomValue - (increment * 20), zoomMin, zoomMax);
		}
	}

}
