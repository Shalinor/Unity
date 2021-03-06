using UnityEngine;
using System.Collections.Generic;
//using System.Linq; // Used to get the first key from a dictionary
using System;

public class TileSpriteController : MonoBehaviour {

	// FIXME
	public Sprite floorSprite;
	public Sprite emptySprite;

	Dictionary<Tile, GameObject> tileGameObjectMap;

	// Shortcut to cut down on wordyness
	World world { get { return WorldController.Instance.world; } }

	// Use this for initialization
	void Start () {
		// Instantiate our dictionary that tracks which GameObject is rendering which Tile data.
		tileGameObjectMap = new Dictionary<Tile, GameObject>();

		// Create a GameObject for each of our tiles, so they show visually.
		for (int x = 0; x < world.Width; x++) {
			for (int y = 0; y < world.Height; y++) {
				Tile tile_data = world.GetTileAt(x,y);

				// This creates a new GameObject and adds it to our scene.
				GameObject tile_go = new GameObject();

				// Add our tile/GO pair to the dictionary
				tileGameObjectMap.Add( tile_data, tile_go );

				tile_go.name = "Tile_"+x+"_"+y;
				tile_go.transform.position = new Vector3( tile_data.X, tile_data.Y, 0 );
				tile_go.transform.SetParent(this.transform, true);

				// Add a Sprite Renderer
				// Add a default sprite for empty tiles.
				SpriteRenderer sr = tile_go.AddComponent<SpriteRenderer>();
				sr.sprite = emptySprite;
				sr.sortingLayerName ="Tile";

			}
		}


		// Register our callback so our GameObject gets updated whenever the tile's data changes
		world.RegisterTileChanged( OnTileChanged );
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

	// Updates our graphics
	void OnTileChanged( Tile tile_data/*, GameObject tile_go*/ ){

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

}
