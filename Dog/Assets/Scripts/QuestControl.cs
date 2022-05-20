using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestControl : MonoBehaviour
{
  public GameObject QuestOfferPannel;
  public GameObject QuestLogPannel;
  public GameObject blackoutPannel;
  public expandMenu menuPannel;
  public GameObject QuestCardsPannel;
  public GameObject QuestCardPrefab;
  public Quest[] QuestDatabase;
  private DialogueManager dioManager;

  public Quest[] QuestCollection;
  public GameObject[] QuestCardObjArray;
  //public bool[] QuestComplete;
	public int[] QuestProgressArray;
  public int[] QuestSaveCodeArray;

  private int maxQuestLimit = 3;

    // Start is called before the first frame update
    void Start()
    {
      dioManager = gameObject.GetComponent<DialogueManager>();
      CloseQuestLog();
      //THIS IS WHERE TO LOAD FROM PlayerPrefs
      LoadQuests();
      PopulateQuestLog();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SaveQuests(){
      for(int i = 0; i < QuestCollection.Length; i++){
        if(QuestCollection[i] != null){
          QuestSaveCodeArray[i] = QuestCollection[i].SaveCode;
        }
      }
      PlayerPrefsX.SetIntArray ("QuestCodes", QuestSaveCodeArray);
      PlayerPrefsX.SetIntArray ("QuestProgress", QuestProgressArray);
      PlayerPrefs.Save();
    }

    public void LoadQuests(){
      QuestSaveCodeArray = PlayerPrefsX.GetIntArray ("QuestCodes", 0, maxQuestLimit);
      QuestProgressArray = PlayerPrefsX.GetIntArray ("QuestProgress", 0, maxQuestLimit);

      for(int i = 0; i < QuestSaveCodeArray.Length; i++){
        if(QuestSaveCodeArray[i] != 0){
          QuestCollection[i] = QuestDatabase[QuestSaveCodeArray[i]-1];
        }
      }
      //QuestProgress(0,0);
    }

    public void OpenQuestLog(){
      menuPannel.menuHide();
  		QuestLogPannel.SetActive(true);
      blackoutPannel.SetActive(true);
    }

    public void CloseQuestLog(){
      menuPannel.menuShow();
  		QuestLogPannel.SetActive(false);
      blackoutPannel.SetActive(false);
    }

    public void OfferQuests(){
      QuestOfferPannel.SetActive(true);
    }

    public void DeclineQuests(){
      QuestOfferPannel.SetActive(false);
      blackoutPannel.SetActive(false);
      menuPannel.menuShow();
      dioManager.ReleaseCurrentNPC();
    }

    public void AddQuest(Quest QuestToAdd){
      int firstEmpty = System.Array.IndexOf (QuestCollection, null);
      if(firstEmpty != -1){
        QuestCollection[firstEmpty] = QuestToAdd;
        print("Adding quest called " + QuestToAdd.name);
        PopulateQuestLog();
        SaveQuests();
        dioManager.ReleaseCurrentNPC();
      } else {
        print("Quest Log Full, program in a replacement function");
      }
    }

    void PopulateQuestLog() {
      //first clear out QuestCardsPannel
  		foreach (Transform child in QuestCardsPannel.transform){
  			GameObject.Destroy(child.gameObject);
  		}
      //QuestCardObjArray.clear(QuestCardObjArray,0,QuestCardObjArray.length);
  		//then from the list on NPC make all the new cards

      for (int i = 0; i < QuestCollection.Length; i++){
        if(QuestCollection[i] != null){
          GameObject displayQuestCard = Instantiate (QuestCardPrefab, QuestCardsPannel.transform);
          QuestCardObjArray[i] = displayQuestCard;
          displayQuestCard.GetComponent<QuestCardSetup>().SetupCard(QuestCollection[i],i);
          if(QuestCollection[i].targetAmount > 1){
            displayQuestCard.GetComponent<QuestCardSetup>().SetupProgressCard(QuestProgressArray[i]);
          }
          if(QuestProgressArray[i] >= QuestCollection[i].targetAmount){
            displayQuestCard.GetComponent<QuestCardSetup>().CompletedCard();
          }
        }
      }


    }

    public void CompleteQuestCard(Quest clickedCard){

    }

    public void QuestProgress(int QuestCode, int Progress){

      for (int i = 0; i < QuestCollection.Length; i++){
        if(QuestCollection[i] != null && QuestCollection[i].SaveCode == QuestCode && QuestProgressArray[i] < QuestCollection[i].targetAmount){
          QuestProgressArray[i] += Progress;
          QuestCardObjArray[i].GetComponent<QuestCardSetup>().SetupProgressCard(QuestProgressArray[i]);
          if(QuestProgressArray[i] >= QuestCollection[i].targetAmount){
            QuestCardObjArray[i].GetComponent<QuestCardSetup>().CompletedCard();
          }
          //  print("Quest Completed!");
          //  QuestProgressArray[i] = 0;
          //  QuestCollection[i] = null;
          //  QuestSaveCodeArray[i] = 0;
          //  PopulateQuestLog();
          //  this.gameObject.GetComponent<AchievementsControl>().AchievementUpdate (5, 1);
          //}
          //quest.progressAmount =+ 1;
          SaveQuests();
        }
      }
    }

    public void QuestCompleted(int questCompCode){

      print("Quest Completed!");
      this.gameObject.GetComponent<AchievementsControl>().AchievementUpdate (5, 1);
      QuestProgressArray[questCompCode] = 0;
      QuestCollection[questCompCode] = null;
      QuestSaveCodeArray[questCompCode] = 0;
      PopulateQuestLog();
      SaveQuests();
    }
}
