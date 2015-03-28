using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

	public float thrust;
//	private Rigidbody rigidbody;

	void Start() {
//		rigidbody = GetComponent<Rigidbody>();

		GetComponent<Rigidbody>().AddForce(transform.forward * thrust);
	}

	void FixedUpdate() {
	}
}
