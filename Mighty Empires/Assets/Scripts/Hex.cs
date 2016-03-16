using UnityEngine;
using System.Collections;

public class Hex : MonoBehaviour {

	// Our coordinates in the map array.
	public int x;
	public int z;

	public Hex[] GetNeighbours()
	{
		// So if we are at x, z -- the neighbour to our left is at x-1, z

		GameObject leftNeighbour = GameObject.Find("Hex_" + (x-1) + "_" + z);

		// Right neighbour is also easy to find.
		GameObject rightNeighbour = GameObject.Find("Hex_" + (x+1) + "_" + z);

		// The problem is that our neighbours to our upper-left and upper-right
		// might be at x-1 and x, OR they might be at x and x+1, depending
		// on whether we ar on an even or odd row (i.e. if z % 2 is 0 or 1)

		return null;

	}

}
