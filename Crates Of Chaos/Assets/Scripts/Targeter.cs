using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Targeter : MonoBehaviour
{
	public GameObject object_to_rotate;

	[HideInInspector]
	public GameObject current_target;

	private List<GameObject> valid_targets = new List<GameObject>();
	
	void OnDisable()
	{
		if (current_target)
		{
			TargetSystem.instance.RemoveTargeter(this);
			current_target = null;
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		valid_targets.Add(other.gameObject);
		if (current_target == null)
		{
			current_target = other.gameObject;
			TargetSystem.instance.AddTargeter(this);
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		int index = valid_targets.IndexOf(other.gameObject);
		valid_targets[index] = valid_targets[valid_targets.Count - 1];
		valid_targets.RemoveAt(valid_targets.Count - 1);

		if (current_target == other.gameObject)
		{
			if (valid_targets.Count == 0)
			{
				TargetSystem.instance.RemoveTargeter(this);
				current_target = null;
			}
			else
			{
				current_target = valid_targets[0];
			}
		}
	}
}
