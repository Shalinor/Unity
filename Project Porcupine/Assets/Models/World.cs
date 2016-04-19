﻿using UnityEngine;
using System.Collections.Generic;

public class World {

	// A two-dimensional array to hold our tile data
	Tile[,] tiles;

	Dictionary<string, InstalledObject> installedObjectPrototypes;

	// The tile width of the world.
	public int Width { get; protected set; }

	// The tile height of the world.
	public int Height { get; protected set; }

	/// <summary>
	/// Initializes a new instance of the <see cref="World"/> class.
	/// </summary>
	/// <param name="width">Width.</param>
	/// <param name="height">Height.</param>
	public World(int width = 100, int height = 100) {
		Width = width;
		Height = height;

		tiles = new Tile[width, height];

		//Instantiate the tiles.
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				tiles[x,y] = new Tile(this, x, y);
			}
		}

		Debug.Log("World created with "+width*height+" tiles.");

		CreateInstalledObjectPrototypes();
	}

	/// <summary>
	/// Creates the installed object prototypes.
	/// </summary>
	void CreateInstalledObjectPrototypes()
	{
		installedObjectPrototypes = new Dictionary<string, InstalledObject>();

		InstalledObject wallPrototype = InstalledObject.CreatePrototype( 
			"Wall_",
			0);	// Impassable, leaving both width & height as the default 1

		installedObjectPrototypes.Add("Wall_", wallPrototype);
	}

	/// <summary>
	/// Randomizes the tiles.
	/// </summary>
	public void RandomizeTiles() {
//		Debug.Log("Randomising");

		for (int x = 0; x < Width; x++) {
			for (int y = 0; y < Height; y++) {
				tiles[x,y] = GetTileAt(x, y);//new Tile(this, x, y);

				if(Random.Range(0, 2) == 0){
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
}
