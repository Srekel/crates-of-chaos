using UnityEngine;
using System.Collections;
using System;

public class Health : MonoBehaviour {
	public int base_health = 100;

	[HideInInspector]
	public int max_health = 100;
	[HideInInspector]
	public int current_health;

	void Start () {
		max_health = base_health;
		current_health = base_health;
	}
	
	internal void TakeDamage(int damage)
	{
		current_health -= damage;
		if (current_health <= 0)
		{
			current_health = 0;
			GameObject.Destroy(gameObject);
		}
	}

	internal void Heal(int health)
	{
		current_health += health;
		if (current_health > max_health)
		{
			current_health = max_health;
		}
	}
}
