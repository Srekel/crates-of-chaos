using UnityEngine;
using System.Collections;

public class Spawner_RandomDirection : MonoBehaviour {

	public ISpawnable prefab_to_spawn;
	public float angle_spread = 30;
	public float power_min = 5;
	public float power_max = 10;
	public float spawn_time = 2;

	// Use this for initialization
	void Start () {
		SpawnerSystem.instance.AddSpawner(this);
	}

	void OnDestroy()
	{
		SpawnerSystem.instance.RemoveSpawner(this);
	}
	
	public void OnSpawned(ISpawnable spawned_object)
	{
		// todo add impulse
	}
}
