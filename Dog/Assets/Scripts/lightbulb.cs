using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightbulb : MonoBehaviour {

public GameObject headObj;
public Vector3 posoffset;
private GameObject spriteObj;
private Animator spriteAnim;
private SpriteRenderer spriteRen;

public bool lightOn = false;

	// Use this for initialization
	void Start () {

		spriteObj = this.gameObject.transform.Find("LB_Sprite").gameObject;
		spriteAnim = spriteObj.GetComponent<Animator>();
		spriteRen = spriteObj.GetComponent<SpriteRenderer>();
		if(!lightOn){
			spriteRen.enabled = false;
		}
//LightUpBulb(5.4f);
	}

	// Update is called once per frame
	void Update () {
		transform.LookAt (Camera.main.transform.position);
		transform.position = Vector3.Lerp(transform.position,(headObj.transform.position + posoffset), Time.deltaTime * 20);
	}

	public void LightUpBulb (float duration) {
		spriteAnim.SetTrigger("LightUp");
		spriteRen.enabled = true;
		lightOn = true;
	}

	public void SwitchOffBulb (){
		spriteAnim.SetTrigger("SwitchOff");
		lightOn = false;
	}
}
