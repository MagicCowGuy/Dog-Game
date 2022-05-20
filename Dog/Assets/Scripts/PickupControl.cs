using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupControl : MonoBehaviour {

	public bool pickedUp;
	public bool dropped;
	public Rigidbody rb;
	public Collider itemcol;
	public Collider targetcol;
	private GameObject gameController;

	public bool postdrop;
	private Vector3 droppoint;
	public Transform targetZoneTrigger;
	//private GameObject lightBulbObject;
  public PlayerInfo playerinfoscript;

  public PickupProperties properties;

	public GameObject killEffect;
  public GameObject RewardObject;
	public int RewardAmount;

	public int AchCode;
	public int AchIncrement;

	public int QuestCode;
	public int QuestIncrement;

	public Vector3 locpos = new Vector3(-0.25f,0,1);
	public Vector3 locrot = new Vector3 (0, 0, 0);

	public AudioClip soundImpact;
	public AudioClip soundThrow;

	// Use this for initialization
	void Start () {
		//lightBulbObject = GameObject.Find("Light_Bulb");
		gameController = GameObject.Find("GameManagment");
	}

	// Update is called once per frame
	void Update () {
		// if (rb.velocity == null) {
		// 	dropped = false;
		// 	postdrop = false;
		// }

		if (postdrop) {
			if ((droppoint.y - transform.position.y) > 2.5f) {
				gameController.GetComponent<AchievementsControl>().AchievementUpdate (AchCode, AchIncrement);
				//GameGrind.Journal.Increment (AchName, AchIncrement);
			}
		}
	}

	public void PlayerPickup (GameObject CollectingPlayer){

		transform.SetParent(CollectingPlayer.transform);
		publicSound(2, soundImpact);
        //playerinfoscript = GameObject.Find("PlayerInfo").GetComponent<PlayerInfo>();
        //playerinfoscript.ItemChange(properties);
				//lightBulbObject.gameObject.GetComponent<lightbulb>().LightUpBulb(5.5f);

		pickedUp = true;
		postdrop = false;
		DisableRagdoll ();
		transform.localPosition = locpos;
		transform.localRotation = Quaternion.Euler(locrot);
	}

	public void PlayerDrop(Vector3 inheritedVelocity)
	{
		//print(inheritedVelocity.y);
		//lightBulbObject.gameObject.GetComponent<lightbulb>().SwitchOffBulb();
		if(inheritedVelocity.y < 10){
			transform.localPosition += new Vector3(0,-0.25f,0);
		}
		transform.parent = null;
		EnableRagdoll ();
		dropped = true;
		pickedUp = false;
		rb.velocity = inheritedVelocity;
		rb.AddTorque(new Vector3(Random.Range(-200,200),Random.Range(-200,200),Random.Range(-200,200)));
		//playerinfoscript.ItemDrop();
    }

	void EnableRagdoll() {
		rb.isKinematic = false;
		rb.detectCollisions = true;
		rb.useGravity = true;
	}
	void DisableRagdoll() {
		rb.isKinematic = true;
		rb.detectCollisions = false;
		rb.useGravity = false;
	}

	public void publicSound(float soundVolume, AudioClip soundFile){
		GetComponent<AudioSource>().PlayOneShot(soundFile, soundVolume);
	}

	void OnCollisionEnter (Collision col){
		//IGNORE IF NO TARGET DROPOFF

		publicSound(1.5f, soundThrow);

		if (dropped == true && targetZoneTrigger != null){
			itemcol = gameObject.GetComponent<Collider> ();
			targetcol = targetZoneTrigger.GetComponent<Collider> ();

			if (targetcol.bounds.Intersects(itemcol.bounds)) {
				gameController.GetComponent<AchievementsControl>().AchievementUpdate (AchCode, AchIncrement);
				gameController.GetComponent<QuestControl>().QuestProgress(QuestCode,QuestIncrement);
				//print("Sending achivement stuff");
				//GameGrind.Journal.Increment (AchName, AchIncrement);
				Instantiate (killEffect, transform.position - new Vector3 (0, -1, 0), Quaternion.Euler (-90, 0, 0));


				for (int i = 0; i < RewardAmount; i++) {
					//spawns rewards evenly spread out
					GameObject rewardspawn = (GameObject)Instantiate (RewardObject, transform.position - new Vector3 (0, -2, 0), Quaternion.Euler (0, 0, 0));
					rewardspawn.transform.eulerAngles = new Vector3 (0, ((360 / RewardAmount) * (i + 1)), 0);
					rewardspawn.GetComponent<Rigidbody>().velocity = rewardspawn.transform.forward * Random.Range(1,12) + Vector3.up * Random.Range(4,6);
				}

				Destroy (gameObject);

			}
			print ("Dropped object hit ground.");
			dropped = false;

		}

		if (dropped == true && postdrop == false) {
			postdrop = true;
			droppoint = transform.position;
		}

	}

}
