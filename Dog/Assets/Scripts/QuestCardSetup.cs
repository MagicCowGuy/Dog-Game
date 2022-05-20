using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestCardSetup : MonoBehaviour
{
    public QuestControl QuestControlScript;

    public RectTransform rtobject;

    private float rotFlo;
    public float rdmOffset;
    private Vector3 posVect;

    public bool isOfferCard;
    public bool isSelected;
    private bool isToLog;
    private bool isDiscarded;

    private float selectCooldown;
    private float targetScale;
    private float oldScale;
    private float tParam = 0;
    private Vector2 targetLocation = new Vector2(475,290);
    private Vector2 oldLocation;

    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    public TextMeshProUGUI rewardAmount;
    public Image darkenCardImage;

    public Image questImage;

    public int questSaveCode;
    public int questColCode;
    public Quest questprefab;

    public GameObject progressPannel;
    public TextMeshProUGUI progressText;

    public GameObject completePannel;
    public TextMeshProUGUI completeText;

    public void SetupCard(Quest questlink, int questCollectionCode){
      title.text = questlink.name;
      description.text = questlink.description;
      rewardAmount.text = questlink.rewardAmount.ToString();
      questImage.sprite = questlink.artwork;
      questSaveCode = questlink.SaveCode;
      questColCode = questCollectionCode;
      questprefab = questlink;
      targetScale = 1;
      oldScale = 1;
      QuestControlScript = GameObject.Find("GameManagment").GetComponent<QuestControl>();
    }

    public void SetupProgressCard(int Progress){
      progressPannel.SetActive(true);
      progressText.text = Progress + "/" + questprefab.targetAmount;
    }

    public void UpdateProgress(){
      progressText.text = questprefab.progressAmount + "/" + questprefab.targetAmount;
    }

    public void Awake (){
      rdmOffset = Random.Range(0,100);
      floatyCard();
    }

    public void OnCardClick(){
      if(isSelected){
        CardToLog();
        QuestControlScript.AddQuest(questprefab);
      }else{
        SelectCard();
      }
    }

    public void SelectCard(){
      isSelected = true;
      targetScale = 1.25f;
      oldScale = rtobject.localScale.x;
      tParam = 0;
      darkenCardImage.enabled = false;

      print(transform.parent.name);
      foreach(Transform otherQuest in this.transform.parent){
        if (otherQuest != this.transform) {
          otherQuest.gameObject.GetComponent<QuestCardSetup>().DeselectCard();
        }
      }
    }

    public void DeselectCard(){
      isSelected = false;
      targetScale = 1;
      oldScale = rtobject.localScale.x;
      tParam = 0;
      darkenCardImage.enabled = true;
    }

    public void CardToLog(){
      isToLog = true;
      oldLocation = rtobject.anchoredPosition;
      targetScale = 0.1f;
      oldScale = rtobject.localScale.x;
      tParam = 0;
      transform.SetAsLastSibling();

      foreach(Transform otherQuest in this.transform.parent){
        if (otherQuest != this.transform) {
          otherQuest.gameObject.GetComponent<QuestCardSetup>().DiscardCard();
        }
      }
    }

    public void DiscardCard(){
      isDiscarded = true;
      oldLocation = rtobject.anchoredPosition;
      targetScale = 1;
      oldScale = rtobject.localScale.x;
      tParam = 0;
      targetLocation = new Vector2(oldLocation.x,-500);
    }

    public void CompletedCard(){

      completePannel.SetActive(true);
      GetComponent<Button>().enabled = true;
    }

    public void ClickCompleted(){
      print("tell questcontrol to remove this card and add currency");
      //tell questcontrol to remove this card and add currency
      QuestControlScript.QuestCompleted(questColCode);
    }

    void Update(){
      if(isOfferCard){
        floatyCard();
      }

      if(tParam < 1.25f){
        float lerpScale = Mathf.SmoothStep(oldScale,targetScale,tParam);

        if(isDiscarded){
          float smoothMove = Mathf.SmoothStep(oldLocation.y,targetLocation.y,tParam);
          rtobject.anchoredPosition = new Vector2(oldLocation.x,smoothMove);
          //rtobject.anchoredPosition = new Vector2(0,0);
          //print(this.name + " IS DISICHGHRESF");
        }
        if(isToLog){
          rtobject.anchoredPosition = Vector2.Lerp(oldLocation,targetLocation,tParam);
          //y = cos(x * 2.27 - 0.55) + 0.15
          lerpScale = Mathf.Clamp(Mathf.Cos(tParam * 2.17f - 0.59f) * 1.5f, 0, 2);
        }

        if(isDiscarded || isToLog){
          tParam += 0.04f;
        } else {
          tParam += 0.1f;
        }

        rtobject.localScale = new Vector3(lerpScale,lerpScale,0);
      }

      if(tParam >= 1.25f && isToLog){
          QuestControlScript.DeclineQuests();
      }

    }

    private void floatyCard() {
      rotFlo = (Mathf.Sin(Time.time + rdmOffset/20)) * (1+ rdmOffset / 50);
      rtobject.rotation = Quaternion.Euler(0,0,rotFlo);
      //rtobject.pivot = new Vector2(Mathf.Sin(Time.time + rdmOffset/60)/5,Mathf.Cos(Time.time + rdmOffset/60)/5 + 0.5f);
      rtobject.pivot = new Vector2(Mathf.Sin(Time.time + rdmOffset)/100 + 0.5f,Mathf.Cos(Time.time + rdmOffset)/100 + 0.5f);
    }

}
