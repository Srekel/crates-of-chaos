using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class RadialMenu : MonoBehaviour {
	public GameObject RadialButtonPrefab;
	public int CurrentNumberOfButtons;
	public GameObject backgroundClickDetector;
	List<RadialButton> createdButtons;
	private static RadialMenu Instance;
	private GameObject currentBuilding;
	private RectTransform rectTransform;

	public static void BuildingClicked(GameObject building) {
		Instance.SetupMenu (building.transform.parent.gameObject);
    }

	void SetupMenu(GameObject building) {
		currentBuilding = building;
		Vector2 buildingPosition = Camera.main.WorldToScreenPoint (building.transform.position);
		rectTransform.localPosition = buildingPosition;
		SetupForBasicTower ();
		backgroundClickDetector.SetActive (true);
		Time.timeScale = .25f;
	}

	void SetupForBasicTower() {
		CurrentNumberOfButtons = 3;
		for(int i = 0; i < CurrentNumberOfButtons; i++) {
			GameObject buttonObj = (GameObject)Instantiate (RadialButtonPrefab, transform.position, Quaternion.identity);
			RadialButton button = buttonObj.GetComponent<RadialButton> ();
			RadialButton.ButtonType type = RadialButton.ButtonType.Cancel;
			if (i == 0) {
				type = RadialButton.ButtonType.BuildBasicShooter;
			} else if (i == 1) {
				type = RadialButton.ButtonType.BuildBasicSpawner;
			}
			button.Setup (type, this, i);
			buttonObj.transform.SetParent (transform.parent);
			createdButtons.Add (button);
		}
	}

	void Awake () {
		Instance = this;
		rectTransform = GetComponent<RectTransform> ();
		createdButtons = new List<RadialButton> ();
	}
	
	void Update () {
	
	}

	public void ButtonPressed(RadialButton.ButtonType buttonType, float power) {
		CloseButtons ();
	}

	void CloseButtons() {
		foreach(RadialButton b in createdButtons) {
			b.Close ();
		}
		createdButtons.Clear ();
		backgroundClickDetector.SetActive (false);
		Time.timeScale = 1f;
	}

	public void BackgroundPressed() {
		CloseButtons ();
	}
}
