﻿using UnityEngine;
using System.Collections;

public class WorldController : MonoBehaviour {

	public static WorldController Instance { get; protected set; }

	public Sprite floorSprite;

	// The world and tile data
	public World World { get; protected set; }

	// Use this for initialization
	void Start () {
		if( Instance != null )
		{
			Debug.LogError("There should never be two world controllers.");
		}
		Instance = this;

		// Create a world with Empty tiles
		World = new World();

		// Create a GameObject for each of our tiles, so they show visually.
		for (int x = 0; x < World.Width; x++) {
			for (int y = 0; y < World.Height; y++) {
				Tile tile_data = World.GetTileAt(x,y);

				GameObject tile_go = new GameObject();
				tile_go.name = "Tile_"+x+"_"+y;
				tile_go.transform.position = new Vector3( tile_data.X, tile_data.Y, 0 );
				tile_go.transform.SetParent(this.transform, true);

				// Add a sprite renderer, but don't bother setting a sprite
				// because all the tiles are empty right now.
				tile_go.AddComponent<SpriteRenderer>();

				// Register OnTileTypeChanged to tile_data's callback - using a Lambda ( () => {} )to insert
				// the gameobject that OnTileTypeChanged requires and we know about, but the tile doesn't
				// So the callback sends 'tile', which is then passed into the Lambda commands and used in the
				// call to OnTileTypeChanged
				tile_data.RegisterTileTypeChangedCallback( (tile) => { OnTileTypeChanged( tile, tile_go ); } );
			}
		}

		World.RandomizeTiles();

		// Center camera in the world
		Camera.main.transform.position = new Vector3( World.Width / 2, World.Height / 2, -10 );
	}

//	float randomiseTileTimer = 2f;

	// Update is called once per frame
	void Update () {
//		randomiseTileTimer -= Time.deltaTime;
//
//		if( randomiseTileTimer < 0 )
//		{
//			world.RandomizeTiles();
//			randomiseTileTimer = 2f;
//		}
	}

	void OnTileTypeChanged( Tile tile_data, GameObject tile_go ){
		
		if(tile_data.Type == Tile.TileType.Floor) {
			tile_go.GetComponent<SpriteRenderer>().sprite = floorSprite;
		}
		else if( tile_data.Type == Tile.TileType.Empty ){
			tile_go.GetComponent<SpriteRenderer>().sprite = null;
		}
		else{
			Debug.LogError("WorldController.cs_OnTileTypeChanged - Unrecognised tile type");
		}
	}

	public Tile GetTileAtWorldCoord( Vector3 coord )
	{
		//		coord += new Vector3( 0.5f, 0.5f, 0 ); // Offsetting for center pivots vs bottom-left
		int x = Mathf.FloorToInt(coord.x);
		int y = Mathf.FloorToInt(coord.y);

		return World.GetTileAt(x, y);
	}
}
