using UnityEngine;
using System.Collections.Generic;

public class CharacterSpriteController : MonoBehaviour {

	Dictionary<Character, GameObject> characterGameObjectMap;

	Dictionary<string, Sprite> characterSprites;

	// Shortcut to cut down on wordyness
	World world { get { return WorldController.Instance.world; } }

	// Use this for initialization
	void Start () {
		LoadSprites();

		// Instantiate our dictionary that tracks which GameObject is rendering which Furniture data.
		characterGameObjectMap = new Dictionary<Character, GameObject>();

		world.RegisterCharacterCreated( OnCharacterCreated );
		// We will in the future add FurnitureChanged & FurnitureDestroyed


		// DEBUG
		world.CreateCharacter( world.GetTileAt( world.Width/2, world.Height/2 ) );

	}

	void LoadSprites()
	{
		characterSprites = new Dictionary<string, Sprite>();
		Sprite[] sprites = Resources.LoadAll<Sprite>("Images/Characters/");

		foreach(Sprite s in sprites)
		{
			characterSprites[s.name] = s;
		}
	}

	public void OnCharacterCreated( Character character )
	{
		//Debug.Log("OnFurnitureCreated");
		// Create a visual GameObject linked to this data.

		// FIXME: Does not consider multi-tile objects nor rotated objects

		// This creates a new GameObject and adds it to our scene.
		GameObject char_go = new GameObject();

		// Add our tile/GO pair to the dictionary
		characterGameObjectMap.Add( character, char_go );

		char_go.name = "Character";
		char_go.transform.position = new Vector3( character.currTile.X, character.currTile.Y, 0 );
		char_go.transform.SetParent(this.transform, true);

		// FIXME: We assume that the object must be a single wall, so use the hardcoded reference to the wall sprite.
		SpriteRenderer sr = char_go.AddComponent<SpriteRenderer>();
		sr.sprite = characterSprites["p1_front"];
		sr.sortingLayerName = "Character";

		// Register our callback so our GameObject gets updated whenever the object's info changes
		//character.RegisterOnChangedCallback( OnFurnitureChanged );
	}

/*	void OnFurnitureChanged( Furniture furn )
	{
		// Make sure the furniture's graphics are correct. e.g. damaged furniture, walls adjusting for newly adjacent walls, etc

		if( characterGameObjectMap.ContainsKey(furn) == false )
		{
			Debug.LogError("OnFurnitureChanged -- trying to change visuals for furniture not in our map.");
			return;
		}

		GameObject furn_go = characterGameObjectMap[furn];
		furn_go.GetComponent<SpriteRenderer>().sprite = GetSpriteForFurniture(furn);
	}
*/
	public Sprite GetSpriteForFurniture( string objectType )
	{
		if(characterSprites.ContainsKey(objectType))
		{
			return characterSprites[objectType];
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
