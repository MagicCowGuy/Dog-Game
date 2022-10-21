using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class SmalTalkBox : MonoBehaviour
{
    public GameObject SpeakerObj;
    public GameObject SpeakerHeadObj;
    public Canvas canvas;
    private Vector3 NPCPosUI;
    private Vector3 PlayerPosUI;
    public Vector3 PosOffset;
    private RectTransform myRT;
    private Vector2 myRTsize;
    public SmallTalkThread stThread;
    public TextMeshProUGUI textLine;
    public ContentSizeFitter TMPContSizeFilt;
    private GameObject playerObj;

    public RectTransform pointerRect;

    public string[] monoThread;
    public SmallTalkThread smallTalks;
    private int monoThreadProg;

    private bool lockedOn;

    // Start is called before the first frame update
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
      if(GetComponent<VerticalLayoutGroup>().enabled == true){
        TrackSpeaker();
      }
    }

    public void setupMonoThread(GameObject speaker, Canvas mCanvas, SmallTalkThread stThread){
      playerObj = GameObject.FindWithTag("Player");
      transform.SetParent(mCanvas.transform);
      transform.SetAsFirstSibling();
      myRT = this.GetComponent<RectTransform>();
      myRT.localScale = Vector3.one;
      SpeakerObj = speaker;
      SpeakerHeadObj = speaker.GetComponent<NPC>().headObject;
      canvas = mCanvas;
      smallTalks = stThread;
      StartCoroutine("monoThreadCoRo");

      //NPCPosUI = worldToUISpace(canvas, SpeakerObj.transform.position);
      //myRT.position = NPCPosUI;
    }

    IEnumerator monoThreadCoRo(){
      monoThreadProg = 0;
      while(monoThreadProg < smallTalks.smallTalkThread.Length){
        textLine.SetText(smallTalks.smallTalkThread[monoThreadProg].diaLine);
        SpeakerObj.SendMessage("TalkTrigger");
        LayoutRebuilder.ForceRebuildLayoutImmediate(myRT);
        monoThreadProg ++;
        SetChatOffset();
        yield return new WaitForSeconds(1.5f);
      }
      Destroy(this.gameObject);
      yield break;
    }

    private void SetChatOffset() {
      NPCPosUI = worldToUISpace(canvas, SpeakerHeadObj.transform.position);
      PlayerPosUI = worldToUISpace(canvas, playerObj.transform.position);
      myRTsize = new Vector2(myRT.rect.width, myRT.rect.height);

      if(NPCPosUI.x < PlayerPosUI.x){
        myRT.pivot = new Vector2(1,0);
        PosOffset = new Vector3(-30,-25,0);
        pointerRect.anchorMax = new Vector2(1,0.5f);
        pointerRect.anchorMin = new Vector2(1,0.5f);
        pointerRect.anchoredPosition = new Vector2(-10,0);
        pointerRect.rotation = Quaternion.Euler(0,0,90);
      } else {
        myRT.pivot = new Vector2(0,0);
        PosOffset = new Vector3(30,-25,0);
        pointerRect.anchorMax = new Vector2(0,0.5f);
        pointerRect.anchorMin = new Vector2(0,0.5f);
        pointerRect.anchoredPosition = new Vector2(10,0);
        pointerRect.rotation = Quaternion.Euler(0,0,-90);
      }
      //pointerRect.localPosition = Vector3.zero;
      LayoutRebuilder.ForceRebuildLayoutImmediate(myRT);

      //pointerRect.rotation = Quaternion.LookRotation(Vector3.forward, pointerRect.position - NPCPosUI);

    }

    private void TrackSpeaker() {
      //get position of NPC on screen
      NPCPosUI = worldToUISpace(canvas, SpeakerHeadObj.transform.position);

      //apply position to SmallTextBox
      myRT.position = NPCPosUI + (PosOffset * canvas.GetComponent<RectTransform>().localScale.x);

    }

    public Vector3 worldToUISpace(Canvas parentCanvas, Vector3 worldPos)
    {
    	    //Convert the world for screen point so that it can be used with ScreenPointToLocalPointInRectangle function
    	    Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
    	    Vector2 movePos;
    	    //Convert the screenpoint to ui rectangle local point
    	    RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, screenPos, parentCanvas.worldCamera, out movePos);
    	    //Convert the local point to world point
    	    return parentCanvas.transform.TransformPoint(movePos);
    }
}
