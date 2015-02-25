using UnityEngine;
using System.Collections;

public class Camera_controller : MonoBehaviour {
	public GameObject current_object;
	public float speed=2f;
	public float angularspeed=100f;
	public float Max_rot_up_down;
	public float Max_tor_left_right;
	public float Rad_Rot;
	public Camera mainCamera;
	private float startTime;
	private float journeyLength;
	private Vector3 last_mouse_position;
	private float rot_Left_Right=0,rot_Up_Down=0;
	private Quaternion next_rotation;
	void Start () 
	{
		last_mouse_position = Input.mousePosition;
		//offset = transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () 
	{
		Rotation_By_Mouse ();
		move();
		//transform.rotation = current_object.transform.rotation;
	}
	void setCurrentObject(GameObject _object)
	{
		current_object=_object;
		startTime = Time.time;
		journeyLength=Vector3.Distance (transform.position, current_object.transform.position);
	}
	void move()
	{
		if (Vector3.Distance (transform.position, current_object.transform.position) < speed * Time.deltaTime) 
		{
			transform.position = current_object.transform.position;
		}
		else
		{
			transform.position = Vector3.Lerp(transform.position,current_object.transform.position,
			                                 (Time.time-startTime)*speed/journeyLength);
		}//current_object.transform.rotation | Quaternion.AngleAxis(rot_Left_Right,Vector3.up).y+
		//next_rotation= current_object.transform.rotation;
		next_rotation.Set (-rot_Up_Down * Mathf.Deg2Rad,rot_Left_Right * Mathf.Deg2Rad, 0f, mainCamera.transform.localRotation.w);
		//print (Quaternion.AngleAxis (rot_Left_Right, Vector3.up).y + "+" + current_object.transform.rotation.y);
		mainCamera.transform.localRotation = Quaternion.RotateTowards(mainCamera.transform.localRotation, next_rotation, angularspeed*Time.deltaTime);
		transform.rotation = Quaternion.RotateTowards(transform.rotation, current_object.transform.rotation, angularspeed*Time.deltaTime);
	}
	void Rotation_By_Mouse()
	{
		Vector3 current_mouse_position=Input.mousePosition;
		rot_Left_Right += (current_mouse_position.x - last_mouse_position.x) * Max_tor_left_right / Rad_Rot;
		rot_Up_Down += (current_mouse_position.y - last_mouse_position.y) * Max_rot_up_down / Rad_Rot;
		//=============normalization
		/*/
		rot_Left_Right = rot_Left_Right > Max_tor_left_right ? Max_tor_left_right : rot_Left_Right;
		rot_Left_Right = rot_Left_Right < -Max_tor_left_right ? -Max_tor_left_right : rot_Left_Right;
		rot_Up_Down = rot_Up_Down > Max_rot_up_down ? Max_rot_up_down : rot_Up_Down;
		rot_Up_Down = rot_Up_Down < -Max_rot_up_down ? -Max_rot_up_down : rot_Up_Down;
		/*/
		last_mouse_position = current_mouse_position;
		//print ("rot_Left_Right=" + rot_Left_Right+"|rot_Up_Down="+rot_Up_Down);
	}
}
