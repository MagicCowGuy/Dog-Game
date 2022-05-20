using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class placementLocation : MonoBehaviour {

	public int sizeCode;
	public int saveArrayCode;
	public bool occupiedSpace;
	public bool highlighted;
	public Collider clickzone;

	public GameObject AreaShadowObj;
	public GameObject AreaHighlightObj;

	public rotatePulse outlineScript;
	public rotatePulse fillScript;

	public GameObject placedObject;

	void Awake () {
		clickzone = GetComponent<Collider>();
		unhighlightArea();
	}

	public void highlightArea() {
	AreaHighlightObj.GetComponent<SpriteRenderer>().enabled = true;
		//ProjectorHighlightObj.GetComponent<Projector>().enabled = true;
		highlighted = true;
		outlineScript.pulsing = true;
		outlineScript.spinning = true;
		//fillScript.pulsing = true;
		//clickzone.enabled = true;
	}

	public void unhighlightArea() {
		highlighted = false;
		outlineScript.pulsing = false;
		outlineScript.spinning = false;
		//fillScript.pulsing = false;
		//clickzone.enabled = false;
		AreaHighlightObj.GetComponent<SpriteRenderer>().enabled = false;
		//ProjectorHighlightObj.GetComponent<Projector>().enabled = false;
	}

	public void selectArea(GameObject prefabItem, int prefabCode) {
		placedObject = Instantiate (prefabItem, transform.position, Quaternion.Euler (0, 0, 0));
		saveArrayCode = prefabCode;
		occupiedSpace = true;
	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
}
