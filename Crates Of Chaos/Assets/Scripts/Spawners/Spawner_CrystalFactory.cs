using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner_CrystalFactory : Spawner
{
	public float spawn_time = 2;
	public float time_between_resources = 0.2f;
	public float angle_spread = 30;
	public float power_min = 50;
	public float power_max = 100;
	public Spawnable basic_resource_prefab;
	public Spawnable blue_resource_prefab;
	public Spawnable red_resource_prefab;
	//public Dictionary<ResourceSystem.ResourceType, Spawnable> resources = new Dictionary<ResourceSystem.ResourceType, Spawnable>();

	private List<Spawnable> to_spawn = new List<Spawnable>();
	private int current_spawn_index = 0;
	private ResourceSystem.ResourceType resource;
	private int num_red = 0;

	void Start()
	{
		SpawnerSystem.instance.AddSpawner(this, spawn_time);
	}

	void OnDestroy()
	{
		SpawnerSystem.instance.RemoveSpawner(this);
	}

	public override GameObject Spawn(out float time_until_next_spawn)
	{
		if (current_spawn_index == to_spawn.Count)
		{
			current_spawn_index = 0;
			to_spawn.Clear();
			if (num_red == 0)
			{
				to_spawn.Add(basic_resource_prefab);
				to_spawn.Add(basic_resource_prefab);
				to_spawn.Add(basic_resource_prefab);
			}
			else
			{
				to_spawn.Add(red_resource_prefab);
				to_spawn.Add(red_resource_prefab);
				to_spawn.Add(red_resource_prefab);
			}
		}

		var prefab = to_spawn[current_spawn_index];
		current_spawn_index++;

		//var rotation = rotated_object_transform.rotation;
		var rotation = transform.rotation;
		var position = transform.position + transform.up * 5;

		var spawned_object = (Spawnable)Instantiate(prefab, position, rotation);

		var rigid_body = spawned_object.rigid_body;
		float angle_degrees = Random.value * angle_spread - angle_spread * 0.5f;
		var angle_radians = angle_degrees * Mathf.Deg2Rad;
		var power = Mathf.Lerp(power_min, power_max, Random.value);

		var rotation_sin = Mathf.Sin(angle_radians);
		var rotation_cos = Mathf.Cos(angle_radians);

		var wx = transform.up.x;
		var wy = transform.up.y;
		var weapon_direction = new Vector2(wx, wy);
		weapon_direction.x = (rotation_cos * wx) - (rotation_sin * wy);
		weapon_direction.y = (rotation_sin * wx) + (rotation_cos * wy);

		var direction = weapon_direction;
		var impulse = direction * power;
		var torque = angle_radians * -1;
		rigid_body.AddForce(impulse, ForceMode2D.Impulse);
		rigid_body.AddTorque(torque, ForceMode2D.Impulse);

		spawned_object.transform.up = direction;

		if (current_spawn_index == to_spawn.Count)
		{
			time_until_next_spawn = spawn_time;
		}
		else
		{
			time_until_next_spawn = time_between_resources;
		}

		return spawned_object.gameObject;
	}

	public void EnterRed()
	{
		num_red++;
		resource = ResourceSystem.ResourceType.Basic;
	}

	public void ExitRed()
	{
		num_red--;
		if (num_red == 0)
		{
			resource = ResourceSystem.ResourceType.Basic;
		}
	}
}
