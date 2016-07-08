using UnityEngine;
using System.Collections;

public class Enemy_Spiral : MonoBehaviour {

	public GameObject projectile_prefab;
	public float time_between_shots = 0.1f;
	public float time_between_attacks = 3;
	public float angle_between_shots = 10;

	private float time_of_next_attack;
	private string state = "waiting";
	private float angle = 0;

	// Use this for initialization
	void Start () {
		time_of_next_attack = Time.time + time_between_attacks;
	}
	
	// Update is called once per frame
	void Update () {
		var time = Time.time;
		if (state == "waiting")
		{
			if (time > time_of_next_attack)
			{
				state = "shooting";
				time_of_next_attack = time + time_between_shots;
				angle = 0;
			}
		}

		if (state == "shooting")
		{
			if (time > time_of_next_attack)
			{
				angle = angle + angle_between_shots;
				if (angle >= 720)
				{
					state = "waiting";
					time_of_next_attack = time + time_between_attacks;
				}

				var radians = angle * Mathf.Deg2Rad;
				var direction = new Vector3(
					Mathf.Cos(radians),
					Mathf.Sin(radians),
					0);
				var offset = direction * 2;
				var pos = transform.position + offset;
				var shot = (GameObject)Instantiate(projectile_prefab, pos, Quaternion.identity);
				shot.GetComponent<Rigidbody2D>().AddForce(direction * 150, ForceMode2D.Impulse);
			}
		}
	}
}
