using UnityEngine;
using System.Collections;

public abstract class Spawner : MonoBehaviour {
	public Spawnable prefab_to_spawn;
	//public float spawn_time = 2;

	//[HideInInspector]
	//public float time_of_next_spawn = 0;

	public abstract GameObject Spawn(out float time_until_next_spawn);
}
