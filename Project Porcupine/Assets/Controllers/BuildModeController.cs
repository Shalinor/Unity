using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class BuildModeController : MonoBehaviour {

	bool	 buildModeIsObjects = false;
	TileType buildModeTile = TileType.FLOOR;
	string	 buildModeObjectType;

	// Use this for initialization
	void Start () {

	}
	
/*	Vector3 CenterPointOffset( Vector3 vector )
	{
		// This offsets sprite positions to allow for center-point-pivots
		// as opposed to bottom-left-point-pivots.
		// It was from MouseController though so not sure it is needed here...
		return vector += new Vector3( 0.5f, 0.5f, 0 );
	}*/

	public void SetMode_BuildFloor()
	{
		buildModeIsObjects = false;
		buildModeTile = TileType.FLOOR;
	}

	public void SetMode_Bulldoze()
	{
		buildModeIsObjects = false;
		buildModeTile = TileType.EMPTY;
	}

	public void SetMode_BuildFurniture( string objectType )
	{
		// Wall is not a Tile! Wall is an "Furniture" that exists on TOP of a tile.
		buildModeIsObjects = true;
		buildModeObjectType = objectType;
	}

	public void DoBuild( Tile t )
	{
		if( buildModeIsObjects == true )
		{
			// Create the Furniture and assign it to the tile

			// FIXME: This instantly builds the furniture:
			//WorldController.Instance.World.PlaceFurniture( buildModeObjectType, t );

			// Can we build the furniture in the selected tile?
			// Run the ValidPlacement function

			string furnitureType = buildModeObjectType;

			if( WorldController.Instance.world.IsFurniturePlacementValid( furnitureType, t ) &&
				t.pendingFurnitureJob == null )
			{
				// This tile position is valid for this furniture
				// Create a job for it to be built
				Job j = new Job( t,
					(theJob) => { WorldController.Instance.world.PlaceFurniture( furnitureType, theJob.tile ); t.pendingFurnitureJob = null; },
					1f );

				// FIXME: I don't like having to manually and explicitly set
				// flags that prevent conflicts. It's too easy to forget to set/clear them!
				t.pendingFurnitureJob = j;
				j.RegisterJobCancelCallback( (theJob) => { theJob.tile.pendingFurnitureJob = null; } );

				// Add the job to the queue
				WorldController.Instance.world.jobQueue.Enqueue( j );
				Debug.Log("Job Queue Size: " + WorldController.Instance.world.jobQueue.Count);
			}
		}
		else
		{
			// We are in tile-changing mode
			t.Type = buildModeTile;
		}

	}
}
