using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldManControl : MonoBehaviour {


	public GameObject headNode;
	public GameObject playerObj;
	private GameObject targetObj;
	public GameObject lookTarget;
	public Vector3 lookdirection;
	private Vector3 lookrelativepos;
	private Quaternion targetlookrot;
	private UnityEngine.AI.NavMeshAgent navMeshAgent;
	//private Vector3 diftoplayer;
	//private Vector3 angletolook;
	private bool busy;
	private bool retrieving;
	private bool returning;
	private Animator myanim;

	private GameObject[] possibletargets;
	private GameObject closesttarget;
	private Vector3 difftarget;
	private float distfromtarget;
	private float tempdist;
	public GameObject targetpickup;
	private GameObject pickedupObject;
	private Rigidbody targetrb;
	public float cooldown;
	public Transform leftHand;
	private bool throwing = false;

	private float breakTime = 5;
	private bool walking = false;

	private GameObject gameControlObj;

	void Awake () {
		gameControlObj = GameObject.Find("GameManagment");
		navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent> ();
		myanim = GetComponent<Animator>();
	}

	// Use this for initialization
	void Start () {
		//navMeshAgent.destination = new Vector3 (15f, -2.5f, -20.4f);
	}

	// Update is called once per frame
	void Update () {
		//diftoplayer = transform.position - playerObj.transform.position;
		//angletolook = playerObj.transform.InverseTransformDirection(diftoplayer);

		if (!walking && !busy){
			breakTime -= 0.1f;
			if(breakTime < 0){
				walking = true;
				walkAbout();
			}
		}

		if(transform.position.x > 40){
			gameControlObj.GetComponent<NPC_Control>().DespawnMob(this.gameObject);
		}

		if (!navMeshAgent.pathPending && walking){
			breakTime = Random.Range(10.0f,35.0f);

			walking = false;
		}

		if (!busy) {
			if (targetpickup == null) {
				if (cooldown < 0) {
					CheckforBall ();
				}
			} else {
				busy = true;
			}
		} else {
			//retrieving = true;
		}

		if (retrieving && pickedupObject == null) {
			RetrivePickup ();
		}

		if (targetpickup != null && targetpickup.transform.position.z > -16.5 ) {
			busy = false;
		}
		cooldown -= 0.5f;

		if (returning && transform.position.z > -20.1f && !throwing) {
			myanim.SetTrigger("TrigThrow");
			throwing = true;
			//ThrowPickup ();
		}

		myanim.SetFloat("Velocity", navMeshAgent.velocity.magnitude);

	}

	void walkAbout(){
		//bit for choosing a new destination along the track.
		navMeshAgent.destination = new Vector3(transform.position.x + Random.Range(2f,5f), -2.5f, Random.Range(-21f, -17.5f));
	}

	void LateUpdate (){

		if (lookTarget != null) {
			lookrelativepos = headNode.transform.position - lookTarget.transform.position;
			targetlookrot = Quaternion.LookRotation (lookrelativepos);
			targetlookrot *= Quaternion.Euler( new Vector3(0, -90f, -90f));
			headNode.transform.rotation = Quaternion.RotateTowards (headNode.transform.rotation, targetlookrot, Time.deltaTime * 50);
		}

	}

	void RetrivePickup (){

		difftarget = targetpickup.transform.position - transform.position;
		distfromtarget = difftarget.magnitude;
		navMeshAgent.destination = targetpickup.transform.position;

		if (distfromtarget < 1.75f && cooldown < 0) {
			myanim.SetTrigger("TrigPickup");
			retrieving = false;
			navMeshAgent.isStopped = true;
			//PickupPickup (targetpickup);
		}
	}

	void PickupPickup () {
		// old one was void PickupPickup (GameObject objtopickup) {
			//play future animation to do this, but for now...
		pickedupObject = targetpickup;
		pickedupObject.transform.SetParent(leftHand);
		targetrb = pickedupObject.GetComponent<Rigidbody>();
		targetrb.isKinematic = true;
		targetrb.useGravity = false;
		pickedupObject.transform.localPosition = new Vector3 (-0.4f, -0.2f, 0);
		returning = true;
		navMeshAgent.destination = new Vector3 (transform.position.x, transform.position.y, -10.2f);
		lookTarget = null;
	}

	void ReturnPickup () {
		navMeshAgent.isStopped = false;
	}

	void ThrowPickup () {
		print ("Old man throw ball good");
		//pickedupObject.transform.position = transform.position + new Vector3 (0, 5, 0);
		gameControlObj.GetComponent<QuestControl>().QuestProgress(3,1);

		targetrb = pickedupObject.GetComponent<Rigidbody>();
		pickedupObject.transform.parent = null;
		targetrb.isKinematic = false;
		targetrb.useGravity = true;
		targetrb.velocity = new Vector3(0, 13, 13);
		targetpickup = null;
		pickedupObject = null;
		cooldown = 60;
		busy = false;
		returning = false;
		lookTarget = playerObj;
		throwing = false;
		navMeshAgent.isStopped = false;


	}

	void CheckforBall () {
		possibletargets = GameObject.FindGameObjectsWithTag ("Pickup");
		closesttarget = null;
		distfromtarget = Mathf.Infinity;
		foreach (GameObject go in possibletargets) {
			difftarget = go.transform.position - transform.position;
			tempdist = difftarget.magnitude;
			if (tempdist < distfromtarget && go.transform.position.z < -16.5 && go.GetComponent<PickupControl>().pickedUp == false) {
				closesttarget = go;
				distfromtarget = tempdist;
			}
		}
		if (closesttarget != null)
		{
			targetpickup = closesttarget;
			retrieving = true;
			lookTarget = targetpickup;
		}
	}

	void DestinationReachedCheck() {


	}
}
