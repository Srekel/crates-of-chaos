using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy_Gravity : MonoBehaviour {
	List<Rigidbody2D> objects = new List<Rigidbody2D>();


	void FixedUpdate() {
		foreach (Rigidbody2D rb in objects)
		{
			Debug.DrawLine(transform.position, rb.transform.position);
			// calculate direction from target to me
			Vector3 forceDirection = transform.position - rb.transform.position;

			// apply force on target towards me
			rb.AddForce(forceDirection.normalized * 1000 * Time.fixedDeltaTime);
		}
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		var rb = collider.gameObject.GetComponent<Rigidbody2D>();
		if (rb)
		{
			objects.Add(rb);
		}
	}

	void OnTriggerExit2D(Collider2D collider)
	{
		var rb = collider.gameObject.GetComponent<Rigidbody2D>();
		if (rb)
		{
			objects.Remove(rb);
		}
	}
}
