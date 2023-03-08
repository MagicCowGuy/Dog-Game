using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickupControl : MonoBehaviour
{

	private cameraControl camConScript;

	// Start is called before the first frame update
	void Start()
	{
		camConScript = this.GetComponent<cameraControl>();
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	public void spawnPickup (GameObject pickupPrefab, bool isIntro, Vector3 spawnPos, Vector3 spawnVelocity){
		GameObject newPickup = Instantiate (pickupPrefab, spawnPos, Quaternion.LookRotation(Vector3.forward));
		if(isIntro){
			print("Introducing the " + newPickup.GetComponent<pickup>().pickupName + " pickupable object");
			StartCoroutine("introPickup");
			StartCoroutine (camConScript.tempFocus(newPickup,4));
		}
	
	}

	IEnumerator introPickup(){

		yield break;
	}
}
