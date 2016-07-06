using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class SpawnerSystem : MonoBehaviour {

	// todo make ISpawner
	private List<Spawner> spawners = new List<Spawner>(100);

	public static SpawnerSystem instance;
	void Awake () {
		instance = this;
	}
	
	void Update () {
		var time = Time.time;
		for (int i = 0; i < spawners.Count; i++)
		{
			var spawner = spawners[i];
			var spawn_time = spawner.time_of_next_spawn;
			if (time > spawn_time)
			{
				var rotation = spawner.transform.rotation; // todo add angle
				var position = spawner.transform.position + spawner.transform.up * 2; // todo add offset
				var spawned_object = (Spawnable)Instantiate(spawner.prefab_to_spawn, position, rotation);
				spawner.OnSpawned(spawned_object);
                spawned_object.transform.SetParent(GameSceneManager.Instance.SpawnerParent.transform);
				spawner.time_of_next_spawn = time + spawner.spawn_time;
			}
		}
	}

	internal void AddSpawner(Spawner spawner, bool reset_spawn_time)
	{
		spawners.Add(spawner);
		if (reset_spawn_time)
		{
			spawner.time_of_next_spawn = Time.time + spawner.spawn_time;
		}
	}

	internal void RemoveSpawner(Spawner spawner)
	{
		for (int i = 0; i < spawners.Count; i++)
		{
			if (spawners[i] == spawner)
			{
				// Swap in the last one, pop it away
				spawners[i] = spawners[spawners.Count - 1];
				spawners.RemoveAt(spawners.Count - 1);

				break;
			}
		}
	}
}
