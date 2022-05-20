using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.AI;

public class TouchMovement : MonoBehaviour
{

[Header("Master Objects & References")]
  public GameObject headObject;
  public GameObject gameManager;
  private Camera mainCamera;

[Header("Tap effects & Emotes")]
  public GameObject TapMoveEffect;
  public GameObject TapPickupEffect;
  public GameObject BarkEffect;
  public GameObject EmoteEffect;
  private SpriteRenderer m_SpriteRenderer;
  public ParticleSystem RunningDustEffect;

[Header("Throw effect")]
  public GameObject ThrowDisplay;
  public Renderer ThrowRender;
  //public Renderer ThrowRenderUnderside;

[Header("Key Bools")]
  private Animator anim;
  private UnityEngine.AI.NavMeshAgent navMeshAgent;
  public bool moveable = true;
  //Rotating Values
  public bool rotating;
  private float rotationtimer;
  //Walking Values
  public bool walking;
  private Vector3 walkTargetPos;
  //was initial touch on UI
  public bool uIClick = false;
  public bool uILockout = false;

  //Idle Effects
  private bool idle;
  private int idleint;
  private float idlecount;

  //Pickup Objects Values
  //private float grabDistance = 2;
  private bool pickupClicked = false;
  public GameObject targetObject;
  //private Transform targetedPickup;
  private bool holdingPickup;
  private float throwVelocity;
  private Vector3 distanceFromTarget;

  private bool engagedNPC = false;
  private Quaternion lookRot;
  private Vector3 lookdir;

  //Interactable Objects Values
  public bool interacting;
  //private bool interactableClicked;
  //private Transform targetInteractable;
  public GameObject currentInteractable;
  private Interactable InteractScript;

  //Jumping Values
  public bool jumping = false;
  private Vector3 jumpTargetPos;
  private Vector3 jumpStartPos;
  private Vector3 jumpCurrentPos;
  private float timeJumping;

  private NavMeshPath pathtest;

  //public GameObject cubetarget;  <----- DELETE IF NOT NEEDED OR RENAME
[Header("Audio")]
  public AudioClip stepsound;
  public AudioClip tapMoveSound;
  public AudioClip landThudSound;
  public AudioClip barkSound;

  void Awake () {
    anim = GetComponent<Animator> ();
    navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    //anim.SetBool ("Pickup", false);
    mainCamera = Camera.main;
  }

