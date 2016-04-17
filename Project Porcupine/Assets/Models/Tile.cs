using UnityEngine;
using System.Collections;
using System;

public class Tile {

	public enum TileType { Empty, Floor }; //Rimworld example -> Marsh, Shallow_Water, Dirt, Rough_Stone, etc, etc};

	private TileType _type = TileType.Empty;
	public TileType Type {
		get { return _type; }
		set {
			if(_type != value){
				_type = value;

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
	public int X { get; protected set; }
	public int Y { get; protected set; }

	Action<Tile> cbTileTypeChanged;

	public Tile( World world, int x, int y ) {
		this.world = world;
		this.X = x;
		this.Y = y;
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
