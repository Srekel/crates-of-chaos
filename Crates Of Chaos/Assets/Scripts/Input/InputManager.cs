using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {
    public InputSphere radial_grabber;
    public Camera camera;
    public LayerMask RaycastLayer;
    private bool detecting_input;
    private RaycastHit lastHit;

	// Use this for initialization
	void Start () {
        detecting_input = true;
	}

    // Update is called once per frame
    void Update() {
        if (detecting_input && Input.GetMouseButton(0)) {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out lastHit, 1000f, RaycastLayer))
            {
                UpdateRadialGrabber();
            } else {
                Debug.LogError("Mouse did not hit background wall, how did we get here?");
            }
        } else  {
            UpdateRadialGrabber();
        }
       
        
    }

    void UpdateRadialGrabber() {
        bool active = (Input.GetMouseButton(0));
        radial_grabber.gameObject.SetActive(active);
        if (active) {
            radial_grabber.SetCurrentTarget(lastHit.point);

        }
    }

    void CrateSelected(GameObject crate) {
        Destroy(crate);
    }
}
