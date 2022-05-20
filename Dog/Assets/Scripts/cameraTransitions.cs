using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraTransitions : MonoBehaviour
{

    public GameObject maskObj;
    public GameObject blackoutObj;
    public GameObject playerObj;
    public RectTransform CanvasRect;
    public Camera Cam;

    private Vector2 ViewportPosition;
    private Vector2 playerUIPos;
    private RectTransform maskRT;

    private bool expanding;
    private bool contracting;
    private bool animating;

    private float IrisSize;

    // Start is called before the first frame update
    void Start()
    {
      maskRT = maskObj.GetComponent<RectTransform>();
      blackoutObj.SetActive(true);

      StartCoroutine(WaitforStartCoroutine());
    }

    IEnumerator WaitforStartCoroutine()
    {
      yield return new WaitForSeconds(2);
      OpenIris();
    }

    void OpenIris()
    {
      maskObj.SetActive(true);

      expanding = true;
      contracting = true;
      animating = true;
      IrisSize = 30;
    }

    void CloseIris()
    {
      blackoutObj.SetActive(true);
      maskObj.SetActive(true);

    }

    void DisableIris()
    {
      animating = false;
      expanding = false;
      contracting = false;
      blackoutObj.SetActive(false);
      maskObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
      if(animating){
        LockOnPlayer();
        if(expanding){
          IrisSize *= 1.085f;
          maskRT.sizeDelta = new Vector2(IrisSize,IrisSize);
          if(IrisSize > 5000){
            DisableIris();
          }
        }
      }
    }

    void LockOnPlayer (){
      ViewportPosition = Cam.WorldToViewportPoint(playerObj.transform.position);
      playerUIPos = new Vector2(
        ((ViewportPosition.x*CanvasRect.sizeDelta.x)-(CanvasRect.sizeDelta.x*0.5f)),
        ((ViewportPosition.y*CanvasRect.sizeDelta.y)-(CanvasRect.sizeDelta.y*0.5f)));
      maskRT.anchoredPosition = playerUIPos;
    }
}
