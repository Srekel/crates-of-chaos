using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {
    public Camera camera;
    public LayerMask RaycastLayer;
    public bool detecting_input;
    public GameObject CrateGrabberPrefab;
    private RaycastHit lastHit;

	// Use this for initialization
	void Start () {
        detecting_input = true;
	}

    // Update is called once per frame
    void Update() {
        if (detecting_input && Input.GetMouseButtonDown(0)) {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out lastHit, 1000f, RaycastLayer))
            {
                CreateRaidalGrabber(lastHit.point);
                
            } else {
                Debug.LogError("Mouse did not hit background wall, how did we get here?");
            }
        }
       
        
    }

    void CreateRaidalGrabber(Vector3 position) {
        GameObject crateGrabber = (GameObject)Instantiate(CrateGrabberPrefab, position, Quaternion.identity);
        InputSphere inputSphere = crateGrabber.GetComponent<InputSphere>();
        inputSphere.manager = this;
    }

    public void CrateSelected(GameObject crate) {
        Destroy(crate);
    }
}
