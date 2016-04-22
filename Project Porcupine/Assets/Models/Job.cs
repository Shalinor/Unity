using UnityEngine;
using System.Collections.Generic;
using System;

public class Job {

	// This class holds info for a queued up job, which can include
	// things like placing furniture, moving stored inventory,
	// working at a desk, and maybe even fighting enemies.

	public Tile tile { get; protected set; }
	float jobTime;	// Time required to complete job

	// FIXME: Hard-coding a parameter for furniture. Do not like.
	public Furniture theFurniture;

	Action<Job> cbJobComplete;
	Action<Job> cbJobCancel;

	public Job ( Tile tile, Action<Job> cbJobComplete, float jobTime = 1f )
	{
		this.tile = tile;
		this.cbJobComplete += cbJobComplete;
		this.jobTime = jobTime;
	}

	public void RegisterJobCompleteCallback( Action<Job> callback )
	{
		cbJobComplete += callback;
	}

	public void RegisterJobCancelCallback( Action<Job> callback )
	{
		cbJobCancel += callback;
	}

	public void DoWork( float workTime )
	{
		jobTime -= workTime;

		if( jobTime <= 0 )
		{
			if( cbJobComplete != null )
				cbJobComplete( this );
		}
	}

	public void CancelJob()
	{
		if( cbJobCancel != null )
			cbJobCancel( this );
	}
}
