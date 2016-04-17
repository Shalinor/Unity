using UnityEngine;
using System.Collections;

public class MouseController : MonoBehaviour {

	public GameObject circleCursor;

	// The world-position of the mouse this frame
	Vector3 currFramePosition;

	// The world-position of the mouse last frame
	Vector3 lastFramePosition;

	// The world-position of our left-mouse drag operation
	Vector3 dragStartPosition;

	// Min/Max zooms for camera
	public int maxCameraZoomOut = 25;
	public int minCameraZoomIn = 5;


	// Use this for initialization
	void Start () {
		// Set zoom level to default (10)
		Camera.main.orthographicSize = 10;
	}
	
	// Update is called once per frame
	void Update () {
		currFramePosition = Camera.main.ScreenToWorldPoint( Input.mousePosition );
		currFramePosition.z = 0;
		currFramePosition = CenterPointOffset( currFramePosition );


		UpdateCursor();
		UpdateDragging();		// Left mouse button
		UpdateCameraMovement();	// Right/Center mouse button click and drag map, scroll for zoom


		// Save the mouse position from this frame
		// We don't use currFramePosition because we may have moved the camera.
		lastFramePosition = Camera.main.ScreenToWorldPoint( Input.mousePosition );
		lastFramePosition.z = 0;
		lastFramePosition = CenterPointOffset( lastFramePosition );
	}

	void UpdateCursor()
	{
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
	}

	void UpdateDragging()
	{
		// Start drag
		if( Input.GetMouseButtonDown(0) ) // If left mouse button was pressed in the last frame
		{
			dragStartPosition = currFramePosition;
		}

		// End drag
		if( Input.GetMouseButtonUp(0) )	// If left mouse button was released in the last frame
		{
			int start_x = Mathf.FloorToInt( dragStartPosition.x );
			int end_x = Mathf.FloorToInt( currFramePosition.x );

			// Invert start_x & end_x in case dragging right to left to ensure that end_x is always >= start_x
			if( end_x < start_x )
			{
				int temp = end_x;
				end_x = start_x;
				start_x = temp;
			}

			int start_y = Mathf.FloorToInt( dragStartPosition.y );
			int end_y = Mathf.FloorToInt( currFramePosition.y );

			// Invert start_y & end_y in case dragging right to left to ensure that end_y is always >= start_y
			if( end_y < start_y )
			{
				int temp = end_y;
				end_y = start_y;
				start_y = temp;
			}

			for (int x = start_x; x <= end_x; x++) {
				for (int y = start_y; y <= end_y; y++) {
					Tile t = WorldController.Instance.World.GetTileAt( x, y );
					if( t != null )
					{
						t.Type = Tile.TileType.Floor;
					}
				}
			}
		}

	}

	void UpdateCameraMovement()
	{
		// Screen dragging - Right or Middle mouse buttons
		if( Input.GetMouseButton(1) || Input.GetMouseButton(2) )
		{
			Vector3 difference = lastFramePosition - currFramePosition;
			Camera.main.transform.Translate( difference );
		}

		// Zoom camera in & out - scroll wheel
		if( Input.GetAxis( "Mouse ScrollWheel" ) != 0 )
		{
			Camera.main.orthographicSize = Mathf.Clamp( ( Camera.main.orthographicSize - Input.GetAxis( "Mouse ScrollWheel" ) ), minCameraZoomIn, maxCameraZoomOut );
		}
	}

	Tile GetTileAtWorldCoord( Vector3 coord )
	{
		//		coord += new Vector3( 0.5f, 0.5f, 0 ); // Offsetting for center pivots vs bottom-left
		int x = Mathf.FloorToInt(coord.x);
		int y = Mathf.FloorToInt(coord.y);

		return WorldController.Instance.World.GetTileAt(x, y);
	}

	Vector3 CenterPointOffset( Vector3 vector )
	{
		return vector += new Vector3( 0.5f, 0.5f, 0 );
	}
}
