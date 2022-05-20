using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotatePulse : MonoBehaviour {

	public bool pulsing = true;
	public bool spinning = true;

	private float pulsescale;
	private float startScale;

	public Transform CircleSprite;

	// Use this for initialization
	void Start () {
		CircleSprite = GetComponent<Transform>();
		startScale = CircleSprite.localScale.x;
	}

	// Update is called once per frame
	void Update () {
		pulsescale = startScale + Mathf.Sin((Time.time * 0.5f) * 10 ) / 30;
		if(spinning){transform.Rotate(Vector3.forward * Time.deltaTime * 20);} else {Quaternion.Euler(0, 0, 0);}
		if(pulsing){CircleSprite.transform.localScale = new Vector3 (pulsescale,pulsescale,1);
			//this.transform.localScale.x = (Mathf.Sin((Time.time + 0.25f) * 10 ) / 6);
		} else {
			CircleSprite.transform.localScale = Vector3.one;
		}

	}
}
