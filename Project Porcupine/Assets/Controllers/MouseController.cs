using UnityEngine;
using System.Collections;

public class MouseController : MonoBehaviour {

	public GameObject circleCursor;

	Vector3 lastFramePosition;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 currFramePosition = Camera.main.ScreenToWorldPoint( Input.mousePosition );
		currFramePosition.z = 0;

		// Update the circle cursor position
		Tile tileUnderMouse = GetTileAtWorldCoord( currFramePosition );
		if( tileUnderMouse != null )
		{
			circleCursor.SetActive( true );
			Vector3 cursorPosition = new Vector3( tileUnderMouse.X, tileUnderMouse.Y, 0 );
			circleCursor.transform.position = cursorPosition;
		}
		else
		{
			circleCursor.SetActive( false );
		}

		// Handle left mouse clicks
		// Start drag
		if( Input.GetMouseButtonDown(0) ) // If left mouse button was pressed in the last frame
		{
			
		}

		// End drag
		if( Input.GetMouseButtonUp(0) )	// If left mouse button was released in the last frame
		{
			if( tileUnderMouse != null )
			{
				if( tileUnderMouse.Type == Tile.TileType.Empty )
				{
					tileUnderMouse.Type = Tile.TileType.Floor;
				}
				else
				{
					tileUnderMouse.Type = Tile.TileType.Empty;
				}
			}
		}

		// Screen dragging - Right or Middle mouse buttons
		if( Input.GetMouseButton(1) || Input.GetMouseButton(2) )
		{
			Vector3 difference = lastFramePosition - currFramePosition;
			Camera.main.transform.Translate( difference );
		}

		lastFramePosition = Camera.main.ScreenToWorldPoint( Input.mousePosition );
		lastFramePosition.z = 0;
	}

	Tile GetTileAtWorldCoord( Vector3 coord )
	{
		coord += new Vector3( 0.5f, 0.5f, 0 ); // Offsetting for center pivots vs bottom-left
		int x = Mathf.FloorToInt(coord.x);
		int y = Mathf.FloorToInt(coord.y);

		return WorldController.Instance.World.GetTileAt(x, y);
	}
}
