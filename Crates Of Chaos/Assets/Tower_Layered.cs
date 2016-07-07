using UnityEngine;
using System.Collections;

public class Tower_Layered : MonoBehaviour {

	public GameObject prefab_1;
	public GameObject prefab_2;
	public GameObject prefab_3;
	public GameObject prefab_4_platform;
	public GameObject prefab_5_weapon_basic;
	public GameObject prefab_5_weapon_blue;
	public GameObject prefab_5_weapon_red;
	public ResourceSystem.ResourceType resource_type;

	GameObject go1;
	GameObject go2;
	GameObject go3;
	GameObject platform_go;
	GameObject weapon_go;
	GameObject tower_parts;

	void Start ()
	{
		tower_parts = gameObject.transform.FindChild("TowerParts").gameObject;
		go1 = Instantiate(prefab_1);
		platform_go = (GameObject)Instantiate(prefab_4_platform, transform.position, transform.rotation);
		if (resource_type == ResourceSystem.ResourceType.Basic)
		{
			weapon_go = (GameObject)Instantiate(prefab_5_weapon_basic, transform.position, transform.rotation);
		}
		else if (resource_type == ResourceSystem.ResourceType.Blue)
		{
			weapon_go = (GameObject)Instantiate(prefab_5_weapon_blue, transform.position, transform.rotation);
		}
		else if (resource_type == ResourceSystem.ResourceType.Red)
		{
			weapon_go = (GameObject)Instantiate(prefab_5_weapon_red, transform.position, transform.rotation);
		}

		go1.transform.SetParent(transform, false);
		platform_go.transform.SetParent(transform);
		weapon_go.transform.SetParent(transform);

		var tower_height = go1.GetComponentInChildren<MeshRenderer>().bounds.extents.y * 2;
		platform_go.transform.position = transform.position + new Vector3(0, tower_height, 0);
		weapon_go.transform.position = transform.position + new Vector3(0, tower_height, 0);

	}
	
	public void Upgrade()
	{
		if (go2 == null)
		{
			go2 = Instantiate(prefab_2);
			go2.transform.SetParent(transform, false);

			var tower_height = go2.GetComponentInChildren<MeshRenderer>().bounds.extents.y * 2;
			platform_go.transform.position = transform.position + new Vector3(0, tower_height, 0);
			weapon_go.transform.position = transform.position + new Vector3(0, tower_height, 0);

			var collider = gameObject.GetComponent<BoxCollider2D>();
			collider.size = new Vector2(collider.size.x, collider.size.y + tower_height);
			collider.offset = new Vector2(collider.offset.x, collider.size.y / 2);
		}
		else if (go3 == null)
		{
			go3 = Instantiate(prefab_3);
			go3.transform.SetParent(transform, false);

			var tower_height = go3.GetComponentInChildren<MeshRenderer>().bounds.extents.y * 2;
			platform_go.transform.position = new Vector3(0, tower_height, 0);
			weapon_go.transform.position = new Vector3(0, tower_height, 0);

			var collider = gameObject.GetComponent<BoxCollider2D>();
			collider.size = new Vector2(collider.size.x, collider.size.y + tower_height);
			collider.offset = new Vector2(collider.offset.x, collider.size.y / 2);
		}
	}
}
