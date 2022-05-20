using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scoring : MonoBehaviour {

public float coinNumber;
public TextMeshProUGUI coinstext;
public TextContainer m_TextContainer;
public GameObject coinTextObject;
public GameObject coinSprite;
private Animator coinAnim;

	// Use this for initialization
	void Awake () {
		coinNumber = 0;
		ScoreUpdate();

	}

	public void ScoreUpdate() {

		//update the text
		coinstext = coinTextObject.GetComponent<TextMeshProUGUI>();
		coinstext.text = coinNumber.ToString();
		//play pulse animation
		coinAnim = coinSprite.GetComponent<Animator>();
		coinAnim.SetTrigger("Pulse");
	}

	// Update is called once per frame
	void Update () {

	}
}
