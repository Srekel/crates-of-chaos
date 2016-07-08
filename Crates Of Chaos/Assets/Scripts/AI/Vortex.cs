using UnityEngine;
using System.Collections;

public class Vortex : MonoBehaviour {
	public int levels = 10;
	public float time_until_first_spawn = 3;
	public float time_between_levels = 3;
	public float time_between_spawns = 0.2f;

	public GameObject enemy_1_prefab;
	public GameObject enemy_2_prefab;

	private int level = 0;
	private float time_to_next;
	private string state = "waiting";
	private string state_after_wait = "opening";
	private int to_spawn;

	// Use this for initialization
	void Start () {
		time_to_next = time_until_first_spawn;
		transform.localScale = new Vector3(1, 1, 1) * 0.5f;
	}
	
	// Update is called once per frame
	void Update () {
		if (state == "waiting")
		{
			if (Time.time > time_to_next)
			{ 
				state = state_after_wait;
			}
		}

		if (state == "opening")
		{
			var dt = Time.deltaTime;
			transform.localScale += new Vector3(dt, dt, dt) * 5;
			if (transform.localScale.x >= 3)
			{
				transform.localScale = new Vector3(3, 3, 3);
				state = "waiting";
				state_after_wait = "spawning";
				time_to_next = Time.time + 1;

				level++;
				to_spawn = level;
			}
		}

		if (state == "spawning")
		{
			if (Time.time > time_to_next)
			{
				var offset = new Vector3(
					Random.value * 0.2f - 0.1f,
					Random.value - 0.5f,
					0) * 5;
				var pos = transform.position + offset;
				pos.z = 0;
				if (Random.value > 0.5)
				{
					Instantiate(enemy_1_prefab, pos, Quaternion.identity);
				}
				else
				{
					Instantiate(enemy_2_prefab, pos, Quaternion.identity);
				}

				to_spawn--;
				to_spawn = 0; // temp todo
				if (to_spawn == 0)
				{
					state = "waiting";
					state_after_wait = "closing";
					time_to_next = Time.time + 1;
				}
				else
				{
					time_to_next = Time.time + time_between_spawns;
				}
			}
		}

		if (state == "closing")
		{
			var dt = Time.deltaTime;
			transform.localScale -= new Vector3(dt, dt, dt) * 5;
			if (transform.localScale.x <= 0.5f)
			{
				transform.localScale = new Vector3(1, 1, 1) * 0.5f;
				state = "waiting";
				state_after_wait = "opening";
				time_to_next = Time.time + time_between_levels;
			}
		}
	}
}
