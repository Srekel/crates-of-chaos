using UnityEngine;
using System.Collections;

public class Spawner_Weapon : Spawner {
	
	public float angle_spread = 30;
	public float power_min = 25;
	public float power_max = 50;
	public int ammo = 3;
	public float time_to_reload = 1;
	public float time_between_shots = 0.1f;

	[HideInInspector]
	public int ammo_modified;
	[HideInInspector]
	public float time_to_reload_modified;
	[HideInInspector]
	public float time_between_shots_modified;

	private Targeter targeter;
	private bool active = false;
	private int current_ammo;

	void Start()
	{
	
		targeter.target_changed += Targeter_target_changed;
		//SpawnerSystem.instance.AddSpawner(this, true);
		current_ammo = ammo;
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
			SpawnerSystem.instance.AddSpawner(this, time_between_shots);
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
		var rotated_object_transform = targeter.object_to_rotate.transform;
		var rotation = rotated_object_transform.rotation; 
		var position = rotated_object_transform.position + rotated_object_transform.up * 2; 
		var spawned_object = (Spawnable)Instantiate(prefab_to_spawn, position, rotation);

		var rigid_body = spawned_object.rigid_body;
		float angle_degrees = Random.value * angle_spread - angle_spread * 0.5f;
		var angle_radians = angle_degrees * Mathf.Deg2Rad;
		var power = Mathf.Lerp(power_min, power_max, Random.value);

		// http://answers.unity3d.com/questions/661383/whats-the-most-efficient-way-to-rotate-a-vector2-o.html
		var rotation_sin = Mathf.Sin(angle_radians);
		var rotation_cos = Mathf.Cos(angle_radians);

		var wx = rotated_object_transform.up.x;
		var wy = rotated_object_transform.up.y;
		var weapon_direction = new Vector2(wx, wy);
		weapon_direction.x = (rotation_cos * wx) - (rotation_sin * wy);
		weapon_direction.y = (rotation_sin * wx) + (rotation_cos * wy);

		var direction = weapon_direction; 
		var impulse = direction * power;
		var torque = angle_radians * -1;
		rigid_body.AddForce(impulse, ForceMode2D.Impulse);
		rigid_body.AddTorque(torque, ForceMode2D.Impulse);

		spawned_object.transform.up = direction;

		var seeker = spawned_object.GetComponent<Seeker_Thrust>();
		if (seeker)
		{
			seeker.SetTarget(targeter.current_target);
		}

		current_ammo -= 1;
		if (current_ammo == 0)
		{
			current_ammo = ammo;
			time_until_next_spawn = time_to_reload;
		}
		else
		{
			time_until_next_spawn = time_between_shots;
		}

		return spawned_object.gameObject;
	}
}
