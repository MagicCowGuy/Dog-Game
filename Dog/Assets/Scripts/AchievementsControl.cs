using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class AchievementsControl : MonoBehaviour {
//private GameObject[] achievementsList;
public Animator notifyAnim;
public notifyControl notifyConScript;
public expandMenu menuPannel;
public GameObject achievementsPannel;
public GameObject blackoutPannel;
public GameObject achievementPrefab;
public GameObject achContentFrame;
public Transform playerObj;
//public GameObject confettiPartObj;
//public GameObject confettiCameraObj;
//public Achievement[] achievementsList;
//public int[] achProgressArray;

public List <Achievement> achPrefabList;
public List <AchievementPanelScript> achPanScriptArray;
public List <int> achProgressList;
private int[] saveAchProgressArray;
public List <bool> achCompleteList;
//public static int achCount;

	// Use this for initialization
	void Awake () {
		achievementsPannel.SetActive(true);
		//achievementsList = GameObject.FindGameObjectsWithTag("Achievement");
		achievementsPannel.SetActive(false);
		LoadAchPrefabs();
		PopulateAchievements();
	}

	private void LoadAchPrefabs(){
		Object[] subListAchievements = Resources.LoadAll("Achievements", typeof(Achievement));
		foreach (Achievement subListAch in subListAchievements) {
			//Achievement tempAch = (GameObject)subListAch;
			achPrefabList.Add(subListAch);
			achProgressList.Add(0);
			achCompleteList.Add(false);

			saveAchProgressArray = PlayerPrefsX.GetIntArray ("AchProgress", 0, achProgressList.Count);
			achProgressList = saveAchProgressArray.ToList();
			while (achProgressList.Count < subListAchievements.Length) achProgressList.Add(0);
		}
	}

	private void SaveAchData() {
		saveAchProgressArray = achProgressList.ToArray();
		PlayerPrefsX.SetIntArray ("AchProgress", saveAchProgressArray);
    	PlayerPrefs.Save();
	}

	private void PopulateAchievements() {
		for(int i = 0; i < achPrefabList.Count; i++){
			GameObject achPanel = Instantiate (achievementPrefab, transform);
			achPanel.transform.SetParent(achContentFrame.transform);
			achPanel.transform.localScale = Vector3.one;
			achPanScriptArray.Add(achPanel.GetComponent<AchievementPanelScript>());

			//AchievementPanelScript achPanScript = achPanScriptArray[i];
			achPanScriptArray[i].achievementName = achPrefabList[i].title;
			achPanScriptArray[i].achievementBlurb = achPrefabList[i].description;
			achPanScriptArray[i].achievementAim = achPrefabList[i].targetAmount;
			achPanScriptArray[i].achievementProgress = achProgressList[i];
			achPanScriptArray[i].achievementCompleted = achPrefabList[i].completed;
			if(achPanScriptArray[i].achievementProgress >= achPanScriptArray[i].achievementAim){
				achPanScriptArray[i].achievementCompleted = true;
				achCompleteList[i] = true;
			}
			achPanScriptArray[i].contentUpdate();
		}
	}

	// Update is called once per frame
	public void ShowAchievementsUI () {
		menuPannel.menuHide();
		achievementsPannel.SetActive(true);
		blackoutPannel.SetActive(true);
	}

	public void HideAchievementsUI () {
		menuPannel.menuShow();
		achievementsPannel.SetActive(false);
		blackoutPannel.SetActive(false);
	}

	public void AchievementUpdate (int achUpCode, int achUpAmount) {


		for(int i = 0; i < achPrefabList.Count; i++)
		{
			if(achPrefabList[i].saveCode == achUpCode && achCompleteList[i] == false) {
				achProgressList[i] += achUpAmount;
				achPanScriptArray[i].achievementProgress = achProgressList[i];
				achPanScriptArray[i].contentUpdate();
				if(achProgressList[i] >= achPrefabList[i].targetAmount){
					print("Achievement Completed - " + achPrefabList[i].title);
					achCompleteList[i] = true;
					notifyConScript.AddNotify(new NoteToDisplay(1, achPrefabList[i].title, "New Achivement Unlocked!"));
					//confettiCameraObj.GetComponent<ParticleSystem>().Play();
				}
				SaveAchData();
			}
		}
	}

}
