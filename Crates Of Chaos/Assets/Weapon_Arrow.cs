using UnityEngine;
using System.Collections;

public class Weapon_Arrow : MonoBehaviour {

	public float base_time_between_shots = 2;
	public GameObject projectile_prefab;
	public GameObject weapon_root;
	public float base_projectile_speed = 50;

	private float time_between_shots = 2;
	private float time_of_next_shot = 0;
	private Targeter targeter;

	// Use this for initialization
	void Start () {
		time_of_next_shot = Time.time + base_time_between_shots;
		targeter = gameObject.GetComponentInChildren<Targeter>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		Debug.DrawLine(weapon_root.transform.position, weapon_root.transform.position + Vector3.forward);
		Debug.DrawLine(weapon_root.transform.position, weapon_root.transform.position + weapon_root.transform.forward * 20);
		Debug.DrawLine(weapon_root.transform.position + weapon_root.transform.forward * 21, weapon_root.transform.position + weapon_root.transform.forward * 25);
		if (Time.time > time_of_next_shot && targeter.current_target != null)
		{
			time_of_next_shot = Time.time + time_between_shots;

			//var rotation = Quaternion.LookRotation(weapon_root.transform.forward, Vector3.up);
			var rotation = Quaternion.LookRotation(Vector3.forward, -weapon_root.transform.up);
			//var rotation = Quaternion.identity;
			var projectile = (GameObject)Instantiate(projectile_prefab, weapon_root.transform.position + weapon_root.transform.forward * 3, rotation);
			//projectile.transform.forward = weapon_root.transform.forward;

			var wx = weapon_root.transform.up.x;
			var wy = weapon_root.transform.up.y;
			var weapon_direction = new Vector2(wx, wy);
			var rb = projectile.GetComponent<Rigidbody2D>();
			rb.AddForce(weapon_root.transform.forward * base_projectile_speed, ForceMode2D.Impulse);
			//rb.velocity = weapon_direction * base_projectile_speed;

			var torque = wx > 0 ? 1 : -1;
			rb.AddTorque(torque * 0.5f, ForceMode2D.Impulse);
		}
	}
}
