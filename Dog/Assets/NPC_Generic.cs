using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;

public class NPC_Generic : MonoBehaviour
{
	public NavMeshAgent myNMA;
	public Animator myAnim;
	public GameObject playerObj;
	public Transform headBone;

	public float maxHeadAngle = 50f;
	private Quaternion headTarRot;
	private Vector3 clampHeadAngle;

	// Start is called before the first frame update
	void Start()
	{
		StartCoroutine("slowLoop");
	}

	// Update is called once per frame
	void Update()
	{
		myAnim.SetFloat("Velocity", myNMA.velocity.magnitude);
	}

	void LateUpdate() {
		headTarRot = Quaternion.LookRotation(playerObj.transform.position - headBone.position, transform.up);
		//print(headBone.localRotation.x + ", " + headBone.localRotation.y + ", " +headBone.localRotation.z );
		

		//headTarRot.x = Mathf.Clamp(headTarRot.x, -0.1f, 0.1f);
		//headTarRot.y = Mathf.Clamp(headTarRot.y, -0.1f, 0.1f);
        headTarRot.z = Mathf.Clamp(headTarRot.z, -0.1f, 0.1f);

		headBone.rotation = headTarRot * Quaternion.Euler(0,90,0);
		
	}

	IEnumerator slowLoop(){
		while(true){
			//print("GO TO PLAYER POS");
			myNMA.SetDestination(playerObj.transform.position);

			if(myNMA.remainingDistance > 10){
				myNMA.speed = 9;
				myAnim.SetBool("Running", true);
			} else {
				myNMA.speed = 3.5f;
				myAnim.SetBool("Running", false);
			}

			yield return new WaitForSeconds(0.2f);
		}
	}

}
