﻿using UnityEngine;
using System.Collections;

public class Spawner_Weapon : Spawner {
	
	public float angle_spread = 30;
	public float power_min = 25;
	public float power_max = 50;
	public int ammo = 3;
	public float time_to_reload = 1;
	public float time_between_shots = 0.1f;
	
	private Targeter targeter;
	private bool active = false;
	private int current_ammo;

	void Start()
	{
		targeter = this.gameObject.transform.parent.FindChild("Targeter").GetComponent<Targeter>();
		targeter.target_changed += Targeter_target_changed;
		//SpawnerSystem.instance.AddSpawner(this, true);
	}

	private void Targeter_target_changed()
	{
		if (targeter.current_target == null)
		{
			SpawnerSystem.instance.RemoveSpawner(this);
			active = false;
		}
		else if(!active)
		{
			active = true;
			SpawnerSystem.instance.AddSpawner(this, time_to_reload);
		}
	}

	void OnDestroy()
	{
		if (active)
		{
			SpawnerSystem.instance.RemoveSpawner(this);
		}
	}

	public override GameObject Spawn(out float time_until_next_spawn)
	{
		var rotation = transform.rotation; // todo add angle
		var position = transform.position + transform.up * 2; // todo add offset
		var spawned_object = (Spawnable)Instantiate(prefab_to_spawn, position, rotation);

		var rigid_body = spawned_object.rigid_body;
		float angle_degrees = Random.value * angle_spread - angle_spread * 0.5f;
		var angle_radians = angle_degrees * Mathf.Deg2Rad;
		var power = Mathf.Lerp(power_min, power_max, Random.value);

		var x =
			this.transform.up.x * Mathf.Sin(angle_radians) +
			this.transform.up.y * Mathf.Sin(angle_radians);
		var y =
			this.transform.up.x * Mathf.Cos(angle_radians) +
			this.transform.up.y * Mathf.Cos(angle_radians);

		var direction = new Vector2(x, y);
		var impulse = direction * power;
		var torque = angle_radians * -1;
		rigid_body.AddForce(impulse, ForceMode2D.Impulse);
		rigid_body.AddTorque(torque, ForceMode2D.Impulse);

		spawned_object.transform.up = direction;

		current_ammo -= 1;
		if (current_ammo == 0)
		{
			current_ammo = ammo;
			time_until_next_spawn = time_to_reload;
		}

		time_until_next_spawn = time_between_shots;
		return spawned_object.gameObject;
	}
}
