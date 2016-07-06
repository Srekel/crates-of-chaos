using UnityEngine;
using System.Collections;

public class Seeker_Thrust : MonoBehaviour {

	public GameObject target;
	private Rigidbody2D rigid_body;

	// Use this for initialization
	void Start () {
		rigid_body = gameObject.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		//if (transform.up)
		//{

		//}
		rigid_body.AddForce(transform.up);
	}
}
