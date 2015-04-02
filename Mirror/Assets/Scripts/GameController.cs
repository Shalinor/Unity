using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public GameObject target;
	public GameObject hazard;
	public Vector3 spawnValues;

	void Start()
	{
		SpawnBolt ();
	}

	void Update()
	{
		//For testing...
		if (Input.GetKeyDown (KeyCode.Space)) {
			SpawnBolt();
		}
	}

	void SpawnBolt()
	{
		/*
		 * x & z would be Random.Range(min, max);
		 * 
		 * Quaternion.identity == no rotation...
		 */
		Vector3 spawnPosition = new Vector3();

		//Random.Range (int, int) returns min - max-1, (float, float) returns min - max
		switch (Random.Range (1, 5)) {
		case 1:	//Top edge
			spawnPosition = new Vector3 (Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
			break;

		case 2: //Bottom edge
			spawnPosition = new Vector3 (Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, -spawnValues.z);
			break;

		case 3: //Left edge
			spawnPosition = new Vector3 (-spawnValues.x, spawnValues.y, Random.Range(-spawnValues.z, spawnValues.z));
			break;

		case 4: //Right edge
			spawnPosition = new Vector3 (spawnValues.x, spawnValues.y, Random.Range(-spawnValues.z, spawnValues.z));
			break;

		default:
			Debug.Log ("SpawnBolt's switch reached default...");
			break;
		}

		//Vector3 spawnPosition = new Vector3 (Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
		Quaternion spawnRotation = new Quaternion ();

		//spawnRotation.SetLookRotation (new Vector3 (0, 0, 0));

		GameObject bolt = Instantiate (hazard, spawnPosition, spawnRotation) as GameObject;

		bolt.transform.LookAt (target.transform);
	}
}
