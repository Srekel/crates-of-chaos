using UnityEngine;
using System.Collections;

public class Upgradable : MonoBehaviour {

	public int level = 1;
	public int strength = 3;

	private int needed_for_next_level = 0;
	public float modifier = 1;

	private Health health_component;
	private Spawner_Weapon weapon_component;

	// Use this for initialization
	void Start () {
		needed_for_next_level = strength;
		modifier = level * (1 + strength * 0.5f);

		health_component = gameObject.GetComponent<Health>();
		health_component.max_health = (int)(health_component.base_health * modifier);
		health_component.current_health = health_component.max_health;

		weapon_component = gameObject.GetComponent<Spawner_Weapon>();
		weapon_component.ammo_modified = (int)(weapon_component.ammo_modified * modifier);
		weapon_component.time_to_reload_modified = weapon_component.time_to_reload_modified / modifier;
		weapon_component.time_between_shots_modified = weapon_component.time_between_shots_modified / modifier;

		gameObject.transform.parent.GetComponentInChildren<CrateCollector>().collected += Upgradable_collected;
	}

	private void Upgradable_collected(GameObject target)
	{
		needed_for_next_level--;
		if (needed_for_next_level == 0)
		{
			needed_for_next_level = strength;
			++level;
			modifier = level * (1 + strength * 0.5f);

			float health_ratio = health_component.current_health / health_component.max_health;
			health_component.max_health = (int)(health_component.base_health * modifier);
			health_component.current_health = (int)(health_component.max_health * health_ratio);

			weapon_component.ammo_modified = (int)(weapon_component.ammo_modified * modifier);
			weapon_component.time_to_reload_modified = weapon_component.time_to_reload_modified / modifier;
			weapon_component.time_between_shots_modified = weapon_component.time_between_shots_modified / modifier;

			if (level == 3)
			{
				var collector = gameObject.transform.parent.GetComponentInChildren<CrateCollector>();
				collector.collected -= Upgradable_collected;

				var collector_collider = collector.gameObject.GetComponent<Collider2D>();
				collector_collider.enabled = false;
			}
		}
	}
	
	void OnGUI ()
	{
		GUI.enabled = true;
		Camera cam = Camera.current;
		Vector3 pos = cam.WorldToScreenPoint(transform.position + new Vector3(0, 3, 0));
		GUI.Label(new Rect(pos.x, Screen.height - pos.y, 150, 130), "str=" + (strength-needed_for_next_level).ToString() + "/" + strength.ToString() + ", level=" + level.ToString());
	}
}
