using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class RadialMenu : MonoBehaviour {
	public GameObject RadialButtonPrefab;
	public int CurrentNumberOfButtons;
	public GameObject backgroundClickDetector;
	public BuildingInfo buildingInfo;
	public GameObject BasicTowerPrefab;
	public GameObject AdvancedTowerPrefab;
	public GameObject CrystalGeneratorPrefab;
	List<RadialButton> createdButtons;
	private static RadialMenu Instance;
	private GameObject currentBuilding;
	private bool menuActive;
	private RectTransform rectTransform;

	public static void BuildingClicked(GameObject building) {
		Instance.SetupMenu (building.transform.parent.gameObject);
    }

	void SetupMenu(GameObject building) {
		currentBuilding = building;

		if (building.GetComponentInChildren<Upgradable>() != null)
		{
			SetupForUpgradable(building);
		}
		else
		{
			SetupForBasicTower();
		}
		UpdatePosition ();
		buildingInfo.gameObject.SetActive (true);
		backgroundClickDetector.SetActive (true);
		buildingInfo.Deactivate ();
		menuActive = true;
		Time.timeScale = .25f;
	}

	void SetupForBasicTower() {
		CurrentNumberOfButtons = 3;
		for(int i = 0; i < CurrentNumberOfButtons; i++) {
			GameObject buttonObj = (GameObject)Instantiate (RadialButtonPrefab, transform.position, Quaternion.identity);
			RadialButton button = buttonObj.GetComponent<RadialButton> ();
			RadialButton.ButtonType type = RadialButton.ButtonType.BuildBasicSpawner; // RadialButton.ButtonType.DestroyTower;
			if (i == 0) {
				type = RadialButton.ButtonType.BuildBasicSpawner;
			} else if (i == 1) {
				type = RadialButton.ButtonType.BuildBasicShooter;
			} else if (i == 2) {
				type = RadialButton.ButtonType.BuildRedShooter;
			}
			button.Setup (type, this, i, true);
			buttonObj.transform.SetParent (transform);
			createdButtons.Add (button);
		}
	}

	void SetupForUpgradable(GameObject building) {
		CurrentNumberOfButtons = 1;
		GameObject buttonObj = (GameObject)Instantiate (RadialButtonPrefab, transform.position, Quaternion.identity);
		RadialButton button = buttonObj.GetComponent<RadialButton> ();
		RadialButton.ButtonType type = RadialButton.ButtonType.DestroyTower;

		button.Setup (type, this, 3, GameState.instance.CanSacrifice(building));
		buttonObj.transform.SetParent (transform);
		createdButtons.Add (button);

		if (!GameState.instance.CanSacrifice(building))
		{
			buildingInfo.instructionsSacrifice.text = "Can't sacrifice. Tower has strength " + building.GetComponentInChildren<Upgradable>().strength + ". A level 3 tower with strength " + GameState.instance.needed_sacrifice + " is needed.";
		}
		else
		{
			buildingInfo.instructionsSacrifice.text = "";
		}
	}

	void Awake () {
		Instance = this;
		rectTransform = GetComponent<RectTransform> ();
		createdButtons = new List<RadialButton> ();
	}
	
	void Update () {
		if (currentBuilding != null) {
			UpdatePosition ();
		} else if (menuActive) {
			// The building got destroyed
			CloseMenu();
		}
	}

	void UpdatePosition() {
		Vector2 buildingPosition = Camera.main.WorldToScreenPoint (currentBuilding.transform.position);
		rectTransform.localPosition = buildingPosition;
	}


	public void ButtonPressed(RadialButton.ButtonType buttonType, int power) {
		
		if (buttonType == RadialButton.ButtonType.DestroyTower) {
			GameState.instance.Sacrifice(currentBuilding);
		} else {
			GameObject prefab;
			if (buttonType == RadialButton.ButtonType.BuildBasicShooter)
				prefab = BasicTowerPrefab;
			else if(buttonType == RadialButton.ButtonType.BuildRedShooter)
				prefab = AdvancedTowerPrefab;
			else
				prefab = CrystalGeneratorPrefab;
			Vector3 buildingPos = currentBuilding.transform.position;
			Destroy (currentBuilding);
			GameObject createdTower = (GameObject)Instantiate (prefab, buildingPos, Quaternion.identity);
			Upgradable up = createdTower.GetComponentInChildren<Upgradable> ();
			if(up != null)
				up.strength = power;
		}

		CloseMenu ();
	}

	void CloseMenu() {
		foreach(RadialButton b in createdButtons) {
			b.Close ();
		}
		createdButtons.Clear ();
		backgroundClickDetector.SetActive (false);
		menuActive = false;
		currentBuilding = null;
		buildingInfo.gameObject.SetActive (false);
		Time.timeScale = 1f;
	}

	public void BackgroundPressed() {
		CloseMenu ();
	}

	public void PointerEnteredButton(RadialButton.ButtonType buttonType) {
		
		buildingInfo.Setup (buttonType);
	}

	public void PointerExitedButton(RadialButton.ButtonType buttonType) {
		buildingInfo.Deactivate ();
	}
}
