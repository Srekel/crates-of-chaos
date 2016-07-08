using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class RadialButton : MonoBehaviour {
	private const float BUTTON_RADIAL_DISTANCE = 45f;
	public Image ButtonSprite;
	public ButtonType CurrentType;
	private RadialMenu currentMenu;
	private int currentPosition;
	private RectTransform rectTransform;

	private float currentPower = 1f;
	private float targetScale;
	private float scaleAnimationSpeed;
	private bool interactionEnabled = true;

	[Serializable]
	public enum ButtonType
	{
		Cancel,
		BuildBasicShooter,
		BuildBasicSpawner
	}

	public void Setup(ButtonType type, RadialMenu menu, int position) {
		CurrentType = type;
		currentMenu = menu;
		currentPosition = position;
		ButtonSprite.sprite = GetSprite ();
		rectTransform = GetComponent<RectTransform> ();
		// TODO: Animate this.
		rectTransform.position = GetRadialPosition ();

	}

	Vector2 GetRadialPosition() {
		Vector2 startingPosition = currentMenu.GetComponent<RectTransform> ().localPosition;
		float angle = ((90f - (currentMenu.CurrentNumberOfButtons * 5f)) * currentPosition);
		// offset
		angle -= 80f;
		startingPosition.x += Mathf.Sin (angle * Mathf.Deg2Rad) * BUTTON_RADIAL_DISTANCE;
		startingPosition.y += Mathf.Cos (angle * Mathf.Deg2Rad) * BUTTON_RADIAL_DISTANCE;
		return startingPosition;
	}

	Sprite GetSprite() {
		switch (CurrentType) {
		case ButtonType.BuildBasicShooter:
			return SpriteManager.Instance.BasicTower;
		case ButtonType.BuildBasicSpawner:
			return SpriteManager.Instance.BasicCrystal;
			break;
		}
		return SpriteManager.Instance.Cancel;
	}

	// Use this for initialization
	void Start () {
		transform.localScale = new Vector3(.1f, .1f, 1f);
		targetScale = 1f;
		scaleAnimationSpeed = 15f;
	}
	
	// Update is called once per frame
	void Update () {
		rectTransform.position = GetRadialPosition ();
		// TODO: Make not terrible

		if (Input.GetMouseButton (0)) {
			currentPower += Time.deltaTime * .5f;
		} else {
			currentPower = 1f;
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


	public void Clicked() {
		//if(interactionEnabled)
			currentMenu.ButtonPressed (CurrentType, currentPower);
	}

	public void Close() {
		targetScale = 0f;
		scaleAnimationSpeed = 10f;
	}
}
