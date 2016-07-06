using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Targeter : MonoBehaviour
{
	public GameObject object_to_rotate;

	[HideInInspector]
	public GameObject current_target;

	private List<GameObject> valid_targets = new List<GameObject>();

	public delegate void ChangedEventHandler();
	public event ChangedEventHandler target_changed;
	
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
		var target = other.gameObject.GetComponent<Target>();
		


		valid_targets.Add(other.gameObject);
		if (current_target == null)
		{
			current_target = other.gameObject;
			TargetSystem.instance.AddTargeter(this);

			if(target_changed != null)
				target_changed();
		}
        target.target_destroyed += DealWithTargetBeingRemoved;
    }

	void OnTriggerExit2D(Collider2D other)
	{
        DealWithTargetBeingRemoved(other.gameObject);
    }

    void DealWithTargetBeingRemoved(GameObject target) {
        int index = valid_targets.IndexOf(target);
        valid_targets[index] = valid_targets[valid_targets.Count - 1];
        valid_targets.RemoveAt(valid_targets.Count - 1);

        if (current_target == target)
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

            if (target_changed != null)
                target_changed();
        }
    }
}
