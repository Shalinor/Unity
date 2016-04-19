using UnityEngine;
using System.Collections.Generic;
//using System.Linq; // Used to get the first key from a dictionary
using System;

public class WorldController : MonoBehaviour {

	public static WorldController Instance { get; protected set; }

	// FIXME
	public Sprite wallSprite;
	public Sprite floorSprite;

	Dictionary<Tile, GameObject> tileGameObjectMap;
	Dictionary<InstalledObject, GameObject> installedObjectGameObjectMap;

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

		World.RegisterInstalledObjectCreated( OnInstalledObjectCreated );

		// Instantiate our dictionary that tracks which GameObject is rendering which Tile data.
		tileGameObjectMap = new Dictionary<Tile, GameObject>();

		// Instantiate our dictionary that tracks which GameObject is rendering which InstalledObject data.
		installedObjectGameObjectMap = new Dictionary<InstalledObject, GameObject>();

		// Create a GameObject for each of our tiles, so they show visually.
		for (int x = 0; x < World.Width; x++) {
			for (int y = 0; y < World.Height; y++) {
				Tile tile_data = World.GetTileAt(x,y);

				// This creates a new GameObject and adds it to our scene.
				GameObject tile_go = new GameObject();

				// Add our tile/GO pair to the dictionary
				tileGameObjectMap.Add( tile_data, tile_go );

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
//				Action<Tile> myLambda = (tile) => { OnTileTypeChanged( tile, tile_go ); };	// This should not be a local variable - it will not be available for future Unregister...
//				tile_data.RegisterTileTypeChangedCallback( myLambda/*(tile) => { OnTileTypeChanged( tile, tile_go ); }*/ );

				// Register our callback so our GameObject gets updated whenever the tile's type changes
				tile_data.RegisterTileTypeChangedCallback( OnTileTypeChanged );
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

/*	// THIS IS AN EXAMPLE -- NOT CURRENTLY USED
	void DestroyAllTileGameObjects()
	{
		// This function might get called when we are changing floors/levels.
		// We need to destroy all visual **GameObjects** -- but not the actual tile data!

		while( tileGameObjectMap.Count > 0 )
		{
			Tile tile_data = tileGameObjectMap.Keys.First();
			GameObject tile_go = tileGameObjectMap[tile_data];

			// Remove the pair from the map
			tileGameObjectMap.Remove(tile_data);

			// Unregister the callback
			tile_data.UnRegisterTileTypeChangedCallback( OnTileTypeChanged );

			// Destroy the visual GameObject
			Destroy( tile_go );
		}

		// Presumably, after this function gets called, we'd be calling another
		// function to build all the GameObjects for the tiles on the new floor/level
	}*/

	void OnTileTypeChanged( Tile tile_data/*, GameObject tile_go*/ ){

		if( tileGameObjectMap.ContainsKey(tile_data) == false )
		{
			Debug.LogError("tileGameObjectMap doesn't contain the tile_data -- did you forget to add the tile to the dictionary? Or maybe forgot to unregister a callback?");
			return;
		}

		GameObject tile_go = tileGameObjectMap[tile_data];

		if( tile_go == null )
		{
			Debug.LogError("tileGameObjectMap' doesn't contain the tile_data's returned GameObject is null -- did you forget to add the tile to the dictionary? Or maybe forgot to unregister a callback?");
			return;
		}

		if(tile_data.Type == TileType.FLOOR) {
			tile_go.GetComponent<SpriteRenderer>().sprite = floorSprite;
		}
		else if( tile_data.Type == TileType.EMPTY ){
			tile_go.GetComponent<SpriteRenderer>().sprite = null;
		}
		else{
			Debug.LogError("WorldController.cs_OnTileTypeChanged - Unrecognised tile type");
		}
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

		return World.GetTileAt(x, y);
	}

	public void OnInstalledObjectCreated( InstalledObject obj )
	{
		//Debug.Log("OnInstalledObjectCreated");
		// Create a visual GameObject linked to this data.

		// FIXME: Does not consider multi-tile objects nor rotated objects

		// This creates a new GameObject and adds it to our scene.
		GameObject obj_go = new GameObject();

		// Add our tile/GO pair to the dictionary
		installedObjectGameObjectMap.Add( obj, obj_go );

		obj_go.name = obj.ObjectType + "_" + obj.tile.X + "_" + obj.tile.Y;
		obj_go.transform.position = new Vector3( obj.tile.X, obj.tile.Y, 0 );
		obj_go.transform.SetParent(this.transform, true);

		// FIXME: We assume that the object must be a single wall, so use the hardcoded reference to the wall sprite.
		obj_go.AddComponent<SpriteRenderer>().sprite = wallSprite;	// FIXME

		// Register our callback so our GameObject gets updated whenever the object's info changes
		obj.RegisterOnChangedCallback( OnInstalledObjectChanged );
	}

	void OnInstalledObjectChanged( InstalledObject obj )
	{
		Debug.LogError("OnInstalledObjectChanged -- NOT IMPLEMENTED");
	}
}
