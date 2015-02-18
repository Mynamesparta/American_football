using UnityEngine;
using System.Collections;

[System.Serializable]
public enum State {Normal,Jump};

public class PlayerMovement : MonoBehaviour {
	public float turnSmoothing = 1f;   // A smoothing value for turning the player.
	public float speedDampTime = 0.5f;  // The damping for the speed parameter
	public float speedDampTimeJump=0.25f;//0.15f;
	public float maxSpeed =	8f;
	public float maxAngularSpeed = 5f;
	public float maxSpaceTimer = 1f;
	public Vector3 eulerAngleVelocity = new Vector3(0, 500, 0);
	public State state=State.Normal;
	public bool isBot=true;

	private Animator anim;              // Reference to the animator component.
	private Hash_id hash;               // Reference to the HashIDs.
	private float speed;
	private float angular_speed;
	private float jumpspeed;
	private float timer;
	
	void Awake ()
	{
		// Setting up the references.
		anim = GetComponent<Animator>();
		hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<Hash_id>();
		timer = 0;
		// Set the weight of the shouting layer to 1.
		//anim.SetLayerWeight(1, 1f);
	}
	
	
	void FixedUpdate ()
	{
		// Cache the inputs.
		if(!isBot)
		{
			float rot = Input.GetAxis("Rotation");
			float fow = Input.GetAxis("Forward");
			if(Input.GetButtonDown ("Jump"))
			{
				/*/
				timer+=Time.deltaTime;
				if(timer> maxSpaceTimer)
				{
				}
				/*/
				if(anim.GetInteger(hash.int_jump)==0)
					anim.SetInteger(hash.int_jump,1);
				state=State.Jump;
			}
			MovementManagement(rot, fow);
		}
		else
		{
			anim.SetFloat (hash.float_speed, 0f, speedDampTime, Time.deltaTime);
			anim.SetFloat (hash.float_angular_speed, 0f, speedDampTime, Time.deltaTime);
			ChangePosition();
			ChangeRotation();
		}
	}
	
	
	void Update ()
	{
		// Cache the attention attracting input.
		//bool shout = Input.GetButtonDown("Attract");
	}
	
	
	void MovementManagement (float rot, float fow)
	{
		//fow = Mathf.Sign (fow);
		//rot = Mathf.Sign (rot);
		switch(state)
		{
			case State.Normal:
			{
				ChangeRotation ();
				if (rot != 0f) //
				{
					anim.SetFloat (hash.float_angular_speed, rot*maxAngularSpeed, speedDampTime*0.5f, Time.deltaTime);
				}
				else
				{
					anim.SetFloat (hash.float_angular_speed, 0f, speedDampTime, Time.deltaTime);
				}
				if (fow != 0f) 
				{
					// ... set the players rotation and set the speed parameter to 5.5f.
					ChangePosition ();
					anim.SetFloat (hash.float_speed, fow*maxSpeed, speedDampTime, Time.deltaTime);
				} 
				else 
				{		
					ChangePosition ();
					anim.SetFloat (hash.float_speed, 0f, speedDampTime, Time.deltaTime);
				}
				break;
			}
			case State.Jump:
			{
				anim.SetFloat (hash.float_speed, jumpspeed, speedDampTimeJump, Time.deltaTime);
				anim.SetFloat (hash.float_angular_speed, 0f, speedDampTime, Time.deltaTime);
				print(anim.GetFloat(hash.float_speed));
				ChangePosition();
				ChangeRotation();
				break;
			}
		}
		
	}

	void ChangePosition ()
	{
		speed = anim.GetFloat (hash.float_speed)* Time.deltaTime;
		//
		//print ("rigidbody:"+rigidbody.rotation.y);
		float rot = (rigidbody.rotation.eulerAngles.y)* Mathf.Deg2Rad;
		rigidbody.MovePosition(new Vector3(rigidbody.position.x+speed*Mathf.Sin(rot) ,
		                                   rigidbody.position.y,
		                                   rigidbody.position.z+speed*Mathf.Cos(rot) ));
		//
		//print (rigidbody.rotation.y+" "+rot+" "+Mathf.Sin(rot) );
		//print (rigidbody.rotation.eulerAngles);
	}
	void ChangeRotation()
	{
		angular_speed=anim.GetFloat (hash.float_angular_speed)* Time.deltaTime;
		//Quaternion deltaRotation = Quaternion.Euler(angular_speed*eulerAngleVelocity * Time.deltaTime);
		//rigidbody.MoveRotation(rigidbody.rotation * deltaRotation);
		rigidbody.angularVelocity = new Vector3 (0f, angular_speed*10, 0f);
		//print (rigidbody.rotation.y);
	}
	//======================Jump==================================
	void setJumpSpeed(float jspeed)
	{
		if(anim.GetFloat (hash.float_speed)>1f)
			jumpspeed = jspeed;
		if(jspeed==2f)
			anim.SetFloat(hash.float_speed,2f);
	}
	void isSecondJump()
	{
		print ("second stage");
		if (Input.GetButton("Jump")) 
		{
			anim.SetFloat (hash.float_speed,1f);
			anim.SetInteger(hash.int_jump,2);
		}
		//Input.getB
	}
	void endJump()
	{
		state=State.Normal;
		anim.SetInteger (hash.int_jump, 0);
	}
	//=============================================================
	void setBot(bool setbot)
	{
		print ("Bot?" + setbot);
		isBot = setbot;
	}


}
