using UnityEngine;
using System.Collections;

public class Spawner_RandomDirection : Spawner
{
	public float angle_spread = 30;
	public float power_min = 50;
	public float power_max = 100;

	void Start () {
		SpawnerSystem.instance.AddSpawner(this, true);
	}

	void OnDestroy()
	{
		SpawnerSystem.instance.RemoveSpawner(this);
	}
	
	public override void OnSpawned(Spawnable spawned_object) 
	{
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
	}
}
