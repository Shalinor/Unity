using UnityEngine;
using System.Collections;
using System;

public class Tile {

	public enum TileType { Empty, Floor }; //Rimworld example -> Marsh, Shallow_Water, Dirt, Rough_Stone, etc, etc};

	TileType type = TileType.Empty;

	Action<Tile> cbTileTypeChanged;

	public TileType Type {
		get {
			return type;
		}
		set {
			if(type != value){
				type = value;

				// Call the callback and let things know we've changed.
				if(cbTileTypeChanged != null)
				{
					//Debug.Log("changed");
					cbTileTypeChanged(this);
				}
			}
		}
	}

	//Currently presuming each tile may only have 1 each of loose & installed object
	LooseObject looseObject;
	InstalledObject installedObject;

	World world;
	int x;

	public int X {
		get {
			return x;
		}
	}

	int y;

	public int Y {
		get {
			return y;
		}
	}

	public Tile( World world, int x, int y ) {
		this.world = world;
		this.x = x;
		this.y = y;
	}

	// Set callback function to local reference
	public void RegisterTileTypeChangedCallback(Action<Tile> callback) {
		//if(cbTileTypeChanged == null)
		//	Debug.Log("pre - cbTileTypeChanged == null");
		
		cbTileTypeChanged += callback;

		if(cbTileTypeChanged == null)
			Debug.Log("post - cbTileTypeChanged == null");
	}

/*	public void UnRegisterTileTypeChangedCallback(Action<Tile> callback) {
		cbTileTypeChanged -= callback;
	}*/
}
