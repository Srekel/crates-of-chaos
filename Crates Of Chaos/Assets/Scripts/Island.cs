using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Island : MonoBehaviour {

	public ResourceSystem.ResourceType resource_type;

	private List<GameObject> colliders = new List<GameObject>();
	private Light light;

	void Start()
	{
		light = gameObject.GetComponentInChildren<Light>();
		light.enabled = false;
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
			for (int i = 0; i < 5; i++)
			{
				var pos = child.transform.position + new Vector3(Random.value*1, Random.value*1, -Random.value * 1f);
				var rot = Quaternion.AngleAxis(Random.value * 180, new Vector3(Random.value, Random.value, Random.value).normalized);
				var slottything = (GameObject)Instantiate(ResourceSystem.instance.red_island_slot_prefab, pos, rot);
				var rb = slottything.GetComponent<Rigidbody2D>();
				if (rb != null)
				{
					rb.isKinematic = true;
				}
			}

			light.transform.position = child.transform.position + new Vector3(0, 0, -3);
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

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.layer != LayerMask.NameToLayer("PlayerObject"))
		{
			return;
		}

		if (resource_type == ResourceSystem.ResourceType.Red)
		{
			var factory = collider.gameObject.GetComponent<Spawner_CrystalFactory>();
			if (factory != null)
			{
				factory.EnterRed();
			}
		}

		var hq = collider.gameObject.GetComponent<Spawner_Headquarters>();
		if (hq)
		{
			hq.Land();
		}

		ResourceSystem.instance.AddResourceType(resource_type);
		colliders.Add(collider.gameObject);

		if (resource_type != ResourceSystem.ResourceType.Basic)
		{
			light.enabled = true;
		}
	}

	void OnTriggerExit2D(Collider2D collider)
	{
		if (collider.gameObject.layer != LayerMask.NameToLayer("PlayerObject"))
		{
			return;
		}

		if (resource_type == ResourceSystem.ResourceType.Red)
		{
			var factory = collider.gameObject.GetComponent<Spawner_CrystalFactory>();
			if (factory != null)
			{
				factory.ExitRed();
			}
		}

		ResourceSystem.instance.RemoveResourceType(resource_type);
		colliders.Remove(collider.gameObject); // uugh

		if (!ResourceSystem.instance.IsResourceTypeAvailable(resource_type))
		{
			light.enabled = false;
		}
	}
}
