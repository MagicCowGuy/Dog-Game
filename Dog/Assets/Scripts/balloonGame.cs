using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_EDITOR

public class balloonGame : MonoBehaviour
{
	private Rigidbody rb;
	private GameObject playerObj;
	private Vector3 playerPos;

	private Transform playerHead;
	private Vector3 bounceVelocity;

	public Vector3 bounceAreaStart;
	public float bounceAreaWidth;
	public float bounceAreaDepth;

	public Vector2 boopAreaMin;
	public Vector2 boopAreaMax;
	public float boopAreaDispHeight;
	public Rect boopAreaRect;

	private float boopPower;
	private float boopHeight;

	private float boopTime;
	public bool booping = false;
	public GameObject groundSprite;
	public Transform balloonShape;

	private Vector3 boopTarget;
	private Vector3 boopStart;
	private Vector3 boopPos;
	private Vector3 boopForce;

	private Vector3 returnWind;

	public AudioClip boopSound;

	// Start is called before the first frame update
	void Start()
	{
		rb = this.GetComponent<Rigidbody>();
		playerObj = GameObject.FindGameObjectWithTag("Player");
		playerHead = playerObj.GetComponent<TouchMovement>().headObject.transform;

		boopAreaRect = Rect.MinMaxRect(boopAreaMin.x, boopAreaMin.y, boopAreaMax.x, boopAreaMax.y);
		StartCoroutine("boundsCheck");
	}

	// Update is called once per frame
	void Update()
	{
		
		Debug.DrawRay(transform.position + Vector3.down * 0.6f, Vector3.down * 100, Color.white, 0, true);
		Debug.DrawRay(boopTarget + Vector3.down * 0.6f, Vector3.down * 100, Color.black, 0, true);
		//Manual Gravity
		//rb.velocity += Vector3.down * 0.05f;

		if(returnWind != Vector3.zero){
			rb.velocity = Vector3.MoveTowards(rb.velocity, returnWind, 10 * Time.deltaTime);
		}

		

		
		//Vector3 horizVelocity = new Vector3(rb.velocity.x,0,rb.velocity.z);
		//if(transform.position.y > -1.0f && horizVelocity.magnitude < 1.5f){
		if(rb.velocity.y < 0){
			groundSprite.GetComponent<SpriteRenderer>().enabled = true;
			groundSprite.transform.position = new Vector3(transform.position.x, 0.1f, transform.position.z);
			float spriteScale = 0.25f + Vector3.Distance(transform.position, groundSprite.transform.position)/10;
			groundSprite.transform.localScale = spriteScale * Vector3.one;
			groundSprite.transform.rotation = Quaternion.Euler(-90,0,0);
		} else {
			groundSprite.GetComponent<SpriteRenderer>().enabled = false;
		}
	}

	IEnumerator boundsCheck(){

		while(true){

			if(boopAreaRect.Contains(new Vector2(transform.position.x, transform.position.z), true)){
				returnWind = Vector3.zero;
			} else {
				if(transform.position.y < 6){
					returnWind.y = 8;
				} else {
					returnWind.y = -2;
				}
				if(transform.position.x < boopAreaRect.xMin){
					returnWind.x = 5;
				}
				if(transform.position.x > boopAreaRect.xMax){
					returnWind.x = -5; 
				}
				if(transform.position.z < boopAreaRect.yMin){
					returnWind.z = 5;
				}
				if(transform.position.z > boopAreaRect.yMax){
					returnWind.z = -5; 
				}
			}
			yield return new WaitForSeconds(0.5f);
		}

		//yield break;
	}

	private void OnTriggerEnter(Collider other){
		if(other.tag == "Player"){
			boopCheck();
		} else {
			booping = false;
		}
	}

