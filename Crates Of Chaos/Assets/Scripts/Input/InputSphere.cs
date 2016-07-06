using UnityEngine;
using System.Collections;

public class InputSphere : MonoBehaviour {
    public InputManager manager {get; set;}
    private const float MAX_SIZE = 3f;
    private const float MIN_SIZE = 0.5f;
    private const float GROWTH_SPEED = 5f;

    private Vector3 currentTarget;

    private void Update() {
        // Scale will always be equal in all dimensions so it's ok to just look at the x scale
        float currentScale = transform.localScale.x;
        if (currentScale < MAX_SIZE)
        {
            currentScale += Time.deltaTime * GROWTH_SPEED;
            if (currentScale > MAX_SIZE)
                currentScale = MAX_SIZE;
            transform.localScale = new Vector3(currentScale, currentScale, currentScale);
        } else {
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Crate")) {
            CrateCollision(other.gameObject);
        }
    }

    private void CrateCollision(GameObject crate) {
        manager.CrateSelected(crate);
    }

    public void SetCurrentTarget(Vector3 position) {
        position.z = 0;
        currentTarget = position;
        transform.position = position;
    }

    void OnEnable() {
        transform.localScale = new Vector3(MIN_SIZE, MIN_SIZE, MIN_SIZE);
    }
}
