using UnityEngine;
using System.Collections.Generic;
//using System.Linq; // Used to get the first key from a dictionary
using System;

public class WorldController : MonoBehaviour {

	public static WorldController Instance { get; protected set; }

	// The world and tile data
	public World world { get; protected set; }

	// Use this for initialization
	void OnEnable /*Start*/ () {	// Ensure that this runs before any other classes Start()
		if( Instance != null )
		{
			Debug.LogError("There should never be two world controllers.");
		}
		Instance = this;

		// Create a world with Empty tiles
		world = new World();

		// Center camera in the world
		Camera.main.transform.position = new Vector3( world.Width / 2, world.Height / 2, Camera.main.transform.position.z );//-10 );
	}

	/// <summary>
	/// Gets the tile at world-space coordinate.
	/// </summary>
	/// <returns>The tile at world coordinate.</returns>
	/// <param name="coord">Unity World-Space coordinates.</param>
	public Tile GetTileAtWorldCoord( Vector3 coord )
	{
		//		coord += new Vector3( 0.5f, 0.5f, 0 ); // Offsetting for center pivots vs bottom-left
		int x = Mathf.FloorToInt(coord.x);
		int y = Mathf.FloorToInt(coord.y);

		return world.GetTileAt(x, y);
	}
}
