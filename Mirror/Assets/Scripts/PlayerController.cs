using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	//The basic Rotate functionality was easy to get to follow mouse position
	// but the RotateAround did not work nicely - worked on many examples I
	// found online, but none worked to my satisfaction.

	//Found the solution to this functionality here and slightly modified it to fit my req.
	//http://answers.unity3d.com/questions/794119/how-to-rotate-an-object-around-a-fixed-point-so-it.html

	public Transform center;
	private Vector3 v;
	private Vector3 centerScreenPos;
	
	
	void Start() {
		// Requires the block to be directly to the right of the center
		//   with rotation set correctly on start - I already had this, yay :)
		v = (transform.position - center.position);
		centerScreenPos = Camera.main.WorldToScreenPoint (center.position);
	}
	
	void Update(){
		//Vector3 centerScreenPos = Camera.main.WorldToScreenPoint (center.position);
		Vector3 dir = Input.mousePosition - centerScreenPos;
		float angle = Mathf.Atan2 (-dir.y, dir.x) * Mathf.Rad2Deg;
		Quaternion q = Quaternion.AngleAxis (angle, Vector3.up);//forward);
		transform.position = center.position + q * v;
		transform.rotation = q;
	}

}