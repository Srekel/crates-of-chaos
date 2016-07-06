using UnityEngine;
using System.Collections;
using System;

public class Health : MonoBehaviour {
	public int health = 100;

	private int current_health;
	void Start () {
		current_health = health;
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
}