    void Update()
    {
//print(throwVelocity);

      if (Input.GetMouseButtonDown(0)){
        if (EventSystem.current.IsPointerOverGameObject() || EventSystem.current.currentSelectedGameObject != null || IsPointerOverUIObject()){
    			uIClick = true;
          //print("UI CLICK");
    		}
      }

      if (moveable && !uIClick && Input.GetMouseButton(0)){
        rotationtimer += Time.deltaTime;
        if (!rotating && rotationtimer > 0.5f && navMeshAgent.velocity.magnitude < 0.1f) {
          rotating = true;
          anim.SetBool ("Rotating", true);
        }
        if(rotating){
          RotandThrow();
        }
      }

      if (Input.GetMouseButtonUp(0)) {
        if(moveable && !rotating && !uIClick && !jumping){
            WorldRaycast();
        }

        if(holdingPickup && throwVelocity > 4){
          anim.SetTrigger ("Throw");
          print ("Throw " + throwVelocity);
        }
        rotating = false;
        rotationtimer = 0;
        uIClick = false;
        anim.SetBool ("Rotating", false);
        ThrowRender.enabled = false;
        //ThrowRenderUnderside.enabled = false;
      }

      if(jumping) {
  			lookdir = (walkTargetPos - transform.position);
  			lookdir.y = 0;
  			if (lookdir != Vector3.zero) {
  					lookRot = Quaternion.LookRotation (lookdir);
  			}
  			transform.rotation = Quaternion.Lerp (transform.rotation, lookRot, Time.deltaTime * 7);
  		} else {

        if(targetObject != null && targetObject.CompareTag ("Interactable")){
          if(!walking){
            Vector3 targetDir = targetObject.transform.position - transform.position;
            targetDir.y = 0;
            Vector3 newDir = Vector3.RotateTowards (transform.forward, targetDir, Time.deltaTime * 8.0f, 0.0f);
            transform.rotation = Quaternion.LookRotation (newDir);
          }
          if(Vector3.Distance(navMeshAgent.destination, headObject.transform.position)< targetObject.GetComponent<Interactable>().radius + 0.7f){
              currentInteractable = targetObject;
              PlayerJump(targetObject.transform.position + InteractScript.posoffset,false,false);
              CancelInvoke("ClosestEdge");
          }
        }

        if(targetObject != null && targetObject.CompareTag ("Pickup")){
          DropEverything();
          if(Vector3.Distance(navMeshAgent.destination, headObject.transform.position)< 2.6f && !jumping && !interacting){
            print("PICKUP");
            targetObject.gameObject.GetComponent<PickupControl>().PlayerPickup(headObject);
            anim.SetTrigger ("Pickup");
            targetObject = null;
            holdingPickup = true;
          }
        }

        if(targetObject != null && targetObject.CompareTag ("NPC")){
          if(Vector3.Distance(transform.position, targetObject.transform.position) < 9.0f){
            walking = false;
            navMeshAgent.isStopped = true;
            navMeshAgent.ResetPath();
            if(!engagedNPC){
              targetObject.GetComponent<NPC>().TriggerDialogue();
              engagedNPC = true;
            }
          }

          if(!walking){
            Vector3 targetDir = targetObject.transform.position - transform.position;
            targetDir.y = 0;
            Vector3 newDir = Vector3.RotateTowards (transform.forward, targetDir, Time.deltaTime * 8.0f, 0.0f);
            transform.rotation = Quaternion.LookRotation (newDir);
          }
        }
      }
      anim.SetFloat("Speed", (navMeshAgent.velocity.magnitude/13f + 1));

      if(!walking && navMeshAgent.velocity.magnitude > 0.1f){
        walking = true;
      }

      if(walking && navMeshAgent.velocity.magnitude < 0.01f){
        walking = false;
        if(Vector3.Distance(navMeshAgent.destination, headObject.transform.position)< 0.5f && targetObject == null){
          navMeshAgent.isStopped = true;
          navMeshAgent.ResetPath();
        }
      }

      if(walking && !RunningDustEffect.isPlaying && !jumping)
      RunningDustEffect.Play();

      if(jumping)
      RunningDustEffect.Stop();

      if(!walking && RunningDustEffect.isPlaying)
      RunningDustEffect.Stop();

      Debug.DrawRay(walkTargetPos, Vector3.up * 2, Color.red);
      Debug.DrawRay(navMeshAgent.destination, Vector3.up, Color.blue);
      Debug.DrawRay(jumpTargetPos, Vector3.up, Color.green);

      if(pathtest != null){
        for (int i = 0; i < pathtest.corners.Length - 1; i++)
          Debug.DrawLine(pathtest.corners[i], pathtest.corners[i + 1], Color.blue);
      }

      if(walking || jumping || rotating){
        idle = false;
      }else{
        idle = true;
      }

      if(idle) {
        idlecount -= 0.1f;
        if(idlecount < 0) {
          idleint = Random.Range(1,4);
          anim.SetInteger("Idle Int", idleint);
  				anim.SetTrigger("Idle Trigger");
  				idlecount = Random.Range(25,35);
        }
      }

    }

