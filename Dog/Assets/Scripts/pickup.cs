using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_EDITOR

public class pickup : MonoBehaviour
{
  public bool pickedUp;
  public bool throwable;
  private bool midair = false;
	private Rigidbody rb;
  private GameObject gameController;
  public targetPoint[] targetPoints;
  public float playerForce;
  public bool playerUsing;
  public GameObject playerUseEffect;

  public PickupProperties properties;
	public GameObject killEffect;
  public GameObject RewardObject;
  public GameObject targetAreaEffect;
  private GameObject parForEffect;
  public GameObject objTargetOutline;

  public string IDTag;
  public int RewardAmount;

  public int AchCode;
  public int AchIncrement;

  public int TaskCode;
  public int TaskProgress;

  public Vector3 locpos = new Vector3(-0.25f,0,1);
  public Vector3 locrot = new Vector3 (0, 0, 0);

  public AudioClip soundImpact;
  public AudioClip soundThrow;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        gameController = GameObject.Find("GameManagment");
        if(throwable){
          StartCoroutine("slowCheck");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayerPickup (GameObject CollectingPlayer){
      transform.SetParent(CollectingPlayer.transform);
      publicSound(1.5f, soundImpact);

      pickedUp = true;
      DisableRagdoll();
      transform.localPosition = locpos;
  		transform.localRotation = Quaternion.Euler(locrot);
      showTargetAreas();
    }

    public void PlayerDrop(Vector3 inheritedVelocity) {
      //Offset if just dropping to ground.
      if(inheritedVelocity.y < 10){
  			transform.localPosition += new Vector3(0,-0.25f,0);
  		}
  		transform.parent = null;
      EnableRagdoll();
      pickedUp = false;
      midair = true;
      rb.velocity = inheritedVelocity;
      //Adding some spinnnn!
  		rb.AddTorque(new Vector3(Random.Range(-200,200),Random.Range(-200,200),Random.Range(-200,200)));
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

    public void playerUseStart(){
        playerUsing = true;
        if(playerUseEffect != null){
          playerUseEffect.SetActive(true);
        }
    }

    public void playerUseStop(){
        playerUsing = false;
        playerForce = 0;
        if(playerUseEffect != null){
          playerUseEffect.SetActive(false);
        }

    }

    void showTargetAreas(){
      hideTargetAreas();
      parForEffect = new GameObject("Area Effects");

      foreach(targetPoint tarpoint in targetPoints){
        GameObject tarAreaEff = Instantiate(targetAreaEffect, tarpoint.TarPointPos, Quaternion.identity);
        tarAreaEff.transform.SetParent(parForEffect.transform);
      }

      if(objTargetOutline != null){
        objTargetOutline.GetComponent<Outline>().enabled = true;
      }
    }

    void hideTargetAreas(){
      Destroy(parForEffect);

      if(objTargetOutline != null){
        objTargetOutline.GetComponent<Outline>().enabled = false;
      }
    }

    public void publicSound(float soundVolume, AudioClip soundFile){
      if(soundFile != null){
        GetComponent<AudioSource>().PlayOneShot(soundFile, soundVolume);
      }
    }

    IEnumerator slowCheck(){
      while(true){
        if(!pickedUp && !midair){
          targetLocCheck();
        }
        yield return new WaitForSeconds(0.5f);
      }
    }

    void targetLocCheck(){
      foreach(targetPoint tarpoint in targetPoints){
        if(Vector3.Distance(transform.position,tarpoint.TarPointPos) < tarpoint.TarPointRange){
          gameController.GetComponent<AchievementsControl>().AchievementUpdate (AchCode, AchIncrement);
  				gameController.GetComponent<TaskControl>().TaskProgress(TaskCode,TaskProgress);
          Instantiate (killEffect, transform.position - new Vector3 (0, -1, 0), Quaternion.Euler (-90, 0, 0));
          for (int i = 0; i < RewardAmount; i++) {
            //spawns rewards evenly spread out
            GameObject rewardspawn = (GameObject)Instantiate (RewardObject, transform.position - new Vector3 (0, -2, 0), Quaternion.Euler (0, 0, 0));
            rewardspawn.transform.eulerAngles = new Vector3 (0, ((360 / RewardAmount) * (i + 1)), 0);
            rewardspawn.GetComponent<Rigidbody>().velocity = rewardspawn.transform.forward * Random.Range(1,12) + Vector3.up * Random.Range(4,6);
          }
          Destroy (gameObject);
        }
      }
    }

    void OnCollisionEnter (Collision col){
      midair = false;
      if(throwable){
        targetLocCheck();
      }
      hideTargetAreas();
      Vector3 collisionForce = col.impulse / Time.fixedDeltaTime;
      if(collisionForce.magnitude > 200){
        publicSound(1.5f, soundThrow);
      }
    }


}


#if UNITY_EDITOR
[CustomEditor(typeof(pickup))]
public class pickup_Editor : Editor
{

	public void OnSceneGUI()
	{
		var LinkObj = target as pickup;

    if(LinkObj.targetPoints != null) {
		    drawPoints(LinkObj.targetPoints);
    }
	}

	public void drawPoints(targetPoint[] pointsToDraw){
		for(int i = 0; i < pointsToDraw.Length; i++){
			EditorGUI.BeginChangeCheck();
			Vector3 newPoint = Handles.PositionHandle(pointsToDraw[i].TarPointPos, Quaternion.identity);
      float newRange = Handles.RadiusHandle(Quaternion.identity, pointsToDraw[i].TarPointPos, pointsToDraw[i].TarPointRange);
			if (EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(target, "Update Target Location Point");
				pointsToDraw[i].TarPointPos = newPoint;
        pointsToDraw[i].TarPointRange = newRange;
			}
		}

	}

}
#endif // UNITY_EDITOR
