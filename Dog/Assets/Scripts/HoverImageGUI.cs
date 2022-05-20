using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class HoverImageGUI : MonoBehaviour {

	public bool isOver = false;
	public GameObject image;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void OnGUI () {
		//EventSystem e = Event.current;
				
		if (EventSystem.current.IsPointerOverGameObject()){
			if (!isOver) {
				isOver = true;
				GetComponent<Image> ().color = new Color32 (255, 255, 225, 100);
			}
		} else {
			if (isOver) {
				isOver = false;
				GetComponent<Image> ().color = new Color32 (255, 255, 225, 50);
			}
									
		}
	}
}
