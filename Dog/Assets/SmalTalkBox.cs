using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class SmalTalkBox : MonoBehaviour
{
    public GameObject SpeakerObj;
    public Canvas canvas;
    private Vector3 NPCPosUI;
    private RectTransform myRT;
    private Vector2 myRTsize;
    public SmallTalkThread stThread;
    public TextMeshProUGUI textLine;
    public ContentSizeFitter TMPContSizeFilt;

    public string[] monoThread;
    private int monoThreadProg;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
      if(GetComponent<VerticalLayoutGroup>().enabled == true){
        TrackSpeaker();
      }
    }

    public void setupMonoThread(GameObject speaker, Canvas mCanvas, string[] mtStrings){
      myRT = this.GetComponent<RectTransform>();
      myRT.localScale = Vector3.one;
      SpeakerObj = speaker;
      canvas = mCanvas;
      monoThread = mtStrings;
      StartCoroutine("monoThreadCoRo");
    }

    IEnumerator monoThreadCoRo(){
      monoThreadProg = 0;
      while(monoThreadProg <= monoThread.Length){
        textLine.SetText(monoThread[monoThreadProg]);
        monoThreadProg ++;
        //GetComponent<VerticalLayoutGroup>().enabled = false;
        //yield return new WaitForFixedUpdate();
        //GetComponent<VerticalLayoutGroup>().enabled = true;

        LayoutRebuilder.ForceRebuildLayoutImmediate(myRT);
        yield return new WaitForSeconds(2);
      }
      //Destroy(this.gameObject);
      yield break;
    }

    private void TrackSpeaker() {
      //get position of NPC on screen
      NPCPosUI = worldToUISpace(canvas, SpeakerObj.transform.position);

      myRTsize = new Vector2(myRT.rect.width, myRT.rect.height);
      NPCPosUI.y += 50;
      if(NPCPosUI.x < Screen.width / 2){
        NPCPosUI.x += myRTsize.x;
      } else {
        NPCPosUI.x -= myRTsize.x;
      }

      if(NPCPosUI.y < Screen.height * 0.8f){
        NPCPosUI.y += myRTsize.y;
      } else {
        NPCPosUI.y -= myRTsize.y;
      }

      //apply position to SmallTextBox
      myRT.position = Vector3.Lerp(myRT.position, NPCPosUI, 0.25f);
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
