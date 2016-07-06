using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class SpawnerSystem : MonoBehaviour {

	// todo make ISpawner
	private List<Spawner> spawners = new List<Spawner>(100);
	private Dictionary<Spawner, float> spawn_times = new Dictionary<Spawner, float>();

	public static SpawnerSystem instance;
	void Awake () {
		instance = this;
	}
	
	void Update () {
		var time = Time.time;
		for (int i = 0; i < spawners.Count; i++)
		{
			var spawner = spawners[i];
			var spawn_time = spawn_times[spawner];
			if (time > spawn_time)
			{
				float time_until_next_spawn;
				var spawned_object = spawner.Spawn(out time_until_next_spawn);
				spawn_times[spawner] = time + time_until_next_spawn;
				spawned_object.transform.SetParent(GameSceneManager.Instance.SpawnerParent.transform);

				spawn_times[spawner] = time + time_until_next_spawn;
			}
		}
	}

	internal void AddSpawner(Spawner spawner)
	{
		spawners.Add(spawner);
	}
	internal void AddSpawner(Spawner spawner, float time_until_next_spawn)
	{
		spawners.Add(spawner);
		spawn_times[spawner] = Time.time + time_until_next_spawn;
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

				spawn_times.Remove(spawner);

				break;
			}
		}
	}
}
