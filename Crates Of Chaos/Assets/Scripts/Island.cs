using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Island : MonoBehaviour {

	public ResourceSystem.ResourceType resource_type;

	private List<GameObject> colliders = new List<GameObject>();

	void Start()
	{
		if (resource_type == ResourceSystem.ResourceType.Basic)
		{
			
		}
		else if (resource_type == ResourceSystem.ResourceType.Blue)
		{
			var lol = gameObject.transform.FindChild("CrystalSlots");
			var index = Random.Range(0, lol.transform.childCount);
			var child = lol.transform.GetChild(index);
			Instantiate(ResourceSystem.instance.red_island_slot_prefab, child.transform.position, child.transform.rotation);
		}
		else if (resource_type == ResourceSystem.ResourceType.Red)
		{
			var lol = gameObject.transform.FindChild("CrystalSlots");
			var index = Random.Range(0, lol.transform.childCount);
			var child = lol.transform.GetChild(index);
			var slottything = (GameObject)Instantiate(ResourceSystem.instance.red_island_slot_prefab, child.transform.position, child.transform.rotation);
			var rb = slottything.GetComponent<Rigidbody2D>();
			if (rb != null)
			{
				rb.isKinematic = true;
			}
		}
	}

	// Use this for initialization
	void Update() {
		if (Random.value < 0.1f)
		{
			for (int i = 0; i < colliders.Count; i++)
			{
				if (colliders[i] == null)
				{
					colliders.RemoveAt(i);
					break;
				}
			}
		}
	}

	void OnTriggerEnter2D(Collision2D collision)
	{
		ResourceSystem.instance.AddResourceType(resource_type);
		colliders.Add(collision.gameObject);
	}

	void OnTriggerExit2D(Collision2D collision)
	{
		ResourceSystem.instance.RemoveResourceType(resource_type);
		colliders.Remove(collision.gameObject); // uugh
	}
}
