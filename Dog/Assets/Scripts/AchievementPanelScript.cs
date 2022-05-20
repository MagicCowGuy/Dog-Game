using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementPanelScript : MonoBehaviour {

	public string achievementName;
	public string achievementBlurb;
	public int achievementAim;
	public int achievementProgress;
	public bool achievementCompleted;
	private float achievementPercentage;
	//private float startingsize;

	public Color completedcolour;

	public TextMeshProUGUI achievementNameTextObj;
	public TextMeshProUGUI achievementBlurbTextObj;
	public GameObject achievementProgressBasePanel;
	public GameObject achievementProgressPanel;

	// Use this for initialization
	void Awake () {
		contentUpdate();
	}

	// Update is called once per frame
	public void contentUpdate () {
		achievementNameTextObj.text = achievementName;
		achievementBlurbTextObj.text = achievementBlurb;
		RectTransform rt = achievementProgressPanel.GetComponent<RectTransform>();
		if(achievementProgress != 0){
			//startingsize = rt.sizeDelta.x;
			achievementPercentage = ((achievementProgress + 0.0f) / achievementAim);
			rt.sizeDelta = new Vector2 ((200 * achievementPercentage), rt.sizeDelta.y);

		} else {
			rt.sizeDelta = new Vector2 (0, rt.sizeDelta.y);
		}
	}
}
