using UnityEngine;
using System.Collections;

public class IslandCrystalSlot : MonoBehaviour {

	void OnDrawGizmos()
	{
		Gizmos.DrawCube(transform.position, new Vector3(0.5f, 0.5f, 0.5f));
	}
}
