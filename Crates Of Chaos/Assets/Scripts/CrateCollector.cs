using UnityEngine;
using System.Collections;

public class CrateCollector : MonoBehaviour {
	void OnTriggerEnter2D(Collider2D other)
	{
		GameObject.Destroy(other.gameObject);
	}
}
