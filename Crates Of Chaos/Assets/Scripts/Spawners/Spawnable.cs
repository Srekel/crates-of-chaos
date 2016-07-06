using UnityEngine;
using System.Collections;

public class Spawnable : MonoBehaviour {
	[HideInInspector]
	public Rigidbody2D rigid_body;

	void Awake()
	{
		rigid_body = this.gameObject.GetComponent<Rigidbody2D>();
	}
}
