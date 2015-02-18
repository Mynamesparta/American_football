using UnityEngine;
using System.Collections;

public class Game_Controller : MonoBehaviour {
	public GameObject Player;
	public GameObject forTest;
	private Camera_controller cameracontroller;
	// Use this for initialization
	void Awake () 
	{
		Player.SendMessage("setBot",false);
		cameracontroller = GameObject.FindGameObjectWithTag(Tags.camera).GetComponent<Camera_controller>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetButton ("Test")) 
		{
			cameracontroller.SendMessage ("setCurrentObject",forTest);
			Player.SendMessage("setBot",true);
			forTest.SendMessage("setBot",false);
		}
	}
}
