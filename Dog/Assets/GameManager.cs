using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;

public class GameManager : MonoBehaviour {

public GameObject optionsPannel;
public expandMenu menuPannel;
public GameObject blackoutPannel;

void Start()
{
		Application.targetFrameRate = 60;
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
	}

	public void HideMenu () {
		menuPannel.menuShow();
		optionsPannel.SetActive(false);
		blackoutPannel.SetActive(false);
	}

	public void ResetSave () {
		PlayerPrefs.DeleteAll();
		print("RESETTING SAVE DATA");
		HideMenu();
	}
}
