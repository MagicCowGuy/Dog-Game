using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmallTalk_Control : MonoBehaviour
{

    private Queue<string> lineQue;
    public GameObject smlTlkPrefab;
    public Canvas mainCanvas;

    public bool convoActive;

    public GameObject testSpeaker;
    public string[] testString;
    public SmallTalkThread testSmallTalk;
    private GameObject[] smallTalkPanels;
    //public SmallTalkThread[] smallTalksToSend;

    // Start is called before the first frame update
    void Start()
    {
      //startMonoThread(testSpeaker,testSmallTalk);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void startConvo(GameObject[] members, SmallTalkThread stThread){

    }

    public void shutDownST(){
      smallTalkPanels =  GameObject.FindGameObjectsWithTag ("SmallTalk");

     for(var i = 0 ; i < smallTalkPanels.Length ; i ++)
         Destroy(smallTalkPanels[i]);
    }

    public void startMonoThread(GameObject speaker, SmallTalkThread stThread){
      //GameObject speakerHead = speaker.GetComponent<NPC>().headObject;
      //speaker.SendMessage("TalkTrigger");
      GameObject smltlkclone = Instantiate (smlTlkPrefab, Vector3.zero, Quaternion.identity);
      smltlkclone.GetComponent<SmalTalkBox>().setupMonoThread(speaker, mainCanvas, stThread);
    }

    public void addLineToQ(string lineToAdd){

    }
}
