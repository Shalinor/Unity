using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour {

	float soundCoolDown = 0;

	// Use this for initialization
	void Start () {
		WorldController.Instance.World.RegisterFurnitureCreated( OnFurnitureCreated );	// Register this classes chosen function as a callback within the World's RegisterFurnitureCreated()

		WorldController.Instance.World.RegisterTileChanged( OnTileChanged );
	}
	
	// Update is called once per frame
	void Update () {
		soundCoolDown -= Time.deltaTime;
	}

	void OnTileChanged( Tile tile_data )
	{
		if( soundCoolDown > 0 )
			return;
		
		// FIXME - Hardcoded, eventually make it dynamic as per wall sprites, etc
		AudioClip ac = Resources.Load<AudioClip>("Sounds/Floor_OnCreated");
		AudioSource.PlayClipAtPoint( ac, Camera.main.transform.position );
		soundCoolDown = 0.1f;
	}

	void OnFurnitureCreated( Furniture furn )
	{
		if( soundCoolDown > 0 )
			return;
		
		// FIXME - Hardcoded, eventually make it dynamic as per wall sprites, etc
		AudioClip ac = Resources.Load<AudioClip>("Sounds/" + furn.FurnitureType + "OnCreated");

		if( ac == null )
		{
			// WTF? What do we do?
			// Since there's no specific sound for whatever Furniture this is, just
			// use a default sound -- i.e. the Wall_OnCreated sound.
			ac = Resources.Load<AudioClip>("Sounds/Wall_OnCreated");
		}

		AudioSource.PlayClipAtPoint( ac, Camera.main.transform.position );
		soundCoolDown = 0.1f;
	}
}
