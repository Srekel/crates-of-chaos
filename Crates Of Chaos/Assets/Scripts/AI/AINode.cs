using UnityEngine;
using System.Collections;

public class AINode : MonoBehaviour {

	// Use this for initialization
	void Start () {
		AINodeSystem.instance.AddNode(this);
	}

	void OnDestroy()
	{
		AINodeSystem.instance.RemoveNode(this);
	}
}
