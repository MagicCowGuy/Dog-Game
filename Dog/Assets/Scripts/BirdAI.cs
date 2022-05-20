using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdAI : MonoBehaviour {

	private float idleTime = 0.0f;
	private float cheepTime = 100f;
	private Vector3 StartPos;
	private UnityEngine.AI.NavMeshAgent navMeshAgent;
	public Transform playerTransform;
	private float distanceToPlayer;
	private bool grounded = true;
	public bool walking;
	public Animator animbird;
	public GameObject emotesprite;
	private SpriteRenderer m_SpriteRenderer;
	public GameObject headObject;
	public GameObject RewardObject;
	public int RewardAmount;

	private GameObject gameControlObj;
	//public string achievementName;
	//public int achievementInt;

	// Use this for initialization
	void Awake () {
		gameControlObj = GameObject.Find("GameManagment");
		navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent> ();
		StartPos = transform.position;
		cheepTime = Random.Range (25f, 180f);
		//animbird = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update () {


		if (grounded) {

				idleTime -= 0.1f;
				cheepTime -= 0.1f;

				if (idleTime <= 0) {
					Vector3 offsetpos = new Vector3 (Random.Range (-3f, 3f), 0, Random.Range (-3f, 3f));
					navMeshAgent.destination = StartPos + offsetpos;
					idleTime = Random.Range (10f, 55f);
				}

				if (cheepTime <= 0) {
					GameObject emoteclone = Instantiate (emotesprite, headObject.transform.position, Quaternion.LookRotation(Vector3.forward));
					emoteclone.transform.localScale = new Vector3 (5,5,5);
					emoteclone.transform.SetParent (headObject.transform);
					emoteclone.transform.localPosition = new Vector3 (-1, 0.5f, 1.25f);
					if(transform.rotation.eulerAngles.y > 0 && transform.rotation.eulerAngles.y < 180){
						m_SpriteRenderer = emoteclone.GetComponent<SpriteRenderer>();
						m_SpriteRenderer.flipX = true;
					}
					cheepTime = Random.Range (25f, 180f);
				}

			if(walking && !navMeshAgent.hasPath){
				walking = false;
				animbird.SetBool ("Walking", false);
			}

			if(!walking && navMeshAgent.hasPath){
				walking = true;
				animbird.SetBool ("Walking", true);
			}




			distanceToPlayer = (playerTransform.position - transform.position).magnitude;

			if (distanceToPlayer <= 6.5f) {
				//print ("EEEEEEK");

				FlyAway ();
				//bird control achivement (guessing its 3)
				gameControlObj.GetComponent<AchievementsControl>().AchievementUpdate (3, 1);

				//GameGrind.Journal.Increment ("Birds Be Gone", 1);
			}




		} else {

			transform.position += (Vector3.up * 0.3f);
			transform.position += (navMeshAgent.transform.forward * 0.2f);

		}

		if (transform.position.y >= 25) {
			DestroyGameObject();
		}
	}
	//Quaternion flydirection added later
	void FlyAway () {
		animbird.SetBool ("Flying", true);
		navMeshAgent.enabled = false;
		grounded = false;

		gameControlObj.GetComponent<BirdControl>().birdCount -= 1;
		gameControlObj.GetComponent<QuestControl>().QuestProgress(1,1);

		for (int i = 0; i < RewardAmount; i++) {
			//spawns rewards evenly spread out
			GameObject rewardspawn = (GameObject)Instantiate (RewardObject, transform.position - new Vector3 (0, -2, 0), Quaternion.Euler (0, 0, 0));
			rewardspawn.transform.eulerAngles = new Vector3 (0, ((360 / RewardAmount) * (i + 1)), 0);
			rewardspawn.GetComponent<Rigidbody>().velocity = rewardspawn.transform.forward * Random.Range(1,12) + Vector3.up * Random.Range(4,6);
		}

	}

	void DestroyGameObject()
	{
		Destroy(gameObject);
	}
}
