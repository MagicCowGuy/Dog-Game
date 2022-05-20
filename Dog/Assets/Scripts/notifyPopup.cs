using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NoteToDisplay {
public string Headtxt;
public string Subheadtxt;

public NoteToDisplay (string headtxt, string subheadtxt){
	Headtxt = headtxt;
	Subheadtxt = subheadtxt;
}




}


public class notifyPopup : MonoBehaviour {
	public List<NoteToDisplay> ListOfNotes = new List<NoteToDisplay>();

	private Animator notifyAnim;
	public TextMeshProUGUI textHeading;
	public TextMeshProUGUI textSubHeading;

	void Start () {
		notifyAnim = this.GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update () {
		if(ListOfNotes.Count > 0) {
			textHeading.text = ListOfNotes[0].Headtxt;
			textSubHeading.text = ListOfNotes[0].Subheadtxt;
			Notify();
			ListOfNotes.RemoveAt(0);
		}
	}

	//Add Notification to list
	public void AddNotify (string notetype, string notename) {
		print(notetype + ": " + notename);
		//dictionary.Add("Tom", "BERE");
		ListOfNotes.Add(new NoteToDisplay(notetype, notename));
	}

	public void Notify () {
		notifyAnim.SetTrigger("TriggerPopup");
	}
}
