using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {

	public int MobCodeNo;
	public Dialogue dialogue;
	public DialogueManager dm;
	public Quest[] QuestToOffer;
	public Sprite NPCImage;
	public GameObject QuestSprite;
	public GameObject QuestSpriteLockObject;
	private float rndoffset;
	private Vector3 posoffset;

	public GameObject headObject;
	public GameObject EmoteEffect;
	private SpriteRenderer m_SpriteRenderer;

	public GameObject gameControlObj;

	public bool holdforplayer = false;

	public void Start() {
		if(QuestSpriteLockObject == null){
			QuestSpriteLockObject = this.gameObject;
		}
		if(QuestToOffer.Length > 0){
			QuestSprite.GetComponent<SpriteRenderer>().enabled = true;
			posoffset = QuestSprite.transform.position - QuestSpriteLockObject.transform.position;
		}
		rndoffset = Random.Range (0.0f, 1.0f);
		dm = gameControlObj.GetComponent<DialogueManager>();
	}

	public void Update() {
		if (QuestSprite != null){
		//QuestSprite.transform.localPosition = new Vector3(QuestSprite.transform.localPosition.x, 2.75f + Mathf.Sin((Time.time + rndoffset) * 6 ) * 0.25f, QuestSprite.transform.localPosition.z);
		QuestSprite.transform.position = QuestSpriteLockObject.transform.position + posoffset + new Vector3(0, Mathf.Sin((Time.time + rndoffset) * 3 ) * 0.25f, 0);
		QuestSprite.transform.LookAt (Camera.main.transform.position);
		//ShadowObject.transform.eulerAngles = new Vector3(90, QuestSprite.transform.eulerAngles.y, 0);
		}

	}


	public void emoteChat (){
		holdforplayer = true;
		GameObject emoteclone = Instantiate (EmoteEffect, headObject.transform.position, Quaternion.LookRotation(Vector3.forward));
		emoteclone.transform.SetParent (headObject.transform);
		emoteclone.transform.localPosition = new Vector3 (0, 0.85f, 1.25f);
		if(headObject.transform.rotation.eulerAngles.y > 0 && headObject.transform.rotation.eulerAngles.y < 180){
			m_SpriteRenderer = emoteclone.GetComponent<SpriteRenderer>();
			m_SpriteRenderer.flipX = true;
		}
	}


	public void TriggerDialogue(){
		StartCoroutine(UIChatDialogue());
		emoteChat();
		gameObject.SendMessage("PauseOrderNPC");
	}
	IEnumerator UIChatDialogue(){
		yield return new WaitForSeconds(0.75f);
		dm.StartDialogue(dialogue, NPCImage, this.gameObject);
	}

	public void ReleaseNPC(){
		print("releasing NPC");
		holdforplayer = false;
		gameObject.SendMessage("ResumeOrderNPC");
	}

	public void MoveToPos(Vector3 destination, float delayMove){

	}

}
