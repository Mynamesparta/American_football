using UnityEngine;
using System.Collections;

[System.Serializable]
public enum State_of_Player {Normal,Jump,Pass};

public class PlayerMovement : MonoBehaviour {
	public float turnSmoothing = 1f;   // A smoothing value for turning the player.
	public float speedDampTime = 0.5f;  // The damping for the speed parameter
	public float speedDampTimeJump=0.25f;//0.15f;
	public float maxSpeed =	8f;
	public float maxAngularSpeed = 5f;
	public float maxSpaceTimer = 1f;
	public Vector3 eulerAngleVelocity = new Vector3(0, 500, 0);
	public State_of_Player state=State_of_Player.Normal;
	public bool isBot=true;
	public bool have_ball=false;
	public float Forse;
	//public int maxForseWhenRun=5;
	public int mouseIndex=0;
	public Transform[] ball_position;

	private Animator anim;              // Reference to the animator component.
	private Hash_id hash;               // Reference to the HashIDs.
	private float speed;
	private float angular_speed;
	private float jumpspeed;
	private float timer;
	private BallController ball_con;
	private int indexBallpos=0;
	//private bool Take_Ball = false;
	
	void Awake ()
	{
		setBallPosition (1);
		// Setting up the references.
		anim = GetComponent<Animator>();
		hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<Hash_id>();
		ball_con = GameObject.FindGameObjectWithTag (Tags.ball).GetComponent<BallController> ();
		timer = 0;
		// Set the weight of the shouting layer to 1.
		anim.SetLayerWeight(1, 1f);
	}
	
	
	void FixedUpdate ()
	{
		// Cache the inputs.
		if(!isBot)
		{
			float rot = Input.GetAxis("Rotation");
			float fow = Input.GetAxis("Forward");
			MovementManagement(rot, fow);
		}
		else
		{
			anim.SetFloat (hash.float_speed, 0f, speedDampTime, Time.deltaTime);
			anim.SetFloat (hash.float_angular_speed, 0f, speedDampTime, Time.deltaTime);
			ChangePosition();
			ChangeRotation();
		}
		if(have_ball)
		{
			ball_con.SetPosition(ball_position[indexBallpos]);
			ball_con.SetRotation(transform);
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
			case State_of_Player.Normal:
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
				//=============================to=Jump=========
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
					state=State_of_Player.Jump;
				}
				//=============================to=Pass=========
				if(Input.GetMouseButtonDown(mouseIndex)&&anim.GetBool(hash.bool_ball))
				{
					state=State_of_Player.Pass;
					anim.SetBool(hash.bool_pass,true);
				}
				break;
			}
			case State_of_Player.Jump:
			{
				anim.SetFloat (hash.float_speed, jumpspeed, speedDampTimeJump, Time.deltaTime);
				anim.SetFloat (hash.float_angular_speed, 0f, speedDampTime, Time.deltaTime);
				//print(anim.GetFloat(hash.float_speed));
				ChangePosition();
				ChangeRotation();
				break;
			}
			case State_of_Player.Pass:
			{
				anim.SetFloat (hash.float_speed, 0f, speedDampTime, Time.deltaTime);
				anim.SetFloat (hash.float_angular_speed, 0f, speedDampTime, Time.deltaTime);
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
	void setBot(bool setbot)
	{
		isBot = setbot;
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
		if (Input.GetButton("Jump")) 
		{
			anim.SetFloat (hash.float_speed,1f);
			anim.SetInteger(hash.int_jump,2);
		}
		//Input.getB
	}
	void endJump()
	{
		state=State_of_Player.Normal;
		anim.SetInteger (hash.int_jump, 0);
	}
	//======================Pass===================================
	private int IndexOfPassForse=0;
	void isTimetoPass()
	{
		IndexOfPassForse++;
		//print ("isTimetoPass: IndexOfPassForse=" + IndexOfPassForse);
		if(!Input.GetMouseButton(mouseIndex))
			anim.SetBool(hash.bool_pass,false);
		/*/
		else
			if(IndexOfPassForse>=maxForseWhenRun&&state!=State_of_Player.Pass)
				anim.SetBool(hash.bool_pass,false);
		/*/
	}
	void Pass()
	{
		ball_con.Pass(IndexOfPassForse*Forse);
		IndexOfPassForse = 0;
		//print ("Pass");
		have_ball = false;
		anim.SetBool(hash.bool_ball, false);
		anim.SetBool(hash.bool_pass,false);
	}
	void endPass()
	{
		//print ("End Pass");
		state = State_of_Player.Normal;
	}
	//===============================================================
	void OnTriggerStay(Collider other)
	{
			//print ("onTriggerStay:"+other.name);
		if(!have_ball&&other.tag==Tags.ball&&ball_con.getState()!=State_of_Ball.Player&&Input.GetMouseButton(1))
		{
			ball_con.Take();
			have_ball=true;
			anim.SetBool(hash.bool_ball, true);
		}
	}
	void setBallPosition(int Index)
	{
		if(Index>=0&&Index<ball_position.Length)
			indexBallpos=Index;
		else
			print ("Error function setBallPosition in PlayerMovement.cs Index? ");
	}
	public Transform takeBallTransform(int Index)
	{
		if(Index>=0&&Index<ball_position.Length)
			return ball_position[Index];
		else
			return null;
	}
}
