using UnityEngine;
using System.Collections;

public class AINodeFollower : MonoBehaviour {
	public float max_speed = 1;

	private AINode current_node;
	private AINode[] previous_nodes = new AINode[3];
	private Rigidbody2D rigid_body;

	void Start()
	{
		rigid_body = gameObject.GetComponent<Rigidbody2D>();
	}

	void FixedUpdate()
	{
		if (current_node == null)
		{
			// lol unity startup orders
			current_node = AINodeSystem.instance.FindNearbyNode(previous_nodes, transform.position);
		}

		var to_target = current_node.transform.position - transform.position;
		var to_target_direction = to_target.normalized;
		var wanted_direction = new Vector2(to_target_direction.x, to_target_direction.y);
		var current_direction = rigid_body.velocity.normalized;
		bool fast_enough = Vector2.SqrMagnitude(rigid_body.velocity) > max_speed * max_speed;
		bool right_direction = Vector2.Dot(current_direction, wanted_direction) > 0.8;
		if (!(right_direction && fast_enough))
		{
			var force = 10;
			if (Vector3.Distance(transform.position, current_node.transform.position) < 10)
			{
				force = 3;
			}
			rigid_body.AddForce(wanted_direction * force);
		}

		rigid_body.AddForce(-Physics2D.gravity);
		rigid_body.AddForce(-rigid_body.velocity * 0.5f);

		Debug.DrawLine(transform.position, current_node.transform.position);
	}

	void Update()
	{
		if (Vector3.Distance(transform.position, current_node.transform.position) < 4)
		{
			var new_node = AINodeSystem.instance.FindNearbyNode(previous_nodes, current_node.transform.position);
			previous_nodes[0] = previous_nodes[1];
			previous_nodes[1] = previous_nodes[2];
			previous_nodes[2] = new_node;
			current_node = new_node;
		}
	}
}
