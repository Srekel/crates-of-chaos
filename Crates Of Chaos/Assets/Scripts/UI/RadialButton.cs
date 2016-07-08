using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class RadialButton : MonoBehaviour {
	private const float BUTTON_RADIAL_DISTANCE = 45f;
	public Image ButtonSprite;
	public ButtonType CurrentType;
	private RadialMenu currentMenu;
	private BuildingInfo currentBuildingInfo;
	private int currentPosition;
	private RectTransform rectTransform;

	private float currentPower = 1f;
	private float targetScale;
	private float scaleAnimationSpeed;
	private bool interactionEnabled = true;
	private bool mouseHovering;
	private int intPower;
	private int basicCost;
	private int redCost;

	[Serializable]
	public enum ButtonType
	{
		BuildBasicSpawner,
		BuildBasicShooter,
		BuildRedShooter,
		DestroyTower
	}

	public void Setup(ButtonType type, RadialMenu menu, int position) {
		CurrentType = type;
		currentMenu = menu;
		currentBuildingInfo = currentMenu.buildingInfo;
		currentPosition = position;
		ButtonSprite.sprite = GetSprite ();
		rectTransform = GetComponent<RectTransform> ();
		// TODO: Animate this.
		rectTransform.position = GetRadialPosition ();
		if (type == ButtonType.BuildBasicSpawner) {
			basicCost = 2;
		}

	}

	Vector2 GetRadialPosition() {
		Vector2 startingPosition = currentMenu.GetComponent<RectTransform> ().localPosition;
		float angle = ((70f - (currentMenu.CurrentNumberOfButtons * 5f)) * currentPosition);
		// offset
		angle -= 70f;
		startingPosition.x += Mathf.Sin (angle * Mathf.Deg2Rad) * BUTTON_RADIAL_DISTANCE;
		startingPosition.y += Mathf.Cos (angle * Mathf.Deg2Rad) * BUTTON_RADIAL_DISTANCE;
		return startingPosition;
	}

	Sprite GetSprite() {
		switch (CurrentType) {
		case ButtonType.BuildBasicShooter:
			return SpriteManager.Instance.BasicTower;
		case ButtonType.BuildBasicSpawner:
			return SpriteManager.Instance.CrystalTower;
		case ButtonType.BuildRedShooter:
			return SpriteManager.Instance.RedTower;
			break;
		}
		return SpriteManager.Instance.Cancel;
	}

	void UpdateStrenthAndCost() {
		int strength = Mathf.FloorToInt (currentPower);
		currentBuildingInfo.strengthLabel.text = "Strength: " + strength.ToString ();
		basicCost = strength;
		if(CurrentType == ButtonType.BuildRedShooter) {
			redCost = 1 + strength;
			currentBuildingInfo.textCost2.text = redCost.ToString ();
		}
		currentBuildingInfo.textCost1.text = basicCost.ToString ();

		if (Input.GetMouseButton(0) 
			&& (basicCost >= ResourceSystem.instance.GetResources (ResourceSystem.ResourceType.Basic) 
				|| redCost >= ResourceSystem.instance.GetResources (ResourceSystem.ResourceType.Red)))
			Clicked ();
	}

	// Use this for initialization
	void Start () {
		transform.localScale = new Vector3(.1f, .1f, 1f);
		targetScale = 1f;
		scaleAnimationSpeed = 15f;
	}
	
	// Update is called once per frame
	void Update () {
		if (currentMenu != null) {
			rectTransform.position = GetRadialPosition ();
			// TODO: Make not terrible
			if(interactionEnabled && (CurrentType == ButtonType.BuildBasicShooter || CurrentType == ButtonType.BuildRedShooter)) {
				if (Input.GetMouseButton (0) && mouseHovering) {
					if (currentPower < 2f) {
						currentPower += Time.deltaTime;
					} else if (currentPower < 5f) {
						currentPower += Time.deltaTime * 4f;
					} else if (currentPower < 10f) {
						currentPower += Time.deltaTime * 10f;
					} else {
						currentPower += Time.deltaTime * 25f;
					}
					targetScale = Mathf.Sqrt (currentPower);
					if (targetScale > 2.5f)
						targetScale = 2.5f;
					UpdateStrenthAndCost ();
				} else {
					currentPower = 1f;
				}

			}



			Vector3 scale = transform.localScale;
			if (scale.x < targetScale) {
				scale.x += Time.deltaTime * scaleAnimationSpeed;
				if (scale.x > targetScale)
					scale.x = targetScale;
				scale.y = scale.x;
				transform.localScale = scale;
			} else if (scale.x > targetScale) {
				scale.x -= Time.deltaTime * scaleAnimationSpeed;
				if (scale.x < targetScale)
					scale.x = targetScale;
				scale.y = scale.x;
				transform.localScale = scale;
				if (scale.x == 0)
					Destroy (gameObject);
			}
		}

	}


	public void Clicked() {
		if (interactionEnabled) {
			if(basicCost <= ResourceSystem.instance.GetResources (ResourceSystem.ResourceType.Basic) 
				&& redCost <= ResourceSystem.instance.GetResources (ResourceSystem.ResourceType.Red)) {
				ResourceSystem.instance.RemoveResources (ResourceSystem.ResourceType.Basic, basicCost);
				ResourceSystem.instance.RemoveResources (ResourceSystem.ResourceType.Red, redCost);
				currentMenu.ButtonPressed (CurrentType, Mathf.FloorToInt (currentPower));
			}

		}
	}

	public void Close() {
		targetScale = 0f;
		scaleAnimationSpeed = 10f;
		interactionEnabled = false;
	}

	public void PointerEnter() {
		currentMenu.PointerEnteredButton (CurrentType);
		if(CurrentType == ButtonType.BuildBasicShooter || CurrentType == ButtonType.BuildRedShooter)
			UpdateStrenthAndCost ();
		mouseHovering = true;
	}

	public void PointerExit() {
		mouseHovering = false;
		currentMenu.PointerExitedButton (CurrentType);
		if (interactionEnabled) {
			targetScale = 1;
			transform.localScale = Vector3.one;
		}
	}
}
