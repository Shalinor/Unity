﻿using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour
{
	public float speed;

	//private Rigidbody rb;

	void Start ()
	{
		//rb = GetComponent<Rigidbody>();
		GetComponent<Rigidbody>().velocity = transform.forward * speed;
	}
}
