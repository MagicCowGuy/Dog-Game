using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class face_camera : MonoBehaviour {

    public GameObject SpriteObject;
    public GameObject ShadowObject;
    public GameObject PlayerObject;
    public Scoring ScoreManager;
    public AudioClip collectSound;
    public AudioSource playerAudioSource;

    private Vector3 PlayerPos;
    private Vector3 distancefromplayer;
    private bool collecting;
    private float speedmag;

	private int timealive;
	private float velocityspeed = 0f;
	private float maxvelspeed = 32f;
	private float rndoffset;

    public int coinValue;

	Rigidbody m_Rigidbody;

    // Use this for initialization
    void Start () {

        PlayerObject = GameObject.FindGameObjectWithTag("Player");
        playerAudioSource = PlayerObject.GetComponent<AudioSource>();
        ScoreManager = GameObject.Find("GameManagment").GetComponent<Scoring>();
		rndoffset = Random.Range (0.0f, 1.0f);

		//Fetch the Rigidbody component from the GameObject
		m_Rigidbody = GetComponent<Rigidbody>();
		//Ignore the collisions between layer 0 (default) and layer 8 (custom layer you set in Inspector window)
		Physics.IgnoreLayerCollision(9, 12);

    }

	// Update is called once per frame
	void Update () {

		timealive++;
    if (timealive > 50){
        PlayerPos = PlayerObject.transform.position + new Vector3(0, 1.25f, 0);
        distancefromplayer = PlayerPos - transform.position;

        if (distancefromplayer.magnitude < 6) { collecting = true; }
        if (distancefromplayer.magnitude < 0.25f) { Collect(); }

}

		if (timealive > 200) { collecting = true; }

        if (collecting) {
			m_Rigidbody.drag = 0;
			m_Rigidbody.angularDrag = 0;
			m_Rigidbody.useGravity = false;
            //speedmag = Mathf.Clamp((10f - distancefromplayer.magnitude), 0.5f, 10);
			transform.position = Vector3.MoveTowards(transform.position, PlayerPos, Time.deltaTime * velocityspeed);
			if(velocityspeed < maxvelspeed) { velocityspeed += 0.5f; }
        }

		SpriteObject.transform.localPosition = new Vector3(SpriteObject.transform.localPosition.x, Mathf.Sin((Time.time + rndoffset) * 6 ) * 0.25f, SpriteObject.transform.localPosition.z);

		SpriteObject.transform.LookAt (Camera.main.transform.position);
		ShadowObject.transform.eulerAngles = new Vector3(90, SpriteObject.transform.eulerAngles.y, 0);



    }

    void Collect ()
    {
      //ScoreManager.coinNumber += coinValue;
      ScoreManager.ScoreUpdate(coinValue);
      Destroy(this.gameObject);

      playerAudioSource.volume = 1.5f + Random.Range(-0.05f, 0.05f);
      playerAudioSource.pitch = 1.1f + Random.Range(-0.1f,0.1f);
      playerAudioSource.PlayOneShot(collectSound, 0.5f);
    }
}
