using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

	public GameObject dialoguePannel;
	public expandMenu menuPannel;
	public GameObject blackoutPannel;
	public GameObject QuestCardsPannel;
	public QuestControl QuestControlScript;
	public TaskControl taskControlScript;

	public TextMeshProUGUI textName;
	public TextMeshProUGUI textDialogue;
	public Image playerchatimage;
	public Image NPCchatimage;
	public NPC currentNPC;
	private bool opendialogue = false;

	public GameObject QuestCardPrefab;
	public DialogueThread curDiaThread;

	Queue<string> sentences;
	Queue<string> names;

	// Use this for initialization
	void Start () {
		sentences = new Queue<string>();
		names = new Queue<string>();
	}

	void Update () {
		if (Input.GetMouseButtonDown(0) && opendialogue)
			{
		    DisplayNextSentence();
			}
	}

	public void StartDialogue (DialogueThread dialogue, Sprite convoImage, GameObject NPCobj){
		curDiaThread = dialogue;
		menuPannel.menuHide();
		dialoguePannel.SetActive(true);
		opendialogue = true;
		blackoutPannel.SetActive(true);
		currentNPC = NPCobj.GetComponent<NPC>();
		this.GetComponent<SmallTalk_Control>().shutDownST();
		//Debug.Log("Starting conversation");
		NPCchatimage.sprite = convoImage;
		//textName.text = dialogue.name;
		sentences.Clear();
		names.Clear();

		foreach (DialogueLine dLine in dialogue.dialogueThread) {
			sentences.Enqueue(dLine.diaLine);
			//print("Adding line: " + dLine.diaLine);
			names.Enqueue(dLine.diaSpeaker.ToString());
		}

//		foreach (string name in dialogue.names) {
//			names.Enqueue(name);
//		}

		//DisplayNextName();
		DisplayNextSentence();
	}

	public void DisplayNextSentence (){
		if (sentences.Count == 0){
			EndDialogue();
			if(curDiaThread.TaskUnlock.Length > 0){
				taskControlScript.showTaskUnlock(curDiaThread.TaskUnlock[0]);
			}
			if(curDiaThread.QuestToOffer.Length > 0){
//			if(currentNPC.QuestToOffer.Length > 0){
				QuestControlScript.OfferQuests();
				PopulateQuestList();
			}
			if(curDiaThread.TaskUnlock.Length == 0 && curDiaThread.QuestToOffer.Length == 0){
				blackoutPannel.SetActive(false);
				menuPannel.menuShow();
				//NPC.GetComponent<NPC>().ReleaseNPC();
				currentNPC.GetComponent<NPC>().ReleaseNPC();
			}
			return;
		}

		string name = names.Dequeue();
		string sentence = sentences.Dequeue();
		StopAllCoroutines();
		textName.text = name;
		if (name == "You"){
			playerchatimage.color = Color.white;
			NPCchatimage.color = new Color(0.45f, 0.45f, 0.45f, 1);
		} else {
			NPCchatimage.color = Color.white;
			playerchatimage.color = new Color(0.45f, 0.45f, 0.45f, 1);
		}
		StartCoroutine(TypeSentence(sentence));
	}

	IEnumerator TypeSentence (string sentence){
		textDialogue.text = "";
		foreach (char letter in sentence.ToCharArray()){
			textDialogue.text += letter;
			yield return null;
		}
	}

	void EndDialogue() {
		Debug.Log("End of conversation");
		dialoguePannel.SetActive(false);
		opendialogue = false;
		//blackoutPannel.SetActive(false);
	}

	public void ReleaseCurrentNPC(){
		currentNPC.GetComponent<NPC>().ReleaseNPC();

		currentNPC = null;
	}

	void PopulateQuestList() {
		//first clear out QuestCardsPannel
		foreach (Transform child in QuestCardsPannel.transform){
			GameObject.Destroy(child.gameObject);
		}
		//then from the list on NPC make all the new cards
		float offsetcard = 0;
		int cardcount = currentNPC.QuestToOffer.Length;
		int cardSpacing = 280;

		foreach (Quest quest in curDiaThread.QuestToOffer){
			print(quest.name);
			GameObject displayQuestCard = Instantiate (QuestCardPrefab, QuestCardsPannel.transform);
			displayQuestCard.GetComponent<QuestCardSetup>().SetupCard(quest,0);
			displayQuestCard.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetcard - (((cardcount - 1) * cardSpacing)/2), 0);
			offsetcard += cardSpacing;
		}
	}

}
