using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {

	public Vector3 posoffset;
	public bool specificoffset;

	public bool lockInInteract;
	public GameObject parentForPlayer;
	public Vector3 camOffset;

	public GameObject playerObj;
	private float distancetoPlayer;

	public interact_part_effects particleScript;

	public bool clicked;
	public bool interacting;

	public float radius;

	[Header("Audio")]
	  public AudioClip impactSound;
		public float impactSoundVolume;
		//public float impactSoundPitch;

	public void OnClick () {

		clicked = true;

		if(!specificoffset){
			posoffset = playerObj.transform.position - transform.position;
			distancetoPlayer = posoffset.magnitude;
			posoffset = (posoffset / distancetoPlayer) * 0.1f;
			print(posoffset);
		}

	}

	public void PlayerLanding (Vector3 forceLanding) {
		parentForPlayer.GetComponent<Rigidbody>().AddForce(Vector3.down * 250 + forceLanding * 300);
		interacting = true;
		if (particleScript != null){
			particleScript.PlayerLandingFunction();
		}

		if (impactSound != null){
			playerObj.GetComponent<TouchMovement>().publicSound(impactSoundVolume,impactSound);
		}
	}

	public void PlayerLeave (){
		interacting = false;
	}

	// Use this for initialization
	void Awake () {
		playerObj = GameObject.FindGameObjectWithTag("Player");
	}

	// Update is called once per frame
	void Update () {

	}
}