	private void OnCollisionEnter(Collision collision){
		if(collision.collider.tag != "Player"){
			float impactFactor = Mathf.Clamp(collision.relativeVelocity.magnitude/6,0,1);
			publicSound(0.1f + impactFactor, boopSound);
			//print(collision.relativeVelocity.magnitude);
		}
	}

	public void boopCheck(){

		boopPower = Random.Range(8,20);
		boopStart = transform.position;
		playerPos = playerObj.transform.position;

		boopTarget = boopStart + Vector3.Normalize(boopStart - new Vector3(playerPos.x, boopStart.y, playerPos.z)) * boopPower;

		//var rect = Rect.MinMaxRect(boopAreaMin.x, boopAreaMin.y, boopAreaMax.x, boopAreaMax.y);
		if(boopAreaRect.Contains(new Vector2(boopTarget.x, boopTarget.z), true)){
			boopTarget.y = 10;
			boopLaunch(boopTarget);
		} else {
			//TO PROGRAM - random chance to be random pos within a range or totally punt it
			boopTarget = new Vector3(Random.Range(boopAreaMin.x + 2 , boopAreaMax.x - 2),10,Random.Range(boopAreaMin.y + 2 , boopAreaMax.y - 2));
			boopLaunch(boopTarget);
		}
	}

	private void boopLaunch(Vector3 bTarget){
		//play boop sound with volume based off distance to cover
		booping = true;
		StartCoroutine("boopCoRo");
		//transform.position = bTarget;
	}

	IEnumerator boopCoRo(){
		boopTime = 0;
		

		boopForce = (boopTarget - transform.position);
		boopForce.y = 10;
		rb.velocity = boopForce;

		rb.AddTorque(new Vector3(Random.Range(-200,200),Random.Range(-200,200),Random.Range(-200,200)));
    
		publicSound(3f, boopSound);

		while(boopTime < 1){
			float falloff = 1f - (Mathf.Pow(boopTime, 6) * 0.9f);

			rb.velocity = Vector3.Scale(rb.velocity, new Vector3(falloff, 1 , falloff));
		
			boopTime += Time.deltaTime * 0.5f;
			yield return null;
		}
		booping = false;

		yield break;
	}

	public void publicSound(float soundVolume, AudioClip soundFile){
      //GetComponent<AudioSource>().volume = soundVolume;
      //GetComponent<AudioSource>().pitch = soundPitch;
      GetComponent<AudioSource>().volume = Random.Range(0.95f, 1.05f);
      GetComponent<AudioSource>().pitch = 1 + Random.Range(-0.1f,0.1f);
      GetComponent<AudioSource>().PlayOneShot(soundFile, soundVolume);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(balloonGame))]
public class balloonGameEditor : Editor
{
  private PathNavPoint[] PathPointsGUI;


  public void OnSceneGUI()
  {
	  var LObj = target as balloonGame;
	float dispY = LObj.boopAreaDispHeight;
	  //drawPath(LObj.PathEnter);
	  //drawPath(LObj.PathExit);
	  //Path in to scene
	  //bounce NavMesh Area to roam
	  Vector3[] bounceAreaVerts = new Vector3[]
			  {
				
				new Vector3(LObj.boopAreaMin.x, dispY, LObj.boopAreaMin.y),
				new Vector3(LObj.boopAreaMin.x, dispY, LObj.boopAreaMax.y),
				new Vector3(LObj.boopAreaMax.x, dispY, LObj.boopAreaMax.y),
				new Vector3(LObj.boopAreaMax.x, dispY, LObj.boopAreaMin.y),

			  };
	  //EditorGUI.DrawRect(new Rect(LObj.bounceAreaCorners[0].x, LObj.bounceAreaCorners[0].z, LObj.bounceAreaCorners[1].x, LObj.bounceAreaCorners[1].z), Color.green);
	  Handles.DrawSolidRectangleWithOutline(bounceAreaVerts, new Color(0.5f, 0.5f, 0.5f, 0.1f), Color.white);
	}
}
#endif // UNITY_EDITOR