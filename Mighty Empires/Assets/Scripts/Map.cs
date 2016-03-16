using UnityEngine;
using System.Collections;

public class Map : MonoBehaviour {

	public GameObject hexPrefab;

	// Size of the map in terms of the number of hex tiles
	// This is NOT representative of the amount of
	// world space that we're going to take up.
	// (i.e. our tiles might be more or less than 1 Unity World Unit)

	//Largest map size on Civ 5 (un-modded) 128x80
	int width = 64;
	int height = 40;

	float xOffSet = 0.882f;
	float zOffSet = 0.764f;

	// Use this for initialization
	void Start () {
		float xPos;

		for (int x = 0; x < width; x++) {
			for (int z = 0; z < height; z++) {	// Using z instead of y for the up/down as we will be looking "down" on the map

				xPos = x * xOffSet;

				// Are we on an odd row?
				/*if(z % 2 == 1)
				{
					xPos += xOffSet / 2f;
				}*/
				xPos += (z % 2) * xOffSet / 2;

				GameObject hex_go = (GameObject)Instantiate(hexPrefab, new Vector3( xPos, 0, z * zOffSet ), Quaternion.identity);

				hex_go.name = "Hex_" + x + "_" + z;

				// Make sure the hex is aware of its place on the map
				hex_go.GetComponent<Hex>().x = x;
				hex_go.GetComponent<Hex>().z = z;

				// For a cleaner hierarchy, parent this hex to the map
				hex_go.transform.SetParent(this.transform);	// Adds hex as a child of the Map Object

				// Slight optimisation - if it makes any difference
				hex_go.isStatic = true;

			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
