using UnityEngine;
using System.Collections.Generic;

public class JobSpriteController : MonoBehaviour {

	// This bare-bones controller is mostly just going to piggyback
	// on FurnitureSpriteController because we don't yet fully know
	// what our job system is going to look like in the end.

	FurnitureSpriteController fsc;
	Dictionary<Job, GameObject> jobGameObjectMap;

	// Use this for initialization
	void Start () {
		fsc = GameObject.FindObjectOfType<FurnitureSpriteController>();

		jobGameObjectMap = new Dictionary<Job, GameObject>();

		// FIXME: No such things as a job queue yet!
		WorldController.Instance.world.jobQueue.RegisterJobCreationCallBack( OnJobCreation );
	}

	void OnJobCreation( Job job )
	{
		// FIXME: We can only do furniture-building jobs.

		// TODO: Sprite

		// This creates a new GameObject and adds it to our scene.
		GameObject job_go = new GameObject();

		// Add our tile/GO pair to the dictionary
		jobGameObjectMap.Add( job, job_go );

		job_go.name = "JOB_" + job.jobObjectType + "_" + job.tile.X + "_" + job.tile.Y;
		job_go.transform.position = new Vector3( job.tile.X, job.tile.Y, 0 );
		job_go.transform.SetParent(this.transform, true);

		SpriteRenderer sr = job_go.AddComponent<SpriteRenderer>();
		sr.sprite = fsc.GetSpriteForFurniture( job.jobObjectType );
		sr.color = new Color( 0.5f, 1f, 0.5f, 0.25f );

		job.RegisterJobCompleteCallback( OnJobEnded );
		job.RegisterJobCancelCallback( OnJobEnded );
	}

	void OnJobEnded( Job job )
	{
		// This executes whether a job was COMPLETED or CANCELLED

		// FIXME: We can only do furniture-building jobs.

		// TODO: Delete sprite

		GameObject job_go = jobGameObjectMap[job];

		job.UnregisterJobCancelCallback(OnJobEnded);
		job.UnregisterJobCompleteCallback(OnJobEnded);

		Destroy(job_go);
	}

}
