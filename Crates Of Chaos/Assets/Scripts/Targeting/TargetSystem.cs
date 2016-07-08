using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TargetSystem : MonoBehaviour {

	public int searching_targeters_per_frame = 5;

	List<Targeter> targeters = new List<Targeter>();
	//List<Targeter> lost_targeters = new List<Targeter>();
	//List<Targeter> searching_targeters = new List<Targeter>();

	//int current_searching_target_index = 0;

	public static TargetSystem instance;
	void Awake()
	{
		instance = this;
	}
	
	void Update ()
	{
		//int start_index = current_searching_target_index >= searching_targeters.Count ? 0 : current_searching_target_index;
		//int end_index = Mathf.Max(start_index + searching_targeters_per_frame, searching_targeters.Count);
		//current_searching_target_index = end_index;
		//for (int i = start_index; i < end_index; i++)
		//{
		//	var targeter = searching_targeters[i];
		//}

		for (int i = 0; i < targeters.Count; i++)
		{
			var targeter = targeters[i];
			var to_target = targeter.current_target.transform.position - targeter.object_to_rotate.transform.position;
			var wanted_direction = to_target.normalized;
			var direction = Vector3.Lerp(targeter.object_to_rotate.transform.forward, wanted_direction, 1f);
			direction = direction.normalized;

			// todo make this better
			targeter.object_to_rotate.transform.forward = direction;
			//DebugExtension.DrawCircle(targeter.transform.position, 5);
			//Debug.DrawLine(targeter.object_to_rotate.transform.position + direction * 1.5f, targeter.current_target.transform.position);
		}
	}

	internal void AddTargeter(Targeter targeter)
	{
		targeters.Add(targeter);
	}

	internal void RemoveTargeter(Targeter targeter)
	{
		int index = targeters.IndexOf(targeter);
		targeters[index] = targeters[targeters.Count - 1];
		targeters.RemoveAt(targeters.Count - 1);
	}
}
