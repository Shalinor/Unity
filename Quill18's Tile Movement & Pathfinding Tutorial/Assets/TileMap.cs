using UnityEngine;
using System.Collections;

public class TileMap : MonoBehaviour {

	public TileType[] tileTypes;

	int[,] tiles;

	int mapSizeX = 10;
	int mapSizeY = 10;

	void Start(){
		GenerateMapData ();
		GenerateMapVisuals ();
	}

	void GenerateMapData(){
		//Allocate our map tiles
		tiles = new int[mapSizeX, mapSizeY];

		int x, y;

		//Initialise our map tiles to be grass
		for (x = 0; x < mapSizeX; x++) {
			for (y = 0; y < mapSizeY; y++) {
				tiles[x, y] = 0;
			}
		}

		//Make a big swamp area
		for (x=3; x <= 5; x++) {
			for(y = 0; y < 4; y++) {
				tiles[x, y] = 1;
			}
		}

		//Let's make a u-shaped mountain range - hard coded naughtiness...
		tiles [4, 4] = 2;
		tiles [5, 4] = 2;
		tiles [6, 4] = 2;
		tiles [7, 4] = 2;
		tiles [8, 4] = 2;
		
		tiles [4, 5] = 2;
		tiles [4, 6] = 2;
		tiles [8, 5] = 2;
		tiles [8, 6] = 2;
	}

	void GenerateMapVisuals(){
		for (int x = 0; x < mapSizeX; x++) {
			for (int y = 0; y < mapSizeY; y++) {

				TileType tt = tileTypes[ tiles[x, y] ];
				Instantiate( tt.tileVisualPrefab, new Vector3(x, y, 0), Quaternion.identity );
			}
		}
	}
}
