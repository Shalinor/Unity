using UnityEngine;
using System.Collections.Generic;
//using System.Linq; // Used to get the first key from a dictionary
using System;

public class FurnitureSpriteController : MonoBehaviour {

	Dictionary<Furniture, GameObject> furnitureGameObjectMap;

	Dictionary<string, Sprite> furnitureSprites;

	// Shortcut to cut down on wordyness
	World world { get { return WorldController.Instance.world; } }

	// Use this for initialization
	void Start () {
		LoadSprites();

		// Instantiate our dictionary that tracks which GameObject is rendering which Furniture data.
		furnitureGameObjectMap = new Dictionary<Furniture, GameObject>();

		world.RegisterFurnitureCreated( OnFurnitureCreated );
		// We will in the future add FurnitureChanged & FurnitureDestroyed
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
		SpriteRenderer sr = furn_go.AddComponent<SpriteRenderer>();
		sr.sprite = GetSpriteForFurniture(furn);
		sr.sortingLayerName = "Furniture";

		// Register our callback so our GameObject gets updated whenever the object's info changes
		furn.RegisterOnChangedCallback( OnFurnitureChanged );
	}

	public Sprite GetSpriteForFurniture( Furniture furn )
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

		// FIXME: Diagonal joins only, and external joins to the "Block" pieces don't work -- There is no graphic for them...

		t = world.GetTileAt(x, y + 1);
		if( t != null && t.furniture != null && t.furniture.FurnitureType == furn.FurnitureType )
		{
			spriteName += "N";
		}

/*		t = World.GetTileAt(x + 1, y + 1);
		if( t != null && t.furniture != null && t.furniture.FurnitureType == furn.FurnitureType )
		{
			spriteName += "NE";
		}*/

		t = world.GetTileAt(x + 1, y);
		if( t != null && t.furniture != null && t.furniture.FurnitureType == furn.FurnitureType )
		{
			spriteName += "E";
		}

/*		t = World.GetTileAt(x + 1, y - 1);
		if( t != null && t.furniture != null && t.furniture.FurnitureType == furn.FurnitureType )
		{
			spriteName += "SE";
		}*/

		t = world.GetTileAt(x, y - 1);
		if( t != null && t.furniture != null && t.furniture.FurnitureType == furn.FurnitureType )
		{
			spriteName += "S";
		}

/*		t = World.GetTileAt(x - 1, y - 1);
		if( t != null && t.furniture != null && t.furniture.FurnitureType == furn.FurnitureType )
		{
			spriteName += "SW";
		}*/

		t = world.GetTileAt(x - 1, y);
		if( t != null && t.furniture != null && t.furniture.FurnitureType == furn.FurnitureType )
		{
			spriteName += "W";
		}

/*		t = World.GetTileAt(x - 1, y + 1);
		if( t != null && t.furniture != null && t.furniture.FurnitureType == furn.FurnitureType )
		{
			spriteName += "NW";
		}*/

		// For example, if this object has all four neighbours of
		// the same tupe, then the string will look like:
		//           Wall_NESW

		if( furnitureSprites.ContainsKey(spriteName) == false )
		{
			Debug.LogError("FurnitureSpriteController.cs - GetSpriteForFurniture(Furniture obj) -- Attempted to display a sprite that has not been created/loaded: " + spriteName);
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

	public Sprite GetSpriteForFurniture( string objectType )
	{
		if(furnitureSprites.ContainsKey(objectType))
		{
			return furnitureSprites[objectType];
		}

		// This is to allow for Quill using "Wall" as his default rather than "Wall_" as I currently do
/*		if(furnitureSprites.ContainsKey(objectType+"_"))
		{
			return furnitureSprites[objectType+"_"];
		}
*/
		Debug.LogError("FurnitureSpriteController.cs - GetSpriteForFurniture(string objectType) -- Attempted to display a sprite that has not been created/loaded: " + objectType);
		return null;
	}
}
