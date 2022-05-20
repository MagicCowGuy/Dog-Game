using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GUInotification : MonoBehaviour {


	public Canvas NotificationCanvas;
	public bool active = false;
	public Vector3 startpos = new Vector3(0,125,0);
	public Vector3 showpos = new Vector3(0,-125,0);
	private Vector3 notipos;
	private float showtimer;

	public TextMeshProUGUI NotifyHeading;
	public TextMeshProUGUI NotifyDescription;

	// Use this for initialization
	void Start () {
		active = false;
		notipos = startpos;
	}
	
	// Update is called once per frame
	void Update () {

		if (active == true) {
			notipos = Vector3.Lerp (notipos, showpos, Time.deltaTime * 7);
			showtimer += Time.deltaTime;
		} else {
			notipos = Vector3.Lerp (notipos, startpos, Time.deltaTime * 7);
		}

		RectTransform myRectTransform = NotificationCanvas.GetComponent<RectTransform> ();
		myRectTransform.anchoredPosition = notipos;

		if (showtimer > 2f) {
			active = false;
			showtimer = 0;
		}

	}

	public void Notification (string notihead, string notidescript){
		print ("Notification Time!");
		NotificationCanvas.enabled = true;
		active = true;
		NotifyHeading.text = notihead;
		NotifyDescription.text = notidescript;
	}
}
