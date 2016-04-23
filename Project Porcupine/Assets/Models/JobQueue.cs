using UnityEngine;
using System.Collections.Generic;
using System;

public class JobQueue {
	Queue<Job> jobQueue;

	Action<Job> cbJobCreated;

	public JobQueue()
	{
		jobQueue = new Queue<Job>();
	}

	public void Enqueue( Job j )
	{
		jobQueue.Enqueue(j);

		// TODO: Call backs!!

		if(cbJobCreated != null)
		{
			cbJobCreated(j);
		}
	}

	public Job Dequeue()
	{
		if(jobQueue.Count == 0)
			return null;

		return jobQueue.Dequeue();
	}

	public void RegisterJobCreationCallBack(Action<Job> cb)
	{
		cbJobCreated += cb;
	}

	public void UnregisterJobCreationCallBack(Action<Job> cb)
	{
		cbJobCreated -= cb;
	}

}
