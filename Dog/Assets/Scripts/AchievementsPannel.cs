using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class taskPannel : MonoBehaviour    , IPointerClickHandler // 2
 {

   private bool expanded = false;
   private RectTransform rt;
	// Use this for initialization
	void Awake () {
    rt = GetComponent<RectTransform>();
    Debug.Log(rt.anchoredPosition);
	}

	public void OnPointerClick(PointerEventData eventData) // 3
	     {
				 Debug.Log("Clicked! ");
         expanded = !expanded;
			 }

	// Update is called once per frame
	void Update () {

    if(expanded){
      rt.anchoredPosition = Vector2.Lerp(rt.anchoredPosition, new Vector2(-100,0), Time.deltaTime * 12);
    } else{
      rt.anchoredPosition = Vector2.Lerp(rt.anchoredPosition, new Vector2(-400,0), Time.deltaTime * 12);
    }

	}
}
