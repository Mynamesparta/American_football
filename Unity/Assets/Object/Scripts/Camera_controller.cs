using UnityEngine;
using System.Collections;

public class Camera_controller : MonoBehaviour {
	public GameObject current_object;
	public float speed=2f;
	public float angularspeed=100f;
	private Vector3 offset;
	private float startTime;
	private float journeyLength;
	void Start () 
	{
		offset = transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
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
		}
		transform.rotation = Quaternion.RotateTowards(transform.rotation, current_object.transform.rotation, angularspeed*Time.deltaTime);
	}
}
