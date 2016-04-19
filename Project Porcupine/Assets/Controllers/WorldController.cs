using UnityEngine;
using System.Collections.Generic;
//using System.Linq; // Used to get the first key from a dictionary
using System;

public class WorldController : MonoBehaviour {

	public static WorldController Instance { get; protected set; }

	// FIXME
	public Sprite floorSprite;
	public Sprite emptySprite;

	Dictionary<Tile, GameObject> tileGameObjectMap;
	Dictionary<Furniture, GameObject> furnitureGameObjectMap;

	Dictionary<string, Sprite> furnitureSprites;

	// The world and tile data
	public World World { get; protected set; }

	// Use this for initialization
	void Start () {
		if( Instance != null )
		{
			Debug.LogError("There should never be two world controllers.");
		}
		Instance = this;

		LoadSprites();

		// Create a world with Empty tiles
		World = new World();

		World.RegisterFurnitureCreated( OnFurnitureCreated );

		// Instantiate our dictionary that tracks which GameObject is rendering which Tile data.
		tileGameObjectMap = new Dictionary<Tile, GameObject>();

		// Instantiate our dictionary that tracks which GameObject is rendering which Furniture data.
		furnitureGameObjectMap = new Dictionary<Furniture, GameObject>();

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

				// Add a Sprite Renderer
				// Add a default sprite for empty tiles.
				tile_go.AddComponent<SpriteRenderer>().sprite = emptySprite;

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

		//World.RandomizeTiles();

		// Center camera in the world
		Camera.main.transform.position = new Vector3( World.Width / 2, World.Height / 2, Camera.main.transform.position.z );//-10 );
	}


	void LoadSprites()
	{
		furnitureSprites = new Dictionary<string, Sprite>();
		Sprite[] sprites = Resources.LoadAll<Sprite>("Images/Furniture/");

		foreach(Sprite s in sprites)
		{
			furnitureSprites[s.name] = s;
		}
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
			tile_go.GetComponent<SpriteRenderer>().sprite = emptySprite;//null;
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

	public void OnFurnitureCreated( Furniture furn )
	{
		//Debug.Log("OnFurnitureCreated");
		// Create a visual GameObject linked to this data.

		// FIXME: Does not consider multi-tile objects nor rotated objects

		// This creates a new GameObject and adds it to our scene.
		GameObject furn_go = new GameObject();

		// Add our tile/GO pair to the dictionary
		furnitureGameObjectMap.Add( furn, furn_go );

		furn_go.name = furn.FurnitureType + "_" + furn.tile.X + "_" + furn.tile.Y;
		furn_go.transform.position = new Vector3( furn.tile.X, furn.tile.Y, 0 );
		furn_go.transform.SetParent(this.transform, true);

		// FIXME: We assume that the object must be a single wall, so use the hardcoded reference to the wall sprite.
		furn_go.AddComponent<SpriteRenderer>().sprite = GetSpriteForFurniture(furn);

		// Register our callback so our GameObject gets updated whenever the object's info changes
		furn.RegisterOnChangedCallback( OnFurnitureChanged );
	}

	Sprite GetSpriteForFurniture( Furniture furn )
	{
		if( furn.LinksToNeighbour == false )
		{
			return furnitureSprites[furn.FurnitureType];
		}

		string spriteName = furn.FurnitureType;// + "_"; // Removed because I have hardcoded the initial '_', unlike Quill

		// Check for neighbours North, East, South, West

		int x = furn.tile.X;
		int y = furn.tile.Y;

		Tile t;

		t = World.GetTileAt(x, y + 1);
		if( t != null && t.furniture != null && t.furniture.FurnitureType == furn.FurnitureType )
		{
			spriteName += "N";
		}

		t = World.GetTileAt(x + 1, y);
		if( t != null && t.furniture != null && t.furniture.FurnitureType == furn.FurnitureType )
		{
			spriteName += "E";
		}

		t = World.GetTileAt(x, y - 1);
		if( t != null && t.furniture != null && t.furniture.FurnitureType == furn.FurnitureType )
		{
			spriteName += "S";
		}

		t = World.GetTileAt(x - 1, y);
		if( t != null && t.furniture != null && t.furniture.FurnitureType == furn.FurnitureType )
		{
			spriteName += "W";
		}

		// For example, if this object has all four neighbours of
		// the same tupe, then the string will look like:
		//           Wall_NESW

		if( furnitureSprites.ContainsKey(spriteName) == false )
		{
			Debug.LogError("WorldController.cs - GetSpriteForFurniture(Furniture obj) -- Attempted to display a sprite that has not been created/loaded: " + spriteName);
			return null;
		}

		return furnitureSprites[spriteName];
	}

	void OnFurnitureChanged( Furniture furn )
	{
		// Make sure the furniture's graphics are correct. e.g. damaged furniture, walls adjusting for newly adjacent walls, etc

		if( furnitureGameObjectMap.ContainsKey(furn) == false )
		{
			Debug.LogError("OnFurnitureChanged -- trying to change visuals for furniture not in our map.");
			return;
		}

		GameObject furn_go = furnitureGameObjectMap[furn];
		furn_go.GetComponent<SpriteRenderer>().sprite = GetSpriteForFurniture(furn);
	}
}