    private bool IsPointerOverUIObject() {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    private void RotandThrow(){
      Ray aimray = Camera.main.ScreenPointToRay (Input.mousePosition + new Vector3(0,Screen.height/10,0));
      Plane hPlane = new Plane (Vector3.up, transform.position);
      float distance = 0;
      if (hPlane.Raycast (aimray, out distance)){
        Vector3 aimhit = aimray.GetPoint(distance);
        Vector3 targetDir = aimhit - transform.position;
        targetDir.y = 0;
        Vector3 newDir = Vector3.RotateTowards (transform.forward, targetDir, Time.deltaTime * 8.0f, 0.0f);
        transform.rotation = Quaternion.LookRotation (newDir);

        if(holdingPickup){
          ThrowDisplay.transform.position = headObject.transform.position + (headObject.transform.forward * 0.75f);
          ThrowDisplay.transform.rotation = transform.rotation;

          throwVelocity = Vector3.Distance (aimhit, transform.position);
          throwVelocity = Mathf.Clamp (throwVelocity, 0, 14.5f);
          ThrowDisplay.transform.localScale = new Vector3 (5, 8, throwVelocity * 0.77f);
          if (throwVelocity > 4) {
              ThrowRender.enabled = true;
              //ThrowRenderUnderside.enabled = true;
            } else {
              ThrowRender.enabled = false;
              //ThrowRenderUnderside.enabled = false;
          }
        }

      }
    }

    public void WorldRaycast(){
      //Ignore UI touch

      Ray ray = mainCamera.ScreenPointToRay (Input.mousePosition);
      RaycastHit hit;

      if (!rotating) {
        int layerMask = LayerMask.GetMask ("Player" , "Objects", "Interactables", "NPC");
        if (Physics.Raycast (ray, out hit, 100, layerMask)){

          targetObject = hit.collider.gameObject;
          //User taps dog while he's not interacting with an object;
          if(targetObject.CompareTag ("Player") && !interacting) {
						Barking ();
						DropEverything ();
					}

          GoToDestination(targetObject.transform.position);

          if(targetObject.CompareTag ("Pickup")){
            DropEverything();
            targetObject = hit.collider.gameObject;
						pickupClicked = true;

            targetObject.GetComponent<HighlightEffect>().pubFlashCall();

            if(interacting){
              StartCoroutine(CalcPathObtoOb(currentInteractable, targetObject, true));
            }
            //Instantiate (TapPickupEffect, walkTargetPos, Quaternion.Euler (90, 0, 0));
          }

          if(targetObject.CompareTag ("Interactable") && targetObject != currentInteractable){

            publicSound(2, tapMoveSound);
            InvokeRepeating ("ClosestEdge", 0, 0.25f);

            targetObject.GetComponent<HighlightEffect>().pubPulseFlashCall();


            if(interacting){
              StartCoroutine(CalcPathObtoOb(currentInteractable, targetObject, false));
            }
            InteractScript = targetObject.GetComponent<Interactable>();
          }

          if(targetObject.CompareTag ("NPC")){
            engagedNPC = false;
            //print(Vector3.Distance(transform.position, targetObject.transform.position));
          }
          //If raycast hits no player, objects, interactables or NPCs
        } else {
          //clear targets
          targetObject = null;
          CancelInvoke("ClosestEdge");

          int terrainlayerMask = LayerMask.GetMask ("Terrain");
					if (Physics.Raycast (ray, out hit, 100, terrainlayerMask)) {
            targetObject = GetClosestTarget (hit.point, GameObject.FindGameObjectsWithTag ("Pickup"));
            if (targetObject != null) {
              DropEverything ();
              pickupClicked = true;
              targetObject.GetComponent<HighlightEffect>().pubFlashCall();

              //walkTargetPos = targetObject.transform.position;
              //GoToDestination(targetObject.transform.position);

              if(interacting){
                StartCoroutine(CalcPathObtoOb(currentInteractable, targetObject, true));
              } else {
                GoToDestination(targetObject.transform.position);
              }

              //Instantiate (TapPickupEffect, walkTargetPos, Quaternion.Euler (90, 0, 0));
              publicSound(2, tapMoveSound);
              return;
            }

            if(interacting){
              StartCoroutine(CalcPathNoObs(currentInteractable, hit.point));
            } else {
              GoToDestination(hit.point);
            }
            Instantiate (TapMoveEffect, hit.point, Quaternion.Euler (90, 0, 0));
            publicSound(1, tapMoveSound);
          }
        }
      }
    }

    IEnumerator CalcPathNoObs(GameObject CurrentObj, Vector3 destVector)
    {
      CurrentObj.GetComponent<NavMeshObstacle>().carving = false;

      //Wait a frame for NavMesh to update
      yield return null;

      CurrentObj.GetComponent<NavMeshObstacle>().carving = true;
      navMeshAgent.Warp(transform.position);
      NavMeshPath path = new NavMeshPath();
      navMeshAgent.CalculatePath(destVector, path);
      walkTargetPos = destVector;
      PlayerJump(Vector3.MoveTowards(path.corners[0],path.corners[1],CurrentObj.GetComponent<Interactable>().radius + 2.2f),true,false);
    }

    IEnumerator CalcPathObtoOb(GameObject CurrentObj, GameObject destObj, bool isPickup)
    {
      CurrentObj.GetComponent<NavMeshObstacle>().carving = false;
      if(!isPickup)
      destObj.GetComponent<NavMeshObstacle>().carving = false;
      //Wait a frame for NavMesh to update
      yield return null;

      CurrentObj.GetComponent<NavMeshObstacle>().carving = true;
      if(!isPickup)
      destObj.GetComponent<NavMeshObstacle>().carving = true;

      navMeshAgent.Warp(transform.position);
      NavMeshPath path = new NavMeshPath();
      navMeshAgent.CalculatePath(destObj.transform.position, path);
      walkTargetPos = destObj.transform.position;
      PlayerJump(Vector3.MoveTowards(path.corners[0],path.corners[1],CurrentObj.GetComponent<Interactable>().radius + 2.2f),true,true);
    }

    public void GoToDestination(Vector3 destination){
      walking = true;
      walkTargetPos = destination;
      navMeshAgent.destination = walkTargetPos;
    }

    void ClosestEdge(){
      navMeshAgent.destination = Vector3.MoveTowards(walkTargetPos,transform.position,0.25f);
    }

    //Find closest target to point
    GameObject GetClosestTarget (Vector3 pointpos, GameObject[] targets) {
      GameObject bestTarget = null;
      float closestDistanceSqr = Mathf.Infinity;
      //Vector3 currentPosition = transform.position;
      foreach(GameObject potentialTarget in targets) {
        Vector3 directionToTarget = potentialTarget.transform.position - pointpos;
        float dSqrToTarget = directionToTarget.sqrMagnitude;
        if(dSqrToTarget < closestDistanceSqr)	{
          closestDistanceSqr = dSqrToTarget;
          bestTarget = potentialTarget.gameObject;
        }
      }
      //cut off by distance.
      if (closestDistanceSqr > 5f) {
        bestTarget = null;
      }
      return bestTarget;
    }

    private void PlayerJump(Vector3 JumpTarget, bool toGround, bool objDest){
      jumpStartPos = transform.position;
      jumpTargetPos = JumpTarget;
      if(!objDest){
        targetObject = null;
      }
      transform.parent = null;
      interacting = false;
      jumping = true;
      walking = false;
      if(!toGround){
          navMeshAgent.updatePosition = false;
          navMeshAgent.isStopped = true;
      }
      currentInteractable.GetComponent<Interactable>().interacting = false;
      StartCoroutine(Jumping(toGround));

      anim.SetBool ("Jumping", true);
    }

    private IEnumerator Jumping(bool toGround)
    {
      timeJumping = 0;
      while(timeJumping < 1.0f){
        jumpCurrentPos = Vector3.Lerp(jumpStartPos, jumpTargetPos, timeJumping);
        jumpCurrentPos.y = jumpCurrentPos.y + Mathf.Sin(Mathf.PI * timeJumping) * 2.6f;

        transform.position = jumpCurrentPos;

        if(timeJumping > 0.6f) {
          anim.SetBool ("Jumping", false);
        }

        timeJumping += 0.045f;
        yield return null;
      }
      if(timeJumping >= 1.0f){
        PlayerLand(toGround);
        jumping = false;
        anim.SetBool ("Jumping", false);
      }
    }

    private void PlayerLand(bool toGround){
      if(toGround){
        navMeshAgent.updatePosition = true;
        navMeshAgent.Warp(transform.position);
        GoToDestination(walkTargetPos);
        currentInteractable = null;

        publicSound(3,landThudSound);
      } else {
        interacting = true;
        InteractScript.PlayerLanding(transform.forward);
        if(InteractScript.parentForPlayer != null){
          transform.SetParent(InteractScript.parentForPlayer.transform);
        }
      }
    }

    public void DropEverything ()
    {
      if (!holdingPickup) {
        return;
      }
      Transform[] allChildren = headObject.GetComponentsInChildren<Transform>();
      foreach (Transform child in allChildren)
      {
        if (child.tag == "Pickup")
        {
          child.gameObject.GetComponent<PickupControl>().PlayerDrop (navMeshAgent.velocity + navMeshAgent.transform.forward * 1.5f);
        }
      }
      anim.SetTrigger ("Drop");
      holdingPickup = false;
    }

    public void ThrowEverything () {
      Transform[] allChildren = headObject.GetComponentsInChildren<Transform>();
      foreach (Transform child in allChildren)
      {
        if (child.tag == "Pickup")
        {
          print(throwVelocity);
          child.gameObject.GetComponent<PickupControl> ().PlayerDrop (navMeshAgent.transform.forward * throwVelocity * 1.25f + Vector3.up * 22f);
        }
      }
      holdingPickup = false;
      throwVelocity = 0;
    }

    public void Barking (){
      foreach (Transform child in headObject.transform)
        if (child.CompareTag ("Pickup")) {return;}
      anim.SetTrigger ("Bark");

      publicSound(0.5f,barkSound);

      GameObject barkclone = Instantiate (BarkEffect, headObject.transform.position, Quaternion.LookRotation(Vector3.forward));
      barkclone.transform.SetParent (headObject.transform);
      barkclone.transform.localPosition = new Vector3 (0, 0, 0.85f);
      barkclone.transform.localEulerAngles = new Vector3 (-90, 10, 90);
      //barkclone.transform.Rotation =  Quaternion.LookRotation (new Vector3(0,0,0));

      GameObject emoteclone = Instantiate (EmoteEffect, headObject.transform.position, Quaternion.LookRotation(Vector3.forward));
      emoteclone.transform.SetParent (headObject.transform);
      emoteclone.transform.localPosition = new Vector3 (0, 0.85f, 1.25f);
      if(transform.rotation.eulerAngles.y > 0 && transform.rotation.eulerAngles.y < 180){
        m_SpriteRenderer = emoteclone.GetComponent<SpriteRenderer>();
        m_SpriteRenderer.flipX = true;
      }

      anim.SetTrigger ("Bark");
      gameManager.GetComponent<AchievementsControl>().AchievementUpdate (4, 1);
    }

    public void Footstep()
		{
				//GetComponent<AudioSource>().volume = 0.3f + Random.Range(-0.05f, 0.05f);
				//GetComponent<AudioSource>().pitch = 1.1f + Random.Range(-0.1f,0.1f);
				GetComponent<AudioSource>().PlayOneShot(stepsound, 2);
		}

    public void publicSound(float soundVolume, AudioClip soundFile){
      //GetComponent<AudioSource>().volume = soundVolume;
      //GetComponent<AudioSource>().pitch = soundPitch;
      GetComponent<AudioSource>().PlayOneShot(soundFile, soundVolume);
    }
}
