using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click_Projector : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Projector proj = GetComponent<Projector> ();
		if (proj.orthographicSize > 2f) {
			Destroy(gameObject);
		}
		proj.orthographicSize += 0.15f;
	}
}
