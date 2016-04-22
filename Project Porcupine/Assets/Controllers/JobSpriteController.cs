using UnityEngine;
using System.Collections;

public class JobSpriteController : MonoBehaviour {

	// This bare-bones controller is mostly just going to piggyback
	// on FurnitureSpriteController because we don't yet fully know
	// what our job system is going to look like in the end.

	FurnitureSpriteController fsc;

	// Use this for initialization
	void Start () {
		fsc = GameObject.FindObjectOfType<FurnitureSpriteController>();

		// FIXME: No such things as a job queue yet!
		WorldController.Instance.world.jobQueue.RegisterJobCreationCallBack( OnJobCreation );
	}

	void OnJobCreation( Job j )
	{
		// FIXME: We can only do furniture-building jobs.

		// TODO: Sprite

		Sprite theSprite = fsc.GetSpriteForFurniture(  );

		j.RegisterJobCompleteCallback( OnJobCompleted );
		j.RegisterJobCancelCallback( OnJobCompleted );
	}

	void OnJobCompleted( Job j )
	{
		// This executes whether a job was COMPLETED or CANCELLED

		// FIXME: We can only do furniture-building jobs.

		// TODO: Delete sprite
	}

}
