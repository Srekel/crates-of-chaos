using UnityEngine;
using System.Collections;

public class InputSphere : MonoBehaviour {
    private Vector3 currentTarget;

    private void Update() {

    }

    public void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("On trigger enter");
        if (other.gameObject.CompareTag("Crate")) {
            CrateCollision(other.gameObject);
        }
    }

    private void CrateCollision(GameObject crate) {
        SendMessageUpwards("CrateSelected", crate);
    }

    public void SetCurrentTarget(Vector3 position) {
        position.z = 0;
        currentTarget = position;
        transform.position = position;
    }
}
