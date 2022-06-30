using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_EDITOR

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

	public Vector3[] spawnPoints;

	public bool holdforplayer = false;

	public int curProgChapter;
	public int curProgSubChapter;
	public DialogueThread curDialogue;
	public SmallTalkThread curSmallTalk;
	public NPC_Progression progStat;

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
		progStat = this.GetComponent<NPC_Progression>();
		//later replace with load from save file.
		ProgressUpdate(0,0);
	}

	public void ProgressUpdate(int newChap, int newSubCap){
		curProgChapter = newChap;
		curProgSubChapter = newSubCap;
		curDialogue = progStat.progressionArray[newChap].progressStatus[newSubCap].diaThread;

		if(progStat.progressionArray[newChap].progressStatus[newSubCap].stThread != null){
			curSmallTalk = progStat.progressionArray[newChap].progressStatus[newSubCap].stThread;
		}

		gameObject.SendMessage("updateWatcher");
	}

	public void SpawnSetup(){
		//Vector3[] posSpawnPoints = newNPC.GetComponent<NPC>().spawnPoints;
    int spawnPointRef = Random.Range(0, spawnPoints.Length);
    transform.position = spawnPoints[spawnPointRef];
		transform.GetComponent<NavMeshAgent>().Warp(transform.position);
		gameObject.SendMessage("SetupNPC");
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
		yield return new WaitForSeconds(0.5f);
	//	dm.StartDialogue(dialogue, NPCImage, this.gameObject);
		dm.StartDialogue(curDialogue, NPCImage, this.gameObject);
	}

	public void ReleaseNPC(){
		print("releasing NPC");
		holdforplayer = false;
		gameObject.SendMessage("ResumeOrderNPC");
	}

	public void MoveToPos(Vector3 destination, float delayMove){

	}

}



#if UNITY_EDITOR
[CustomEditor(typeof(NPC))]
public class NPC_Editor : Editor
{

	public void OnSceneGUI()
	{
		var LinkScriptNPC = target as NPC;
		drawPoints(LinkScriptNPC.spawnPoints);

	}

	public void drawPoints(Vector3[] pointsToDraw){
		for(int i = 0; i < pointsToDraw.Length; i++){
			EditorGUI.BeginChangeCheck();
			Vector3 newPoint = Handles.PositionHandle(pointsToDraw[i], Quaternion.identity);
			if (EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(target, "Update Spawn Point");
				pointsToDraw[i] = newPoint;
			}
		}

	}

}
#endif // UNITY_EDITOR
