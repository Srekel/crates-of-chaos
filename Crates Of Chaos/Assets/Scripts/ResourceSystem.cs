using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourceSystem : MonoBehaviour {

	public enum ResourceType
	{
		Basic,
		Red,
		Blue,
	};

	public UnityEngine.Object basic_island_slot_prefab;
	public UnityEngine.Object red_island_slot_prefab;

	public string[] RESOURCE_TYPES = { "Basic", "Red", "Blue" };

	private Dictionary<ResourceType, int> resource_types = new Dictionary<ResourceType, int>();
	private Dictionary<ResourceType, int> resources = new Dictionary<ResourceType, int>();

	public static ResourceSystem instance;
	void Awake()
	{
		instance = this;
		resource_types[ResourceType.Basic] = 1;
		resource_types[ResourceType.Red] = 0;
		resource_types[ResourceType.Blue] = 0;
		resources[ResourceType.Basic] = 0;
		resources[ResourceType.Red] = 0;
		resources[ResourceType.Blue] = 0;
	}

	public void AddResourceType(ResourceType resource)
	{
		resource_types[resource] += 1;
	}

	public void RemoveResourceType(ResourceType resource)
	{
		resource_types[resource] -= 1;
	}

	public bool IsResourceTypeAvailable(ResourceType resource)
	{
		return resource_types[resource] > 0;
	}
	
	public void AddResources(ResourceType resource, int count)
	{
		resources[resource] += count;
	}

	public void RemoveResources(ResourceType resource, int count)
	{
		resources[resource] -= count;
	}
}
