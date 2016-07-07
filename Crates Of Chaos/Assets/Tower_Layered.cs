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
		platform_go = Instantiate(prefab_4_platform);
		if (resource_type == ResourceSystem.ResourceType.Basic)
		{
			weapon_go = Instantiate(prefab_5_weapon_basic);
		}
		else if (resource_type == ResourceSystem.ResourceType.Blue)
		{
			weapon_go = Instantiate(prefab_5_weapon_blue);
		}
		else if (resource_type == ResourceSystem.ResourceType.Red)
		{
			weapon_go = Instantiate(prefab_5_weapon_red);
		}

		go1.transform.SetParent(tower_parts.transform, false);
		platform_go.transform.SetParent(tower_parts.transform, false);
		weapon_go.transform.SetParent(tower_parts.transform, false);

		var tower_height = go1.GetComponentInChildren<MeshRenderer>().bounds.extents.y * 2;
		var platform_height = platform_go.GetComponentInChildren<MeshRenderer>().bounds.extents.y * 2;
		var weapon_height = weapon_go.GetComponentInChildren<MeshRenderer>().bounds.extents.y * 2;
		platform_go.transform.localPosition = new Vector3(0, tower_height, 0);
		weapon_go.transform.localPosition = new Vector3(0, tower_height + platform_height + weapon_height / 2, 0);
	}
	
	public void Upgrade()
	{
		if (go2 == null)
		{
			go2 = Instantiate(prefab_2);
			go2.transform.SetParent(tower_parts.transform, false);

			//var tower_height = go2.GetComponentInChildren<MeshRenderer>().bounds.extents.y * 2;
			//platform_go.transform.position = transform.position + new Vector3(0, tower_height, 0);
			//weapon_go.transform.position = transform.position + new Vector3(0, tower_height, 0);

			var tower_height = go2.GetComponentInChildren<MeshRenderer>().bounds.extents.y * 2;
			platform_go.transform.localPosition += new Vector3(0, tower_height, 0);
			weapon_go.transform.localPosition += new Vector3(0, tower_height, 0);

			var collider = gameObject.GetComponent<BoxCollider2D>();
			collider.size = new Vector2(collider.size.x, collider.size.y + tower_height);
			collider.offset = new Vector2(collider.offset.x, collider.size.y / 2);
		}
		else if (go3 == null)
		{
			go3 = Instantiate(prefab_3);
			go3.transform.SetParent(tower_parts.transform, false);

			//var tower_height = go3.GetComponentInChildren<MeshRenderer>().bounds.extents.y * 2;
			//platform_go.transform.position = new Vector3(0, tower_height, 0);
			//weapon_go.transform.position = new Vector3(0, tower_height, 0);

			var tower_height = go3.GetComponentInChildren<MeshRenderer>().bounds.extents.y * 2;
			platform_go.transform.localPosition += new Vector3(0, tower_height, 0);
			weapon_go.transform.localPosition += new Vector3(0, tower_height, 0);

			var collider = gameObject.GetComponent<BoxCollider2D>();
			collider.size = new Vector2(collider.size.x, collider.size.y + tower_height);
			collider.offset = new Vector2(collider.offset.x, collider.size.y / 2);
		}
	}
}
