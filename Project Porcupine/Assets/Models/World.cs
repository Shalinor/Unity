﻿using UnityEngine;
using System.Collections.Generic;
using System;

public class World {

	// A two-dimensional array to hold our tile data
	Tile[,] tiles;

	List<Character> characters;

	Dictionary<string, Furniture> furniturePrototypes;

	// The tile width of the world.
	public int Width { get; protected set; }

	// The tile height of the world.
	public int Height { get; protected set; }

	Action<Furniture> cbFurnitureCreated;
	Action<Tile> cbTileChanged;
	Action<Character> cbCharacterCreated;

	public JobQueue jobQueue;

	/// <summary>
	/// Initializes a new instance of the <see cref="World"/> class.
	/// </summary>
	/// <param name="width">Width.</param>
	/// <param name="height">Height.</param>
	public World(int width = 100, int height = 100) {
		jobQueue = new JobQueue();

		Width = width;
		Height = height;

		tiles = new Tile[width, height];

		//Instantiate the tiles.
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				tiles[x,y] = new Tile(this, x, y);
				tiles[x,y].RegisterTileTypeChangedCallback( OnTileChanged );
			}
		}

		Debug.Log("World created with "+width*height+" tiles.");

		CreateFurniturePrototypes();

		characters = new List<Character>();
	}

	public void Update( float deltaTime )
	{
		foreach(Character c in characters)
		{
			c.Update(deltaTime);
		}
	}

	public Character CreateCharacter( Tile t )
	{
		Character c = new Character( t );

		characters.Add(c);

		if(cbCharacterCreated != null)
			cbCharacterCreated( c );

		return c;
	}

	/// <summary>
	/// Creates the installed object prototypes.
	/// </summary>
	void CreateFurniturePrototypes()
	{
		furniturePrototypes = new Dictionary<string, Furniture>();

		Furniture wallPrototype = Furniture.CreatePrototype( 
			"Wall_",
			0,		// Impassable
			1,		// Width
			1,		// Height
			true );	// Links to neighbours and "sort of" becomes part of a large object

		furniturePrototypes.Add("Wall_", wallPrototype);
	}

	/// <summary>
	/// Randomizes the tiles.
	/// </summary>
	public void RandomizeTiles() {
//		Debug.Log("Randomising");

		for (int x = 0; x < Width; x++) {
			for (int y = 0; y < Height; y++) {
				tiles[x,y] = GetTileAt(x, y);//new Tile(this, x, y);

				if(UnityEngine.Random.Range(0, 2) == 0){
					tiles[x,y].Type = TileType.EMPTY;
				}
				else{
					tiles[x,y].Type = TileType.FLOOR;
				}
			}
		}

//		Debug.Log("Randomised");
	}

	/// <summary>
	/// Gets the tile at x and y.
	/// </summary>
	/// <returns>The <see cref="Tile"/>.</returns>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	public Tile GetTileAt(int x, int y) {
		if(x > Width || x < 0 || y > Height || y < 0){
			Debug.LogError("Tile ("+x+","+y+") is out of range.");
			return null;
		}

		return tiles[x,y];
	}


	public void PlaceFurniture( string objectType, Tile t )
	{
//		Debug.Log("PlaceFurniture");
		// TODO: This function assumes 1x1 tiles -- change this later!
		// TODO: This function assumes no rotation

		if( furniturePrototypes.ContainsKey( objectType ) == false )
		{
			Debug.LogError("furniturePrototypes doesn't contain a proto for key: " + objectType);
			return;
		}

		Furniture obj = Furniture.PlaceInstance( furniturePrototypes[objectType], t );

		if( obj == null )
		{
			// Failed to place object -- most likely there was already something there.
			return;
		}

		if( cbFurnitureCreated != null )
		{
			cbFurnitureCreated( obj );
		}
	}

	public void RegisterFurnitureCreated( Action<Furniture> callbackfunc )
	{
		cbFurnitureCreated += callbackfunc;
	}

	public void UnregisterFurnitureCreated( Action<Furniture> callbackfunc )
	{
		cbFurnitureCreated -= callbackfunc;
	}

	public void RegisterTileChanged( Action<Tile> callbackfunc )
	{
		cbTileChanged += callbackfunc;
	}

	public void UnregisterTileChanged( Action<Tile> callbackfunc )
	{
		cbTileChanged -= callbackfunc;
	}

	public void RegisterCharacterCreated( Action<Character> callbackfunc )
	{
		cbCharacterCreated += callbackfunc;
	}

	public void UnregisterCharacterCreated( Action<Character> callbackfunc )
	{
		cbCharacterCreated += callbackfunc;
	}


	void OnTileChanged( Tile t )
	{
		if( cbTileChanged == null )
			return;

		cbTileChanged(t);
	}

	public bool IsFurniturePlacementValid( string furnitureType, Tile t )
	{
		return furniturePrototypes[furnitureType].IsValidPosition( t );
	}

	public Furniture GetFurniturePrototype( string objectType )
	{
		if( furniturePrototypes.ContainsKey(objectType) == false )
		{
			Debug.LogError("No furniture with type: " + objectType);
			return null;
		}
		
		return furniturePrototypes[objectType];
	}
}
