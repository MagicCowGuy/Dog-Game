using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

public GameObject optionsPannel;
public expandMenu menuPannel;
public GameObject blackoutPannel;
public CamTarget cameraScript;

void Start()
{
		//Application.targetFrameRate = 60;
		ShowMenu();
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
		blackoutPannel.SetActive(true);
		cameraScript.cameraMode = 3;
	}

	public void HideMenu () {
		menuPannel.menuShow();
		optionsPannel.SetActive(false);
		blackoutPannel.SetActive(false);
		cameraScript.cameraMode = 1;
	}

	public void ResetSave () {
		PlayerPrefs.DeleteAll();
		print("RESETTING SAVE DATA");
		HideMenu();
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
