using UnityEngine;
using System.Collections;

public class CrateCollector : MonoBehaviour {

	public delegate void CollectedHandler(GameObject target);
	public event CollectedHandler collected;

	private Health health_component;

	// Use this for initialization
	void Start()
	{
		health_component = gameObject.transform.parent.GetComponentInChildren<Health>();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (health_component.current_health < health_component.max_health)
		{
			health_component.Heal(10);
		}
		else if (collected != null)
		{
			collected(other.gameObject);
		}

		GameObject.Destroy(other.gameObject);
	}
}
