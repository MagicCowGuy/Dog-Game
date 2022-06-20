using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC_Tracker : MonoBehaviour
{
  public Canvas canvas;
  public Vector3 NPCPosUI;
  private Vector3 ClampNPCPosUI;
  public RectTransform NPCIconImage;
  public RectTransform PointerImage;
  public Vector2 ScreenSize;
  private int UIborder = 125;
  public bool showTracker;
  public Transform trackedTransform;
  //private float pointerRot;

    // Start is called before the first frame update
  void Start()
  {
    ScreenSize = new Vector2(Screen.width, Screen.height);
    print(ScreenSize);
  }

    // Update is called once per frame
  void Update()
  {
        TrackNPC();
  }

  private void TrackNPC() {
    //get position of NPC on screen
    NPCPosUI = worldToUISpace(canvas, trackedTransform.position);

    //is the NPC off the screen and therefore needs to be tracked?
    if(NPCPosUI.y > 0 && NPCPosUI.y < ScreenSize.y && NPCPosUI.x > 0 && NPCPosUI.x < ScreenSize.x){
      showTracker = false;
      NPCIconImage.GetComponent<Image>().enabled = false;
      PointerImage.GetComponent<Image>().enabled = false;
      return;
    } else {
      showTracker = true;
      NPCIconImage.GetComponent<Image>().enabled = true;
      PointerImage.GetComponent<Image>().enabled = true;
      PointToNPC();
    }

    //set position of icon to track NPC off screen.
		ClampNPCPosUI = new Vector3(Mathf.Clamp(NPCPosUI.x,UIborder,ScreenSize.x-UIborder), Mathf.Clamp(NPCPosUI.y,UIborder,ScreenSize.y-UIborder), 0);
		NPCIconImage.GetComponent<RectTransform>().position = ClampNPCPosUI;

	}

  private void PointToNPC(){
    PointerImage.GetComponent<RectTransform>().rotation = Quaternion.LookRotation(Vector3.forward, ClampNPCPosUI - NPCPosUI);
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
