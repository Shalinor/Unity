using UnityEngine;
using System.Collections;
using System;

// Furniture are things like walls, doors and furniture (e.g. a sofa)

public class Furniture {

	// This represents the BASE tile the object is on -- but in practice, large objects may actually occupy
	// multiple tiles
	public Tile tile{ get; protected set; }	

	// This "objectType" will be queried by the visual system to know what sprite to render for this object
	public string FurnitureType{ get; protected set; }

	// This is a multiplier. So a value of "2" here, means you move twice as slowly (i.e. at half speed)
	// Tile types and other environmental effects may be combined.
	// For example,. a "rough" tile ( cost of 2) with a table (cost of 3) that is on fire (cost of 3)
	// would have a total movement cost of 8 (2+3+3 = 8), so you'd move through this tile at 1/8th normal speed.  MAYBE???
	// SPECIAL: If movementCost = 0, then this tile is impassible. (e.g. a wall).
	float movementCost;

	// For example, a sofa might be 3x2 (actual graphics only appear to cover the 3x1 area, but the extra row is for leg room.
	int width;
	int height;

	public bool LinksToNeighbour{ get; protected set; }

	Action<Furniture> cbOnChanged;

	Func<Tile, bool> funcPositionValidation;

	// TODO: Implement larger objects
	// TODO: Implement object rotation

	// Prevent external, non-child classes from creating empty InstallObjects.
	protected Furniture()
	{
		
	}

	static public Furniture CreatePrototype( string objectType, float movementCost = 1f, int width = 1, int height = 1, bool linksToNeighbour = false )
	{
		Furniture furn = new Furniture();

		furn.FurnitureType = objectType;
		furn.movementCost = movementCost;
		furn.width = width;
		furn.height = height;
		furn.LinksToNeighbour = linksToNeighbour;

		furn.funcPositionValidation = furn.IsValidPosition;

		return furn;
	}

	static public Furniture PlaceInstance( Furniture proto, Tile tile )
	{
		// Check position validity
		if( proto.funcPositionValidation( tile ) == false )
		{
			Debug.LogError("PlaceInstance -- Position Validity Function returned FALSE.");
			return null;
		}

		// We now know our placement destination is valid.

		Furniture furn = new Furniture();

		furn.FurnitureType = proto.FurnitureType;
		furn.movementCost = proto.movementCost;
		furn.width = proto.width;
		furn.height = proto.height;
		furn.LinksToNeighbour = proto.LinksToNeighbour;

		furn.tile = tile;

		// FIXME: This (tile.PlaceObject()) assumes we are 1x1!
		if( tile.PlaceFurniture( furn ) == false)	// This should never happen now we use funcPositionValidation above...
		{
			// For some reason, we weren't able to place our object in thie tile.
			// (Probably it was already occupied.)

			// Do NOT return our newly instantiated object.
			// (It will be garbage collected.)
			return null;
		}

		if( furn.LinksToNeighbour )
		{
			// This type of furniture links itself to its neighbours,
			// so we should inform our neighbours that they have a new
			// buddy.  Just trigger their onChangedCallback.


			Tile t;	// Temporary holder of potential neighbour tiles
			int x = tile.X;
			int y = tile.Y;

			t = tile.world.GetTileAt(x, y + 1);	// North
			if( t != null && t.furniture != null && t.furniture.FurnitureType == furn.FurnitureType )
			{
				// We have a Northern neighbour with the same object type as us, so
				// tell it that it has changed by firing it's callback.
				t.furniture.cbOnChanged(t.furniture);
			}

/*			t = tile.world.GetTileAt(x + 1, y + 1); // North-East
			if( t != null && t.furniture != null && t.furniture.FurnitureType == furn.FurnitureType )
			{
				// We have a Northern neighbour with the same object type as us, so
				// tell it that it has changed by firing it's callback.
				t.furniture.cbOnChanged(t.furniture);
			}*/

			t = tile.world.GetTileAt(x + 1, y); // East
			if( t != null && t.furniture != null && t.furniture.FurnitureType == furn.FurnitureType )
			{
				// We have a Eastern neighbour with the same object type as us, so
				// tell it that it has changed by firing it's callback.
				t.furniture.cbOnChanged(t.furniture);
			}

/*			t = tile.world.GetTileAt(x + 1, y - 1); // South-East
			if( t != null && t.furniture != null && t.furniture.FurnitureType == furn.FurnitureType )
			{
				// We have a Northern neighbour with the same object type as us, so
				// tell it that it has changed by firing it's callback.
				t.furniture.cbOnChanged(t.furniture);
			}*/

			t = tile.world.GetTileAt(x, y - 1); // South
			if( t != null && t.furniture != null && t.furniture.FurnitureType == furn.FurnitureType )
			{
				// We have a Southern neighbour with the same object type as us, so
				// tell it that it has changed by firing it's callback.
				t.furniture.cbOnChanged(t.furniture);
			}

/*			t = tile.world.GetTileAt(x - 1, y - 1); // South-West
			if( t != null && t.furniture != null && t.furniture.FurnitureType == furn.FurnitureType )
			{
				// We have a Northern neighbour with the same object type as us, so
				// tell it that it has changed by firing it's callback.
				t.furniture.cbOnChanged(t.furniture);
			}*/

			t = tile.world.GetTileAt(x - 1, y); // West
			if( t != null && t.furniture != null && t.furniture.FurnitureType == furn.FurnitureType )
			{
				// We have a Western neighbour with the same object type as us, so
				// tell it that it has changed by firing it's callback.
				t.furniture.cbOnChanged(t.furniture);
			}

/*			t = tile.world.GetTileAt(x - 1, y + 1); // North-West
			if( t != null && t.furniture != null && t.furniture.FurnitureType == furn.FurnitureType )
			{
				// We have a Northern neighbour with the same object type as us, so
				// tell it that it has changed by firing it's callback.
				t.furniture.cbOnChanged(t.furniture);
			}*/
		}

		return furn;
	}

	public void RegisterOnChangedCallback( Action<Furniture> callbackFunc )
	{
		cbOnChanged += callbackFunc;
	}

	public void UnregisterOnChangedCallback( Action<Furniture> callbackFunc )
	{
		cbOnChanged -= callbackFunc;
	}

	public bool IsValidPosition( Tile t )
	{
		// Make sure tile is FLOOR
		if( t.Type != TileType.FLOOR )
		{
			return false;
		}

		// Make sure tile doesn't already have furniture
		if( t.furniture != null )
		{
			return false;
		}

		return true;
	}

	public bool IsValidPosition_Door( Tile t)
	{
		if( IsValidPosition( t ) == false )
		{
			return false;
		}
		// Make sure we have a pair of E/W walls or N/S walls

		return true;
	}
}
