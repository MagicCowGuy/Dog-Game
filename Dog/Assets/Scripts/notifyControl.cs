using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class NoteToDisplay {
	public int noteType;
	public string Headtxt;
	public string Subheadtxt;
	public Sprite noteIcon;

	public NoteToDisplay (int notetype, string headtxt, string subheadtxt, Sprite noteicon = null){
		noteType = notetype;
		Headtxt = headtxt;
		Subheadtxt = subheadtxt;
		noteIcon = noteicon;
	}
}


public class notifyControl : MonoBehaviour {
	public List<NoteToDisplay> ListOfNotes = new List<NoteToDisplay>();

	public NoteToDisplay curTutorialNote;
	public NoteToDisplay curNoteNote;

	public Animator notifyAnim;
	public RectTransform notifyPanel;
	public RectTransform textLayoutPanel;
	public TextMeshProUGUI textHeading;
	public TextMeshProUGUI textSubHeading;

	public bool displaying = false;

	public GameObject BGInstruct;
	public GameObject BGAchive;
	public GameObject BGTask;
	public Animator BGAnim;

	public Image noteImage;
	
	public GameObject confettiCameraObj;
	
	//private Vector2 panPos;
	private float panYPos;
	private float panYTar;
	private float cooldownNote;

	private int hiddenYPos = 15;
	
	void Start () {
	//	notifyAnim = this.GetComponent<Animator>();
	StartCoroutine("NoteCheck");
	}

	// Update is called once per frame
	void Update () {
		
		if(displaying){
			panYPos = Mathf.MoveTowards(panYPos, panYTar, 500 * Time.deltaTime);
			notifyPanel.anchoredPosition = new Vector2(0,panYPos);
		
			if(panYTar == hiddenYPos && panYPos >= hiddenYPos){
				displaying = false;
				BGInstruct.SetActive(false);
				BGAchive.SetActive(false);
				BGTask.SetActive(false);
			}
		}
	}

	//Add Notification to list
	public void AddNotify (NoteToDisplay newNote) {
		//print(notetype + ": " + notename);
		//dictionary.Add("Tom", "BERE");
		ListOfNotes.Add(newNote);
	}

	public void InstructNote (NoteToDisplay newInstruct) {
		curTutorialNote = newInstruct;
		//notify();
	}

	public void InstructClear (){
		curTutorialNote = null;
		Hide();
	}

	public void notify (NoteToDisplay dispNote) {
		curNoteNote = dispNote;
		//Achivement notification
		if(dispNote.noteType == 1){
			BGAchive.SetActive(true);
			confettiCameraObj.GetComponent<ParticleSystem>().Play();
			BGAnim.SetTrigger("Baloons");
		}
		
		if(dispNote.noteType == 2){
			BGTask.SetActive(true);
			noteImage.sprite = dispNote.noteIcon;
		}

		if(dispNote.noteType == 3){
			BGInstruct.SetActive(true);
		} else {
			cooldownNote = 10;
		}
		textHeading.text = dispNote.Headtxt;
		if(dispNote.Subheadtxt != null) {
			textSubHeading.enabled = true;
			textSubHeading.text = dispNote.Subheadtxt;
		} else {
			textSubHeading.enabled = false;
		}
		
		LayoutRebuilder.ForceRebuildLayoutImmediate(textLayoutPanel);
		LayoutRebuilder.ForceRebuildLayoutImmediate(notifyPanel);

		panYTar = 30 -notifyPanel.rect.height;
		displaying = true;
	}

	public void Hide (){
		panYTar = hiddenYPos;
	}

	public IEnumerator NoteCheck() {
		while(true){
			if(displaying){
				if(curNoteNote.noteType != 3){
					cooldownNote -= 1;
					if(cooldownNote <= 0){
						Hide();
					}
				}
				
			} else {
				if(curTutorialNote != null){
					notify(curTutorialNote);
				}
				if(curTutorialNote == null && ListOfNotes.Count > 0) {
					notify(ListOfNotes[0]);
					ListOfNotes.RemoveAt(0);
				}
			}
			yield return new WaitForSeconds(0.5f);
		}
		yield break;
	}
}
