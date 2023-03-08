using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TaskMenuEffect : MonoBehaviour
{

    public RectTransform menuContent;
    public RectTransform titleTexture;
    public RectTransform handlePan;
    public RectTransform handleJoinPan;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        titleTexture.anchoredPosition = new Vector2(-10,-menuContent.position.y+1000);
        handlePan.anchoredPosition = new Vector2(-500, Mathf.Sin(menuContent.position.y/40) * 20);
        handleJoinPan.anchoredPosition = new Vector2(-14, Mathf.Sin(menuContent.position.y/40) * 5);
    }

    void OnGui(){
        
    }
}
