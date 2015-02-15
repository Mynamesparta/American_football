using UnityEngine;
using System.Collections;

public class Hash_id : MonoBehaviour 
{
	public int bool_is_sit;
	public int float_speed;
	public int float_angular_speed;
	public int int_jump;
	public int Jump_1;
	void Awake ()
	{
		float_speed = Animator.StringToHash ("Speed");

		bool_is_sit = Animator.StringToHash ("Sit");

		float_angular_speed = Animator.StringToHash ("Angular_speed");

		int_jump = Animator.StringToHash ("Jump");

		Jump_1 = Animator.StringToHash ("Base Layer.Jump_1");
	}
}
