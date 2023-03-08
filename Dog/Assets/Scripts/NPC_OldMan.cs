using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_OldMan : MonoBehaviour
{

	public bool holdforplayer = false;
	private UnityEngine.AI.NavMeshAgent navMeshAgent;
	private Animator myanim;
	private GameObject gameControlObj;

	public Transform leftHand;
	// walking, retrieving, returning, sitting, interacting
	public int MobStatus = 1;
	public int dirFactor;

	public GameObject targetPickup;
	public GameObject heldPickup;
	public Rigidbody pickupRB;


	// Start is called before the first frame update
	void Start()
	{
		gameControlObj = GameObject.Find("GameManagment");
		navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		myanim = GetComponent<Animator>();

		StartCoroutine("CheckForItems");
		StartCoroutine("WalkAbout");
	}

	// Update is called once per frame
	void Update()
	{
		//if(MobStatus == 1){
		myanim.SetFloat("Velocity", navMeshAgent.velocity.magnitude/navMeshAgent.speed);
		//}
	}

	public void SetupNPC(){
		//Space to setup NPC after spawn point is assigned.
		if(transform.position.x < 0){
		dirFactor = 1;
		} else {
		dirFactor = -1;
		}
	}

	public void PauseOrderNPC(){
		//Space for holding NPC while interacting with player.
	}

	public void ResumeOrderNPC(){
		//Space for finishing dialogue with player.
	}

	IEnumerator WalkAbout() {
		MobStatus = 1;
		while(true){
		if(Random.Range(0, 10) < 3){

		}
		navMeshAgent.destination = new Vector3(transform.position.x + (Random.Range(2f,5f) * dirFactor), 0 , Random.Range(-30.5f, -26.0f));
		yield return new WaitForSeconds(Random.Range(3,5.5f));
		}
	}

	IEnumerator PickupObj() {

		MobStatus = 2;
		while(Vector3.Distance(transform.position, targetPickup.transform.position) > 2){
		navMeshAgent.destination = targetPickup.transform.position;
		yield return new WaitForSeconds(1);
		}
		if(Vector3.Distance(transform.position, targetPickup.transform.position) < 2){
		myanim.SetTrigger("TrigPickup");
		yield return new WaitForSeconds(2.5f);
		StartCoroutine("returnObj");
		yield break;
		}
	}

	public void PickupPickup() {
		heldPickup = targetPickup;
		heldPickup.transform.SetParent(leftHand);
		pickupRB = heldPickup.GetComponent<Rigidbody>();
		pickupRB.isKinematic = true;
		pickupRB.useGravity = false;
		heldPickup.transform.localPosition = new Vector3 (-0.4f, -0.2f, 0);
		heldPickup.GetComponent<pickup>().pickedUp = true;		
	}

	public void ThrowPickup() {
		heldPickup.transform.parent = null;
		pickupRB.isKinematic = false;
 		pickupRB.useGravity = true;
		pickupRB.velocity = new Vector3(0, 15, 13);
		
		targetPickup = null;
		heldPickup = null;
		pickupRB = null;
		
		MobStatus = 1;
		//navMeshAgent.destination = transform.position;
		//gameControlObj.GetComponent<QuestControl>().QuestProgress(3,1);
	}

	IEnumerator returnObj() {
		MobStatus = 3;
		navMeshAgent.destination = new Vector3 (transform.position.x, transform.position.y, -27f);
		while(true){
		if(transform.position.z > -27.5f){
			yield return new WaitForSeconds(1);
			navMeshAgent.isStopped = true;
			navMeshAgent.ResetPath();
			navMeshAgent.isStopped = false;
			myanim.SetTrigger("TrigThrow");
			yield return new WaitForSeconds(3);
			StartCoroutine("WalkAbout");
			yield break;
		}
		yield return new WaitForSeconds(0.5f);
		}
	}

	IEnumerator CheckForItems() {
		while(true){
		//Check to despawn on distance along path.
		if(transform.position.x / dirFactor > 40){
			gameControlObj.GetComponent<NPC_Control>().DespawnMob(this.gameObject);
		}

		if(MobStatus == 1){
			GameObject[] possibletargets = GameObject.FindGameObjectsWithTag ("Pickup");
				GameObject closesttarget = null;
			//float distfromtarget = Mathf.Infinity;
			float tempdist = 0;
			float distcheck = 0;
				foreach (GameObject postar in possibletargets) {
			if(postar.transform.position.z < -27.5f && postar.GetComponent<pickup>().pickedUp == false){

				distcheck = Vector3.Distance(postar.transform.position, transform.position);
				if(closesttarget == null || distcheck < tempdist){
				tempdist = distcheck;
				closesttarget = postar;
				}
			}
			}

			if(closesttarget != null){
			targetPickup = closesttarget;
			StopCoroutine("WalkAbout");
			StartCoroutine("PickupObj");
			}
		}

		yield return new WaitForSeconds(1.5f);
		}
	}
}
