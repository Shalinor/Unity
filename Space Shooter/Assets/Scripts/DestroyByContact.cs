﻿using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour
{
	public GameObject explosion;
	public GameObject playerExplosion;
	public int scoreValue;

	private GameController gameController;

	void Start()
	{
		GameObject gameControllerObject = GameObject.FindWithTag("GameController");

		if(gameControllerObject != null)
		{
			gameController = gameControllerObject.GetComponent<GameController>();
		}

		if(gameController == null)
		{
			Debug.Log("Cannot find 'GameController' script");
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Boundary"))//(other.tag == "Boundary")
		{
			return;
		}

		//Debug.Log(other.name);
		Instantiate(explosion, transform.position, transform.rotation);

		if(other.tag == "Player")
		{
			Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
			gameController.GameOver();
		}

		if(other.tag == "Bolt" || other.tag == "Player")
		{
			gameController.AddScore(scoreValue);
		}

		Destroy(other.gameObject);
		Destroy(gameObject);
	}
}