using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class expandMenu : MonoBehaviour
{
    public GameObject uiexpander;
    private RectTransform rt;
    private float menuTargetSize;
    private float menuSize;
    public bool expanding;
    public bool expanded;
    public bool hidden;
    private int selectedItem;

    public RectTransform selectCircleObj;
    private Vector2 selecttargetpos;

    public BuildControl buildcontrolscript;
    public AchievementsControl achievementscontrolscript;
    public QuestControl questcontrolscript;
    public storeControl storeControlScript;
    public GameManager gamemanagerscript;
    public TaskControl taskControlScript;

    public GameObject[] menuitem;

    public int smallestSize = 0;
    public int biggestSize = 650;
    public float targetAlpha;
    private float menuAlpha;
    public Image image;

    // Start is called before the first frame update
    void Start()
    {
      selectedItem = -1;
      menuTargetSize = 80;
      menuSize = 80;
      menuAlpha = 0;
      targetAlpha = 0.25f;
      rt = uiexpander.GetComponent<RectTransform>();

      image = GetComponent<Image>();
      menuClose();
    }

    // Update is called once per frame
    void Update()
    {

      menuSize = Mathf.Lerp(menuSize,menuTargetSize,0.4f);
      menuAlpha = Mathf.Lerp(menuAlpha,targetAlpha,0.4f);

      var tempColor = image.color;
      tempColor.a = menuAlpha;
      image.color = tempColor;

      rt.sizeDelta = new Vector2( menuSize, menuSize);
      //rt.GetComponent<Image>().color.a = menuAlpha;
      selectCircleObj.position = selecttargetpos;
    }

    public void menuExpand(){
      expanding = true;
      menuTargetSize = biggestSize;
      targetAlpha = 0.35f;
    }

    public void menuClose(){
      expanding = false;
      menuTargetSize = 80;
      targetAlpha = 0.25f;
      if(selectedItem == 0){
          print("Build Mode!");
          buildcontrolscript.ActivateBuildMode();
      }
      if(selectedItem == 1){
          achievementscontrolscript.ShowAchievementsUI();
      }
      if(selectedItem == 2){
          storeControlScript.ShowStoreUI();
      }
      if(selectedItem == 3){
          gamemanagerscript.ShowMenu();
      }
      if(selectedItem == 4){
          taskControlScript.OpenTaskList();
      }
    }

    public void menuHide(){
      hidden = true;
      expanding = false;
      menuTargetSize = 0;
    }

    public void menuShow(){
      hidden = false;
      menuTargetSize = 80;
    }

    public void itemselect(int itemToHighlight){
      //print("SELECTING: " + menuitem[itemToHighlight].name);
      selectedItem = itemToHighlight;
      selecttargetpos = menuitem[itemToHighlight].GetComponent<RectTransform>().position;
    }

    public void itemdeselect(int itemToLowlight){
      selectedItem = -1;
      selecttargetpos = new Vector2(0,0);
      //print("DESELECTING: " + menuitem[itemToLowlight].name);
    }

}
