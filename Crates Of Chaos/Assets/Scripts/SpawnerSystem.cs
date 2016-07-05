using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class SpawnerSystem : MonoBehaviour {

	// todo make ISpawner
	private List<Spawner_RandomDirection> spawners = new List<Spawner_RandomDirection>(100);
	private List<float> spawn_timers = new List<float>(100);

	public static SpawnerSystem instance;
	void Awake () {
		instance = this;
	}
	
	void Update () {
		var time = Time.time;
		for (int i = 0; i < spawners.Count; i++)
		{
			var spawn_time = spawn_timers[i];
			if (time > spawn_time)
			{
				var spawner = spawners[i];
				var rotation = spawner.transform.rotation; // todo add angle
				var position = spawner.transform.position + Vector3.up * 2; // todo add offset
				var spawned_object = (Spawnable)Instantiate(spawner.prefab_to_spawn, position, rotation);
				spawner.OnSpawned(spawned_object);

				spawn_timers[i] = time + spawner.spawn_time;
			}
		}
	}

	internal void AddSpawner(Spawner_RandomDirection spawner)
	{
		var time = Time.time;
		spawners.Add(spawner);
		spawn_timers.Add(time + spawner.spawn_time);
	}

	internal void RemoveSpawner(Spawner_RandomDirection spawner)
	{
		for (int i = 0; i < spawners.Count; i++)
		{
			if (spawners[i] == spawner)
			{
				// Swap in the last one, pop it away
				spawners[i] = spawners[spawners.Count - 1];
				spawners.RemoveAt(spawners.Count - 1);

				spawn_timers[i] = spawn_timers[spawn_timers.Count - 1];
				spawn_timers.RemoveAt(spawn_timers.Count - 1); 

				break;
			}
		}
	}
}
