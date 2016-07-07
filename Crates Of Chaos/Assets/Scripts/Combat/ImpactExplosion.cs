using UnityEngine;
using System.Collections;

public class ImpactExplosion : MonoBehaviour {
	public int damage = 100;
	public ParticleSystem explosion_effect;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		var ps = (ParticleSystem)Instantiate(explosion_effect, transform.position, Quaternion.identity);
		ps.Play();
		GameObject.Destroy(gameObject);

		var health = other.gameObject.GetComponent<Health>();
		if (health != null)
		{
			health.TakeDamage(damage);
		}
	}
}
