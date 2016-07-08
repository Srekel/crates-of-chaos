using UnityEngine;
using System.Collections;

public class Crystal : MonoBehaviour {
	public ResourceSystem.ResourceType CrystalType;

	void OnDestroy() {
		ResourceSystem.instance.AddResources (CrystalType, 1);
	}
}
