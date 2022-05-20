using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class emoteSprite : MonoBehaviour {

public GameObject EmoteSpriteObj;
public Animator spriteAnimator;

	// Use this for initialization
	void Awake () {
		//spriteAnimator.Play("Elastic_Awake");
	}

	// Update is called once per frame
	void Update () {
		EmoteSpriteObj.transform.LookAt (Camera.main.transform.position);

	}

void KillSprite (){
	Destroy(this.gameObject);
}

}
