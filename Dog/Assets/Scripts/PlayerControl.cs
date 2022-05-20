using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerControl : MonoBehaviour {

	//private int playerNumber = 1;			//Set player's number for networking
	private float speedx;					//Input's X value
	private float speedy;					//Input's Y value
	//private bool jumpkey = false;			//Is player pressing jump key
	private Vector2 inputvector;			//Vector of player's input X&Z;
	private Vector2 horizvelocity;
	private float vertvelocity = 0;
	private Vector3 totalvelocity;
	private Vector2 clamprun;
	public float maxrunspeed;
    private float runspeed;
    private bool hasJustLanded;
    public bool controlable;


	private float gravity = 30;
	public CharacterController controller;
    public Transform body;
    public Animator bodyAnimator;

	public void Update () {


        if (controlable)
        {
            speedx = Input.GetAxisRaw("Horizontal");
            speedy = Input.GetAxisRaw("Vertical");
            //jumpkey = Input.GetButton ("Jump");
        }

		inputvector = new Vector2(speedx,speedy);
		horizvelocity += (inputvector.normalized);

        if (controller.isGrounded)
        {
            vertvelocity = 0;

            if (!hasJustLanded)
            {
                //GetComponent<Player_Sound>().playSound(0);
                //print("You landed");
                hasJustLanded = true;
            }
            else
            {
                //hasJustLanded = false;
            }
        }
        else
        {
            hasJustLanded = false;
        }

        vertvelocity = Mathf.Lerp(vertvelocity, -gravity, Time.deltaTime * 2);


        if (Input.GetButtonDown("Jump") && controlable){
            if (controller.isGrounded)
            {
                vertvelocity = 22;
            }
        }

		ClampSpeed();
		totalvelocity = new Vector3(horizvelocity.x, vertvelocity, horizvelocity.y);

        controller.Move(totalvelocity * Time.deltaTime);

		if (speedx == 0 && speedy == 0)
		{
			horizvelocity = Vector2.MoveTowards(horizvelocity, Vector2.zero, 0.35f);
		}else{
			Quaternion targetrotation = Quaternion.LookRotation(new Vector3(horizvelocity.x,0,horizvelocity.y));
			transform.rotation = Quaternion.Slerp(transform.rotation, targetrotation, Time.deltaTime*17);
		}


		GetComponent<Animator>().SetFloat("Speed", (horizvelocity.magnitude/6.75f));
		GetComponent<Animator>().SetFloat("VertSpeed", (vertvelocity));
		GetComponent<Animator>().SetBool("Grounded", (controller.isGrounded));

    }

    void ClampSpeed(){
		clamprun = horizvelocity;
		clamprun = Vector2.ClampMagnitude(clamprun,maxrunspeed);
		horizvelocity.x = clamprun.x;
		horizvelocity.y = clamprun.y;
	}



}
