using UnityEngine;
using System.Collections;
using System;

public enum TileType { EMPTY, FLOOR }; //Rimworld example -> Marsh, Shallow_Water, Dirt, Rough_Stone, etc, etc};

public class Tile {
	private TileType _type = TileType.EMPTY;
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

	// Currently presuming each tile may only have 1 each of loose & installed object
	// Inventory is something like a drill or a stack of metal sitting on the floor
	Inventory inventory;

	// Furniture is something like a wall, door or sofa.
	public Furniture furniture { get; protected set; }

	// The context in which we exist - in case we need to know it...
	public World world { get; protected set; }
	public int X { get; protected set; }
	public int Y { get; protected set; }

	// The function we callback any time our type changes
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

	public void UnRegisterTileTypeChangedCallback(Action<Tile> callback) {
		cbTileTypeChanged -= callback;
	}

	public bool PlaceFurniture( Furniture objInstance )
	{
		if( objInstance == null )
		{
			// We are uninstalling whatever was here before
			furniture = null;
			return true;
		}

		// objInstance isn't null
		if( furniture != null )
		{
			Debug.LogError("Trying to assign furniture to a tile that already has one!");
			return false;
		}

		// At this point, everything's fine!
		furniture = objInstance;
		return true;
	}
}
