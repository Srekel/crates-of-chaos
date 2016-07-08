using UnityEngine;
using System.Collections;

public class Upgradable : MonoBehaviour {

	public int level = 1;
	public int strength = 4;

	private int needed_for_next_level = 0;
	public float modifier = 1;

	private Health health_component;
	private Spawner_Weapon spawner_component;
	private Tower_Layered tower_component;
	private Weapon weapon_component;

	// Use this for initialization
	void Start () {
		needed_for_next_level = strength;
		modifier = 1 + (level - 1) * strength * 0.5f;

		health_component = gameObject.transform.GetComponent<Health>();
		health_component.max_health = (int)(health_component.base_health * modifier);
		health_component.current_health = health_component.max_health;

		spawner_component = gameObject.GetComponent<Spawner_Weapon>();
		spawner_component.ammo_modified = (int)(spawner_component.ammo_modified * modifier);
		spawner_component.time_to_reload_modified = spawner_component.time_to_reload_modified / modifier;
		spawner_component.time_between_shots_modified = spawner_component.time_between_shots_modified / modifier;

		tower_component = gameObject.transform.parent.gameObject.GetComponent<Tower_Layered>();


		gameObject.transform.parent.GetComponentInChildren<CrateCollector>().collected += Crystal_collected;
	}

	private void Crystal_collected(GameObject target)
	{
		needed_for_next_level--;
		if (needed_for_next_level == 0)
		{
			needed_for_next_level = strength;
			++level;
			modifier = 1 + (level - 1) * strength * 0.5f;

			float health_ratio = health_component.current_health / health_component.max_health;
			health_component.max_health = (int)(health_component.base_health * modifier);
			health_component.current_health = (int)(health_component.max_health * health_ratio);

			spawner_component.ammo_modified = (int)(spawner_component.ammo_modified * modifier);
			spawner_component.time_to_reload_modified = spawner_component.time_to_reload_modified / modifier;
			spawner_component.time_between_shots_modified = spawner_component.time_between_shots_modified / modifier;

			tower_component.Upgrade();

			weapon_component = tower_component.weapon_go.GetComponent<Weapon>();
			weapon_component.Upgrade(strength);

			if (level == 3)
			{
				var collector = gameObject.transform.parent.GetComponentInChildren<CrateCollector>();
				collector.collected -= Crystal_collected;

				var collector_collider = collector.gameObject.GetComponent<Collider2D>();
				collector_collider.enabled = false;
			}
		}
	}
	
	void OnGUI ()
	{
		GUI.enabled = true;
		Camera cam = Camera.main;
		Vector3 pos = cam.WorldToScreenPoint(transform.position + new Vector3(0, 6, 0));
		GUI.Label(new Rect(pos.x, Screen.height - pos.y, 150, 130), "str=" + (strength-needed_for_next_level).ToString() + "/" + strength.ToString() + ", level=" + level.ToString());
	}
}
