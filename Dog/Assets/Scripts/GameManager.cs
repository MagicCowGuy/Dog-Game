using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

public GameObject optionsPannel;
public expandMenu menuPannel;
public UIBgControl uIBg;
//public GameObject blackoutPannel;
//public CamTarget cameraScript;
public cameraControl camConScript;

void Start()
{
		//Application.targetFrameRate = 60;
		QualitySettings.vSyncCount = 0;
		ShowMenu();
		//uIBg = this.GetComponent<UIBgControl>();
}

	public void doExitGame()

	{
		print ("Quitting");
		//UnityEditor.EditorApplication.isPlaying = false;
		Application.Quit();
	}

	public void ShowMenu () {
		menuPannel.menuHide();
		optionsPannel.SetActive(true);
		uIBg.clearOut();
		//blackoutPannel.SetActive(true);
		camConScript.menuMode();
		//cameraScript.cameraMode = 3;
	}

	public void HideMenu () {
		menuPannel.menuShow();
		optionsPannel.SetActive(false);
		uIBg.noneOut();
		//blackoutPannel.SetActive(false);
		camConScript.watchPlayer();
		//cameraScript.cameraMode = 1;
	}

	public void ResetSave () {
		PlayerPrefs.DeleteAll();
		print("RESETTING SAVE DATA");
		HideMenu();
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
