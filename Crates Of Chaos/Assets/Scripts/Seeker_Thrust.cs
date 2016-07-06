using UnityEngine;
using System.Collections;

public class Seeker_Thrust : MonoBehaviour {

	public GameObject target;
	private Rigidbody2D rigid_body;
	bool has_target = false;
	
	void Start () {
		rigid_body = gameObject.GetComponent<Rigidbody2D>();
	}

	public void SetTarget(GameObject target)
	{
		this.target = target;
		target.GetComponent<Target>().target_destroyed += Seeker_Thrust_target_destroyed;
		has_target = true;
	}

	private void Seeker_Thrust_target_destroyed(GameObject target)
	{
		has_target = false;
	}

	void FixedUpdate() {
		if (!has_target)
		{
			return;
		}

		var to_target = target.transform.position - transform.position;
		var wanted_direction = to_target.normalized;
		var direction = Vector3.Lerp(transform.up, wanted_direction, 0.1f);
		rigid_body.AddForce(direction * 10);
		rigid_body.AddForce(Vector3.up * 10);

		float dot = Vector3.Dot(transform.right, wanted_direction);
		if (dot > 0)
		{
			rigid_body.AddTorque(dot * -1);
		}
		else
		{
			rigid_body.AddTorque(dot * -1);
		}

		//Debug.DrawLine(transform.position, target.transform.position);
		//Debug.DrawLine(transform.position + Vector3.up, transform.position + Vector3.up+ transform.right * 3);
		//Debug.DrawLine(transform.position + Vector3.up, transform.position + Vector3.up + transform.up * 5);
		//Debug.DrawLine(transform.position + Vector3.up, transform.position + Vector3.up + wanted_direction * (1+ dot*5));
	}
}
