using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {
    public Camera camera;
    public LayerMask LeftClickRaycastLayer;
	public LayerMask RightClickRaycastLayer;
    public bool detecting_input;
    public GameObject CrateGrabberPrefab;
    public GameObject GenericTowerPrefab;
    private RaycastHit lastHit;

    public static MouseStatus CurrentMouseStatus {get; internal set;}
    public enum MouseStatus {
        Nothing,
        LeftMouseDownFirstFrame,
        LeftMouseDown,
        RightMouseDownFirstFrame,
        RightMouseDown
    }

	void Start () {
        detecting_input = true;
	}

    void Update() {
        UpdateMouseStatus();
        if (detecting_input) {

            if(CurrentMouseStatus == MouseStatus.LeftMouseDownFirstFrame || CurrentMouseStatus == MouseStatus.RightMouseDownFirstFrame) {
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
				LayerMask layerMask = CurrentMouseStatus == MouseStatus.LeftMouseDownFirstFrame ? LeftClickRaycastLayer : RightClickRaycastLayer;
				float sphereRadius = CurrentMouseStatus == MouseStatus.LeftMouseDownFirstFrame ? 0f : 2f;
				if (Physics.SphereCast(ray, 2f, out lastHit, 1000f, layerMask))
                {
                    if (CurrentMouseStatus == MouseStatus.LeftMouseDownFirstFrame)
                    {
						if (lastHit.collider.gameObject.CompareTag ("GameWall")) {
							CreateRaidalGrabber (lastHit.point);
						} else {
							// Create menu
							RadialMenu.BuildingClicked(lastHit.collider.gameObject);
						}
                    }
                    else if(CurrentMouseStatus == MouseStatus.RightMouseDownFirstFrame) {
						if (lastHit.collider.gameObject.CompareTag ("Crate")) {
							TurnIntoGenericTower(lastHit.collider.gameObject);
						}
                    }
                    
                }
				else if(CurrentMouseStatus == MouseStatus.LeftMouseDownFirstFrame)
                {
                    Debug.LogError("Mouse did not hit background wall, how did we get here?");
                }
            }
        }
		if (Input.GetKey(KeyCode.Space))
		{
			Time.timeScale = 0.1f;
			Time.fixedDeltaTime = 0.02F * Time.timeScale;
		}
        else
		{
			Time.timeScale = 1f;
			Time.fixedDeltaTime = 0.02F * Time.timeScale;
		}
	}

    void CreateRaidalGrabber(Vector3 position) {
		position.z = 0;
        GameObject crateGrabber = (GameObject)Instantiate(CrateGrabberPrefab, position, Quaternion.identity);
        InputSphere inputSphere = crateGrabber.GetComponent<InputSphere>();
        inputSphere.manager = this;
    }

    void TurnIntoGenericTower(GameObject crystal) {
        GameObject tower = (GameObject)Instantiate(GenericTowerPrefab, crystal.transform.position, crystal.transform.rotation);
		Rigidbody2D towerRigidBody = tower.GetComponent<Rigidbody2D> ();
		Rigidbody2D crystalRigidBody = crystal.transform.parent.gameObject.GetComponent<Rigidbody2D> ();
		towerRigidBody.velocity = crystalRigidBody.velocity;
		Destroy(crystal.transform.parent.gameObject);
    }

    /// <summary>
    /// Should help make mouse detection a little cleaner. NOTE: Right mouse takes priority, so if both the left and right mouse buttons are down the status will choose the right mouse.
    /// </summary>
    void UpdateMouseStatus() {
        if(Input.GetMouseButtonDown(1)) {
            CurrentMouseStatus = MouseStatus.RightMouseDownFirstFrame;
            return;
        }

        if(Input.GetMouseButtonDown(0)) {
            CurrentMouseStatus = MouseStatus.LeftMouseDownFirstFrame;
            return;
        }

        if(Input.GetMouseButton(1)) {
            CurrentMouseStatus = MouseStatus.RightMouseDown;
            return;
        }

        if(Input.GetMouseButton(0)) {
            CurrentMouseStatus = MouseStatus.LeftMouseDown;
            return;
        }
        
        CurrentMouseStatus = MouseStatus.Nothing;
    }

    public void CrateSelected(GameObject crate) {
        Destroy(crate);
    }
}
