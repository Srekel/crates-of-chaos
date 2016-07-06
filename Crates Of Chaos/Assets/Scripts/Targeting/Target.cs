using UnityEngine;
using System.Collections;

public class Target : MonoBehaviour {
    public delegate void TargetDestroyedHandler(GameObject target);
    public event TargetDestroyedHandler target_destroyed;


    void OnDestroy() {
        if (target_destroyed != null) {
            target_destroyed(gameObject);
        }
    }
}
