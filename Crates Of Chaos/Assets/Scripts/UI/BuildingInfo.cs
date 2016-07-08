using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuildingInfo : MonoBehaviour {
	public Text textCost1;
	public Text textCost2;
	public Image imageCost1;
	public Image imageCost2;
	public Text towerName;
	public Text instructions;
	public Animator animator;

	private int basicCost;
	private int redCost;
	private int blueCost;


	private RadialButton.ButtonType currentType;

	public void Setup(RadialButton.ButtonType buttonType) {
		currentType = buttonType;
		animator.Play ("BuildingInfoActivate");

		redCost = blueCost = 0;
		basicCost = 1;
		imageCost2.gameObject.SetActive (buttonType != RadialButton.ButtonType.BuildBasicShooter);
		if (buttonType == RadialButton.ButtonType.BuildBasicShooter) {
			imageCost2.gameObject.SetActive (false);
		} else if (buttonType == RadialButton.ButtonType.BuildRedShooter) {
			
		}

		textCost1.text = basicCost.ToString();
		if (redCost > 0)
			textCost2.text = redCost.ToString ();
		else if (blueCost > 0)
			textCost2.text = blueCost.ToString ();
		else
			textCost2.text = "";
	}

	void Start () {
	
	}

	void Update () {
	
	}

	public void Deactivate() {
		
	}
}
