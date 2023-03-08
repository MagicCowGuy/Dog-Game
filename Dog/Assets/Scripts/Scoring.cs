using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scoring : MonoBehaviour {

public int coinNumber;
public TextMeshProUGUI coinstext;
public TextContainer m_TextContainer;
public GameObject coinTextObject;
public GameObject coinSprite;
private Animator coinAnim;

public int realCoin;
public GameObject coinObject;
  
	// Use this for initialization
	void Awake () {
		coinNumber = 0;
		ScoreUpdate(0);
		
	}

	public void ScoreUpdate(int updateFactor) {

		coinNumber += updateFactor;

		//update the text
		coinstext = coinTextObject.GetComponent<TextMeshProUGUI>();
		coinstext.text = coinNumber.ToString();
		//play pulse animation
		coinAnim = coinSprite.GetComponent<Animator>();
		coinAnim.SetTrigger("Pulse");
	}

	public void CoinDrop(Vector3 dropPoint, int coinCount){
		realCoin += coinCount;
		  for (int i = 0; i < coinCount; i++) {
			//spawns rewards evenly spread out
			GameObject rewardspawn = (GameObject)Instantiate (coinObject, dropPoint - new Vector3 (0, -2, 0), Quaternion.Euler (0, 0, 0));
			rewardspawn.transform.eulerAngles = new Vector3 (0, ((360 / coinCount) * (i + 1)), 0);
			rewardspawn.GetComponent<Rigidbody>().velocity = rewardspawn.transform.forward * Random.Range(1,12) + Vector3.up * Random.Range(4,6);
          }
	}

	// Update is called once per frame
	void Update () {

	}
}
