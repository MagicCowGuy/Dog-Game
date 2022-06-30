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

    // Start is called before the first frame update
    void Start()
    {
      startMonoThread(testSpeaker,testString);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void startConvo(GameObject[] members, SmallTalkThread stThread){

    }

    public void startMonoThread(GameObject speaker, string[] stStrings){
      GameObject smltlkclone = Instantiate (smlTlkPrefab, Vector3.zero, Quaternion.identity);
      smltlkclone.transform.SetParent(mainCanvas.transform);
      //smltlkclone.GetComponent<SmalTalkBox>().canvas = mainCanvas;
      smltlkclone.GetComponent<SmalTalkBox>().setupMonoThread(speaker, mainCanvas, stStrings);
      //smltlkclone.GetComponent<RectTransform>().localScale = Vector3.one;
      //smltlkclone.GetComponent<SmalTalkBox>().SpeakerObj = speaker;

      //smltlkclone.GetComponent<SmalTalkBox>().monoThread = stStrings;
    }

    public void addLineToQ(string lineToAdd){

    }
}
