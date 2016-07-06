using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AINodeSystem : MonoBehaviour {

	List<AINode> nodes = new List<AINode>();

	public static AINodeSystem instance;
	void Awake()
	{
		instance = this;
	}

	internal void AddNode(AINode node)
	{
		nodes.Add(node);
	}

	internal void RemoveNode(AINode node)
	{
		int index = nodes.IndexOf(node);
		nodes[index] = nodes[nodes.Count - 1];
		nodes.RemoveAt(nodes.Count - 1);
	}

	public AINode FindNearbyNode(AINode[] previous_nodes, Vector3 current_position)
	{
		AINode best = null;
		float best_distance = float.MaxValue;
		for (int i = 0; i < nodes.Count; i++)
		{
			AINode node = nodes[i];

			bool ignore = false;
			for (int j = 0; j < previous_nodes.Length; j++)
			{
				if (node == previous_nodes[j])
				{
					ignore = true;
					break;
				}
			}

			if (ignore)
			{
				continue;
			}

			var distance = Vector3.Distance(current_position, node.transform.position);
			var distance_randomed = distance * (1 + Random.value * 0.25);
			if (distance_randomed < best_distance)
			{
				best_distance = distance;
				best = node;
			}
		}

		return best;
	}
}
